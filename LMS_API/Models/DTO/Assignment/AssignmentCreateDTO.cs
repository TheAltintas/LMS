using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models.DTO.Assignment
{
    public class AssignmentCreateDTO
    {
        [Required(ErrorMessage = "Points of the assignment is required")]
        [Range(0, 1000)]
        public decimal Points { get; set; }

        [Required(ErrorMessage = "Type of the assignment is required")]
        [MaxLength(50)]
        public string Type { get; set; } // 'Quiz', 'Homework'

        [Required(ErrorMessage = "Class level of the assignment is required")]
        [MaxLength(20)]
        public string ClassLevel { get; set; } // 'Grade 10'

        [Required(ErrorMessage = "Subject of the assignment is required")]
        [MaxLength(100)]
        public string Subject { get; set; }
    }
}
