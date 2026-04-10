using LMS_API.Models;
using LMS_API.Models.DTO.Assignment;

namespace LMS_API.Services.Contract
{
    public interface IAssignmentService
    {
        Task<Assignment> CreateAssignmentAsync(AssignmentCreateDTO assignmentDTO, int teacherId);
        Task<IEnumerable<Assignment>> GetAllAssignmentsAsync(int teacherId);
        Task<bool> DeleteAssignmentAsync(int id, int teacherId);
    }
}
