using LMS_API.Models;
using LMS_API.Models.DTO.Assignment;

namespace LMS_API.Services.Contract
{
    public interface IAssignmentService
    {
        Task<Assignment> CreateAssignmentAsync(AssignmentCreateDTO assignmentDTO);
        Task<IEnumerable<Assignment>> GetAllAssignmentsAsync();
        Task<bool> DeleteAssignmentAsync(int id);
    }
}
