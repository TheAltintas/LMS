using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models
{
    public class StudyClassCreateDTO
    {
        public string Name { get; set; }
        public int TeacherId { get; set; }
    }
}
