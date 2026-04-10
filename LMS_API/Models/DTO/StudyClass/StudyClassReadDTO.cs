using LMS_API.Models.DTO.Student;

namespace LMS_API.Models
{
    public class StudyClassReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StudentReadDTO> Students { get; set; } = new();
    }
}
