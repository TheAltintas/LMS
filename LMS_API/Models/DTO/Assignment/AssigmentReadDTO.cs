namespace LMS_API.Models.DTO.Assignment
{
    public class AssignmentReadDTO
    {
        public int Id { get; set; }
        public decimal Points { get; set; }
        public string Type { get; set; }
        public string ClassLevel { get; set; }
        public string Subject { get; set; }
        public string? PictureUrl { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}