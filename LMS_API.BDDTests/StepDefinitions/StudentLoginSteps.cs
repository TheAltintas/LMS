using LMS_API.Controllers;
using LMS_API.Models;
using LMS_API.Models.DTO.Auth;
using LMS_API.Models.DTO.Student;
using LMS_API.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Reqnroll;
using System.Security.Claims;

namespace LMS_API.BDDTests.StepDefinitions
{
    [Binding]
    public class StudentLoginSteps
    {
        private readonly Mock<IStudentService> _studentServiceMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();

        private StudentController _controller = null!;
        private ActionResult<AuthResponseDTO> _result = null!;

        // ── Given ────────────────────────────────────────────────────────────

        [Given("a student exists with email {string} and password {string}")]
        public void GivenAStudentExistsWithEmailAndPassword(string email, string password)
        {
            var loginDTO = new StudentLoginDTO { Email = email, Password = password };
            var student = new Student
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Email = email,
                Password = "hashed"
            };

            // Only authenticate when both email AND password match
            _studentServiceMock
                .Setup(s => s.AuthenticateAsync(It.Is<StudentLoginDTO>(d => d.Email == email && d.Password == password)))
                .ReturnsAsync(student);

            _tokenServiceMock
                .Setup(t => t.GenerateToken(student.Id, student.Email, "Student"))
                .Returns("jwt-token");

            _tokenServiceMock
                .Setup(t => t.GetTokenExpiryUtc())
                .Returns(DateTime.UtcNow.AddHours(1));
        }

        [Given("no student exists with email {string}")]
        public void GivenNoStudentExistsWithEmail(string email)
        {
            // AuthenticateAsync returns null for any attempt with this email
            _studentServiceMock
                .Setup(s => s.AuthenticateAsync(It.Is<StudentLoginDTO>(d => d.Email == email)))
                .ReturnsAsync((Student?)null);
        }

        // ── When ─────────────────────────────────────────────────────────────

        [When("the student logs in with email {string} and password {string}")]
        public async Task WhenTheStudentLogsIn(string email, string password)
        {
            _controller = new StudentController(_studentServiceMock.Object, _tokenServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
                }
            };

            var dto = new StudentLoginDTO { Email = email, Password = password };
            _result = await _controller.LoginStudent(dto);
        }

        // ── Then ─────────────────────────────────────────────────────────────

        [Then("the response should be 200 OK")]
        public void ThenTheResponseShouldBe200()
        {
            Assert.IsType<OkObjectResult>(_result.Result);
        }

        [Then("the response should contain a JWT token with role {string}")]
        public void ThenTheResponseShouldContainAJwtToken(string role)
        {
            var ok = Assert.IsType<OkObjectResult>(_result.Result);
            var response = Assert.IsType<AuthResponseDTO>(ok.Value);
            Assert.False(string.IsNullOrEmpty(response.Token));
            Assert.Equal(role, response.Role);
        }

        [Then("the response should be 401 Unauthorized")]
        public void ThenTheResponseShouldBe401()
        {
            Assert.IsType<UnauthorizedObjectResult>(_result.Result);
        }
    }
}
