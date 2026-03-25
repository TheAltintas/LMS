using AutoMapper;
using LMS_API.Data;
using LMS_API.Models;
using LMS_API.Models.DTO.Assignment;
using LMS_API.Models.DTO.Teacher;
using LMS_API.Services.Contract;
using Microsoft.EntityFrameworkCore;

namespace LMS_API.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public AssignmentService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<Assignment> CreateAssignmentAsync(AssignmentCreateDTO assignmentDTO)
        {
            try
            {
                if (assignmentDTO == null)
                {
                    return null;
                }

                Assignment assignment = _mapper.Map<Assignment>(assignmentDTO);
                assignment.CreatedDate = DateTime.Now;
                await _db.Assignments.AddAsync(assignment);
                await _db.SaveChangesAsync();
                return assignment;
            }
            catch (Exception ex)
            {
                // Optionally log the exception or handle as needed
                return null;
            }
        }
        public async Task<IEnumerable<Assignment>> GetAllAssignmentsAsync()
        {
            try
            {
                return await _db.Assignments.ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<Assignment>();
            }
        }
        public async Task<bool> DeleteAssignmentAsync(int id)
        {
            try
            {
                var assignment = await _db.Assignments.FindAsync(id);
                if (assignment == null)
                {
                    return false;
                }
                _db.Assignments.Remove(assignment);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}