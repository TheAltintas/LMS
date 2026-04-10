using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<StudyClass> StudyClasses { get; set; } = new List<StudyClass>();

        public ICollection<AssignmentSet> AssignmentSets { get; set; } = new List<AssignmentSet>();
    }
}