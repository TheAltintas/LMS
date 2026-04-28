using LMS_API.Data;
using LMS_API.Services.Contract;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS_API.Test.Controllers
{
    // Step 1: Create the File and the Test Method
    public class StudentControllerSpeedTests
    {
        private readonly ITestOutputHelper _output;

        // xUnit will automatically pass this in for us!
        public StudentControllerSpeedTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task MeasureSpeed_Mock_Vs_Database()
        {
            // Step 2: Create the Mocks(The "Arrange" Phase)
            var mockTokenService = new Mock<ITokenService>();
            var mockStudentService = new Mock<IStudentService>();

            // Step 3: "Program" the Mocks
            // 1. The controller needs to extract a teacher ID from the logged-in user
            int fakeTeacherId = 1;
            mockTokenService
                .Setup(x => x.TryGetTeacherId(It.IsAny<System.Security.Claims.ClaimsPrincipal>(), out fakeTeacherId))
                .Returns(true);


            // 2. Setup the Student Service:
            var fakeStudents = new List<LMS_API.Models.DTO.Student.StudentReadDTO>
                                        {
                                            new LMS_API.Models.DTO.Student.StudentReadDTO 
                                                { 
                                                    Id = 1, 
                                                    FirstName = "Mock", 
                                                    LastName = "Student", 
                                                    Email = "mock@student.com" 
                                                }
                                        };
            // 3. Tell the mock to return this list when asked
            mockStudentService
                .Setup(x => x.GetStudentsCreatedByTeacherAsync(fakeTeacherId))
                .ReturnsAsync(fakeStudents);

            
            // Step 4: Build the Controller and Measure the Speed
            
            // 1. Build the controller using the "Robots" (we use .Object to pass the actual mocked instance)
            var controller = new LMS_API.Controllers.StudentController(mockStudentService.Object, mockTokenService.Object);

            // 2. Start the timer
            System.Diagnostics.Stopwatch mockTimer = System.Diagnostics.Stopwatch.StartNew();

            // 3. Act: Call the controller method (This fixes your 'await' warning!)
            var mockResult = await controller.GetStudentsCreatedByTeacher();

            // 4. Stop the timer
            mockTimer.Stop();

            // Save the time it took into a variable so we can compare it later
            long timeTakenMocking = mockTimer.ElapsedMilliseconds;

            // Just to see it while debugging, let's write it to the console
            _output.WriteLine($"Mocking took: {timeTakenMocking} ms");


            // Step 5: Setup the In - Memory Database

            // ==========================================
            // TEST 2: THE DATABASE APPROACH
            // ==========================================

            // 1. Configure the In-Memory Database
            var dbOptions = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<LMS_API.Data.ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB name for every test run
                .Options;

            // 2. Open the database connection and add a real student
            using (var dbContext = new LMS_API.Data.ApplicationDbContext(dbOptions))
            {
                // Create a real student entity (not a DTO)
                var realStudent = new LMS_API.Models.Student
                {
                    FirstName = "Database",
                    LastName = "Student",
                    Email = "db@student.com",
                    Password = "hashedpassword", // Add any fields your model strictly requires
                    CreatedByTeacherId = fakeTeacherId
                };

                dbContext.Students.Add(realStudent);
                await dbContext.SaveChangesAsync(); // Save it to the fake database

                // We will measure the speed here in the next step!
                //Step 6: Measure the Database Speed and Compare
                // 3. To use the real service, we need to give it the DB context. 
                // It probably also asks for an IMapper in its constructor. We can just mock the mapper to keep things simple.
                var mockMapper = new Mock<AutoMapper.IMapper>();
                var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<LMS_API.Services.StudentService>>();
                // 4. Create the REAL service using the In-Memory DB
                var realStudentService = new LMS_API.Services.StudentService(dbContext, mockMapper.Object, mockLogger.Object);
                // 5. Create a new controller using the REAL service (we can reuse the mockTokenService)
                var dbController = new LMS_API.Controllers.StudentController(realStudentService, mockTokenService.Object);

                // 6. Start the timer for the database test
                System.Diagnostics.Stopwatch dbTimer = System.Diagnostics.Stopwatch.StartNew();

                // 7. Act: Call the controller method
                var dbResult = await dbController.GetStudentsCreatedByTeacher();

                // 8. Stop the timer
                dbTimer.Stop();
                long timeTakenDb = dbTimer.ElapsedMilliseconds;

                _output.WriteLine($"Database took: {timeTakenDb} ms");

                // ==========================================
                // FINAL ASSERT: Prove to your teacher that Mocking is faster!
                // ==========================================

                // We assert that the Mock time is less than or equal to the DB time
                Assert.True(timeTakenMocking <= timeTakenDb, $"Mocking ({timeTakenMocking}ms) should be faster than DB ({timeTakenDb}ms)");
            }

            
        }

    }
}
