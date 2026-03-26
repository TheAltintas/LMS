using AutoMapper;
using LMS_API.Data;
using LMS_API.Models;
using LMS_API.Models.DTO.Assignmentset;
using LMS_API.Models.DTO.Assignment;
using LMS_API.Services.Contract;
using Microsoft.EntityFrameworkCore;

namespace LMS_API.Services
{
    public class AssignmentSetService : IAssignmentSetService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AssignmentSetService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<AssignmentSet> CreateAssignmentSetAsync(AssignmentSetCreateDTO assignmentSetDTO)
        {
            try
            {
                if (assignmentSetDTO == null) return null;

                AssignmentSet assignmentSet = _mapper.Map<AssignmentSet>(assignmentSetDTO);
                assignmentSet.CreatedDate = DateTime.Now;

                await _db.AssignmentSets.AddAsync(assignmentSet);
                await _db.SaveChangesAsync();
                return assignmentSet;
            }
            catch (Exception)
            {
                return null;
            }
        }

       public async Task<IEnumerable<AssignmentSetReadDTO>> GetAllAssignmentSetsByTeacherAsync(int teacherId)
        {
            try
            {
                var sets = await _db.AssignmentSets
                    .Include(x => x.AssignmentAssignmentSets)
                        .ThenInclude(link => link.Assignment)
                    .Where(x => x.TeacherId == teacherId)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<AssignmentSetReadDTO>>(sets);
            }
            catch (Exception)
            {
                return Enumerable.Empty<AssignmentSetReadDTO>();
            }
}
        public async Task<bool> AddAssignmentToSetAsync(int assignmentSetId, int assignmentId)
        {
            try
            {
                var assignmentSet = await _db.AssignmentSets
                    .Include(x => x.AssignmentAssignmentSets)
                    .FirstOrDefaultAsync(x => x.Id == assignmentSetId);

                var assignment = await _db.Assignments.FindAsync(assignmentId);

                if (assignmentSet == null || assignment == null)
                    return false;

                // Prevent duplicates
                if (!assignmentSet.AssignmentAssignmentSets.Any(x => x.AssignmentId == assignmentId))
                {
                    assignmentSet.AssignmentAssignmentSets.Add(new AssignmentAssignmentSet
                    {
                        AssignmentSetId = assignmentSetId,
                        AssignmentId = assignmentId
                    });

                    await _db.SaveChangesAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}