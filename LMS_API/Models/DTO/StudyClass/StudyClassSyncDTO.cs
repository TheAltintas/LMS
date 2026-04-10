using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models
{
    public class StudyClassSyncDTO
    {
        public int? Id { get; set; } 
        public List<int> StudentIds { get; set; } = new();

        public DateTime? UpdatedDate { get; set; }
    }
}
