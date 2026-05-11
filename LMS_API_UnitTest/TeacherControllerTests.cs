using FluentAssertions;
using LMS_API.Controllers;
using LMS_API.Models;
using LMS_API.Models.DTO.Teacher;
using LMS_API.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LMS_API_UnitTest
{
    public class TeacherControllerTests
    {
        private readonly Mock<ITeacherService> _teacherServiceMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly TeacherController _controller;

        public TeacherControllerTests()
        {
            _controller = new TeacherController(_teacherServiceMock.Object, _tokenServiceMock.Object);
        }

        // DDT: different valid teacher inputs all expect 201 Created
        public static IEnumerable<object[]> ValidTeacherData =>
        [
            [new TeacherCreateDTO { FirstName = "Alice", LastName = "Smith",  Email = "alice@school.dk", Password = "pass123" }],
            [new TeacherCreateDTO { FirstName = "Bob",   LastName = "Hansen", Email = "bob@school.dk",   Password = "secret99" }],
            [new TeacherCreateDTO { FirstName = "Sara",  LastName = "Lund",   Email = "sara@school.dk",  Password = "abcdef" }],
        ];

        [Theory]
        [MemberData(nameof(ValidTeacherData))]
        public async Task CreateTeacher_ValidData_ReturnsCreated(TeacherCreateDTO dto)
        {
            var teacher = new Teacher { Id = 1, FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email, Password = dto.Password };

            _teacherServiceMock.Setup(s => s.RegisterTeacherAsync(dto)).ReturnsAsync(teacher);

            var result = await _controller.CreateTeacher(dto);

            var created = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var body = created.Value.Should().BeOfType<TeacherReadDTO>().Subject;

            body.Email.Should().Be(dto.Email);
            body.FirstName.Should().Be(dto.FirstName);
            body.LastName.Should().Be(dto.LastName);
        }

        [Fact]
        public async Task CreateTeacher_NullDTO_ReturnsBadRequest()
        {
            var result = await _controller.CreateTeacher(null!);

            result.Result.Should().BeOfType<BadRequestObjectResult>();

            // Null check must short-circuit before reaching the service - Verify that RegisterTeacherAsync is never called
            _teacherServiceMock.Verify(s => s.RegisterTeacherAsync(It.IsAny<TeacherCreateDTO>()), Times.Never);
        }

        [Fact]
        public async Task CreateTeacher_EmailAlreadyExists_ReturnsConflict()
        {
            var dto = new TeacherCreateDTO { FirstName = "Alice", LastName = "Smith", Email = "existing@school.dk", Password = "pass123" };

            _teacherServiceMock.Setup(s => s.RegisterTeacherAsync(dto)).ReturnsAsync((Teacher?)null);

            var result = await _controller.CreateTeacher(dto);

            result.Result.Should().BeOfType<ConflictObjectResult>();
        }
    }
}
