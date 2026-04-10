using LMS_API.Models;
using LMS_API.Models.DTO;
using LMS_API.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [Route("api/studyclass")]
    [ApiController]
    public class StudyClassController : ControllerBase
    {
        private readonly IStudyClassService _studyClassService;

        public StudyClassController(IStudyClassService studyClassService)
        {
            _studyClassService = studyClassService;
        }

        [HttpPost]
        public async Task<ActionResult<StudyClassReadDTO>> Create(StudyClassCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _studyClassService.CreateStudyClassAsync(dto);

            return Ok(result);
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _studyClassService.DeleteStudyClassAsync(id);

            if (!success)
                return NotFound($"StudyClass with id {id} not found");

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<StudyClassReadDTO>> AddStudentsToStudyClass(StudyClassSyncDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _studyClassService.AddStudentsToStudyClassAsync(dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}