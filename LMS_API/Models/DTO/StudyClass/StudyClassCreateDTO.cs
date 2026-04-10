using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models.DTO
{
    public class StudyClassCreateDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
