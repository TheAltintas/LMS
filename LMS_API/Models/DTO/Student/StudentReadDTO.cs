
namespace LMS_API.Models.DTO.Student
{
    public class StudentReadDTO
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
