using LMS_API.Controllers;
using LMS_API.Models.DTO.Student;
using LMS_API.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Reqnroll;
using Xunit; // We use xUnit for the Asserts

namespace LMS_API.BDD.StepDefinitions
{
    [Binding]
    public class TeacherStudentsStepDefinitions
    {
        // Class-level variables to share data between the steps
        private int _teacherId;
        private Mock<ITokenService> _mockTokenService = new Mock<ITokenService>();
        private Mock<IStudentService> _mockStudentService = new Mock<IStudentService>();
        private StudentController _controller;
        private IActionResult _result;

        [Given("a teacher with ID {int} exists")]
        public void GivenATeacherWithIdExists(int teacherId)
        {
            _teacherId = teacherId;

            // Program the token mock to pretend this teacher is logged in
            _mockTokenService
                .Setup(x => x.TryGetTeacherId(It.IsAny<System.Security.Claims.ClaimsPrincipal>(), out _teacherId))
                .Returns(true);
        }

        [Given("the teacher has created {int} students")]
        public void GivenTheTeacherHasCreatedStudents(int studentCount)
        {
            // Create a fake list with the exact number of students the English text asked for
            var fakeStudents = new List<StudentReadDTO>();
            for (int i = 1; i <= studentCount; i++)
            {
                fakeStudents.Add(new StudentReadDTO { Id = i, FirstName = "BDD", LastName = "Student", Email = $"bdd{i}@student.com" });
            }

            // Program the student mock to return this list
            _mockStudentService
                .Setup(x => x.GetStudentsCreatedByTeacherAsync(_teacherId))
                .ReturnsAsync(fakeStudents);
        }

        [When("the teacher requests their created students")]
        public async Task WhenTheTeacherRequestsTheirCreatedStudents() // Notice this is now 'async Task'
        {
            // Build the controller and call the method
            _controller = new StudentController(_mockStudentService.Object, _mockTokenService.Object);

            // Call the API (use 'var' so it accepts the ActionResult<T>)
            var response = await _controller.GetStudentsCreatedByTeacher();

            // Extract the actual HTTP Result (the 200 OK) and save it to our class variable
            _result = response.Result;

            // Save the result so the 'Then' step can check it
        }

        [Then("the system should return exactly {int} students")]
        public void ThenTheSystemShouldReturnExactlyStudents(int expectedCount)
        {
            // 1. Assert that the controller returned a 200 OK result
            var okResult = _result as OkObjectResult;
            Assert.NotNull(okResult);

            // 2. Extract the actual data (the list of students) from the result
            var returnedStudents = okResult.Value as List<StudentReadDTO>;
            Assert.NotNull(returnedStudents);

            // 3. Assert that the number of students matches the expected count
            Assert.Equal(expectedCount, returnedStudents.Count);
        }
    }
}