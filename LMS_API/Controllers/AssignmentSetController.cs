using LMS_API.Models;
using LMS_API.Models.DTO.Assignmentset;
using LMS_API.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [Route("api/assignmentset")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class AssignmentSetController:ControllerBase
    {
        private readonly IAssignmentSetService _assignmentSetService;
        private readonly ITokenService _tokenService;

        public AssignmentSetController(IAssignmentSetService assignmentSetService, ITokenService tokenService)
        {
            _assignmentSetService = assignmentSetService;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<ActionResult<AssignmentSet>> CreateAssignmentSet(AssignmentSetCreateDTO assignmentSetDTO)
        {
            try
            {
                if (assignmentSetDTO == null)
                {
                    return BadRequest("Assignment Set data is required");
                }
                if (!_tokenService.TryGetTeacherId(User, out var teacherId))
                {
                    return Unauthorized("Missing or invalid teacher identity.");
                }

                var assignmentSet = await _assignmentSetService.CreateAssignmentSetAsync(assignmentSetDTO, teacherId);
                if (assignmentSet == null)
                {
                    return BadRequest("Could not create assignment set.");
                }
                return CreatedAtAction(nameof(CreateAssignmentSet), new { id = assignmentSet.Id }, assignmentSet);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  $"An error occurred while creating the assignment set: {ex.Message} ");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssignmentSet>>> GetAllAssignmentSet()
        {
            try
            {
                if (!_tokenService.TryGetTeacherId(User, out var teacherId))
                {
                    return Unauthorized("Missing or invalid teacher identity.");
                }

                var assignmentSet = await _assignmentSetService.GetAllAssignmentSetsByTeacherAsync(teacherId);
                if (assignmentSet == null || !assignmentSet.Any())
                {
                    return NotFound("No assignment sets found.");
                }
                return Ok(assignmentSet);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    $"Error retrieving data: {ex.Message}");
            }
        }
        [HttpPost("{assignmentSetId:int}/add-assignment/{assignmentId:int}")]
        public async Task<ActionResult> AddAssignmentToSet(int assignmentSetId, int assignmentId)
        {
            if (!_tokenService.TryGetTeacherId(User, out var teacherId))
            {
                return Unauthorized("Missing or invalid teacher identity.");
            }

            var result = await _assignmentSetService.AddAssignmentToSetAsync(assignmentSetId, assignmentId, teacherId);
            if (!result) return BadRequest("Could not add assignment to set.");
            return Ok("Assignment added successfully.");
        }
    }
}
