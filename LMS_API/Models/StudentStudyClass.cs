namespace LMS_API.Models
{
    public class StudentStudyClass
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int StudyClassId { get; set; }
        public StudyClass StudyClass { get; set; }
    }
}