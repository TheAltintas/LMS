using LMS_API.Models;
using LMS_API.Models.DTO.Assignment;
using LMS_API.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [Route("api/assignment")]
    [ApiController]
    public class AssignmentController:ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        public AssignmentController(IAssignmentService assignmentService) 
        {
            _assignmentService = assignmentService;
        }
        

        [HttpPost]
        public async Task<ActionResult<Assignment>> CreateAssignment(AssignmentCreateDTO assignmentDTO)
        {
            try
            {
                if (assignmentDTO == null)
                {
                    return BadRequest("Assignment data is required");
                }
                var assignment = await _assignmentService.CreateAssignmentAsync(assignmentDTO);
                if (assignment == null)
                {
                    return BadRequest("Could not create assignment.");
                }
                return CreatedAtAction(nameof(CreateAssignment), new { id = assignment.Id }, assignment);// instead of Ok
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  $"An error occurred while creating the assignment: {ex.Message} ");
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAllAssignments()
        {
            try
            {
                var assignments = await _assignmentService.GetAllAssignmentsAsync();
                return Ok(assignments);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    $"Error retrieving data: {ex.Message}");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var deleted = await _assignmentService.DeleteAssignmentAsync(id);

            if (!deleted) 
            {
                return NotFound($"Assignment with ID {id} not found.");
            }

            return Ok(new { message = $"Record deleted with ID: {id}" });
        }
    }
}
