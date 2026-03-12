using Microsoft.AspNetCore.Mvc;

namespace LMS_API.Controllers
{
    [Route("api/teacher")]
    [ApiController]
    public class TeacherController:ControllerBase
    {
        [HttpGet]
        public string GetTeacherLogin()
        {
            return "Get request (for teacher login)-Sprint1-API-Get Request";
        }
        [HttpGet("{id:int}")]
        public string GetTeacherById(int id)
        {
            return $"Fetching Teacher Id: {id}";
        }
    }
}
