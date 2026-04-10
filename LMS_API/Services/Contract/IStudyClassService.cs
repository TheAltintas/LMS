using LMS_API.Models;
using LMS_API.Models.DTO;

namespace LMS_API.Services.Contract
{
    public interface IStudyClassService
    {
        Task<StudyClassReadDTO> CreateStudyClassAsync(StudyClassCreateDTO studyClassDTO);
        Task<bool> DeleteStudyClassAsync(int id);
        Task<StudyClassReadDTO?> AddStudentsToStudyClassAsync(StudyClassSyncDTO dto);
    }
}