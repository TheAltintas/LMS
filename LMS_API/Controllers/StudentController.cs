using LMS_API.Models;
using LMS_API.Models.DTO.Auth;
using LMS_API.Models.DTO.Student;
using LMS_API.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController:ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ITokenService _tokenService;

        public StudentController(IStudentService studentService, ITokenService tokenService)
        {
            _studentService = studentService;
            _tokenService = tokenService;
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(StudentCreateDTO studentDTO)
        {
            try
            {
                if (studentDTO == null)
                {
                    return BadRequest("Student data is required");
                }
                var student = await _studentService.RegisterStudentAsync(studentDTO);
                if (student == null)
                {
                    return Conflict($"'{studentDTO.Email}' already exists.");
                }
                return CreatedAtAction(nameof(CreateStudent), new { id = student.Id }, student);// instead of Ok

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   $"An error occurred while creating the student: {ex.Message} ");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDTO>> LoginStudent(StudentLoginDTO studentLoginDTO)
        {
            try
            {
                var student = await _studentService.AuthenticateAsync(studentLoginDTO);
                if (student == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                var token = _tokenService.GenerateToken(student.Id, student.Email, "Student");
                return Ok(new AuthResponseDTO
                {
                    Token = token,
                    Role = "Student",
                    Email = student.Email,
                    ExpiresAtUtc = _tokenService.GetTokenExpiryUtc()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred while login: {ex.Message} ");
            }

        }
    }
}
