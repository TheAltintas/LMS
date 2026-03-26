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
        public string Type { get; set; } // 'Delprøve 1', 'Delprøve 2'

        [Required(ErrorMessage = "Class level of the assignment is required")]
        [MaxLength(20)]
        public string ClassLevel { get; set; } // 'A, B or C'

        [Required(ErrorMessage = "Subject of the assignment is required")]
        [MaxLength(100)]
        public string Subject { get; set; }

        [MaxLength(500)]
        public string? PictureUrl { get; set; }
        
        [MaxLength(500)]
        public string? VideoUrl { get; set; }
        
    }
}
