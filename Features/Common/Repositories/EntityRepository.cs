using LMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LMS.Features.DepartmentManagement;
using LMS.Features.ProgramManagement;
using LMS.Features.CourseManagement;
using LMS.Features.SchoolManagement;
using LMS.Features.CampusManagement;
using LMS.Features.BatchManagement;
using LMS.Features.SectionManagement;

namespace LMS.Features.Common.Repositories
{
    public class EntityRepository : IEntityRepository
    {
        private readonly ApplicationDbContext _context;

        public EntityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity?> GetByIdAsync<TEntity>(int entityId) where TEntity : class
        {
            return typeof(TEntity) switch
            {
                _ when typeof(TEntity) == typeof(Department) => await _context.Departments
                    .Include(d => d.School)
                    .FirstOrDefaultAsync(d => d.DepartmentId == entityId) as TEntity,

                _ when typeof(TEntity) == typeof(Programs) => await _context.Programs
                    .Include(p => p.Department)
                    .ThenInclude(d => d.School)
                    .FirstOrDefaultAsync(p => p.ProgramId == entityId) as TEntity,

                _ when typeof(TEntity) == typeof(ProgramBatch) => await _context.ProgramBatches
                    .Include(pb => pb.Program) 
                    .ThenInclude(p => p.Department)
                    .ThenInclude(d => d.School)
                    .FirstOrDefaultAsync(pb => pb.ProgramBatchId == entityId) as TEntity,

                _ when typeof (TEntity) == typeof(ProgramBatchSection) => await _context.ProgramBatchSections
                    .Include(s => s.ProgramBatch)
                    .ThenInclude(pb => pb.Program)
                    .ThenInclude(p => p.Department)
                    .ThenInclude(d => d.School)
                    .FirstOrDefaultAsync(s => s.ProgramBatchSectionId == entityId) as TEntity,

                _ when typeof(TEntity) == typeof(Course) => await _context.Courses
                    .Include(c => c.Department)
                    .ThenInclude(d => d.School)
                    .FirstOrDefaultAsync(c => c.CourseId == entityId) as TEntity,

                _ when typeof(TEntity) == typeof(School) => await _context.Schools
                    .FirstOrDefaultAsync(s => s.SchoolId == entityId) as TEntity,

                _ when typeof(TEntity) == typeof(Campus) => await _context.Campuses
                    .FirstOrDefaultAsync(c => c.CampusId == entityId) as TEntity,

                _ => throw new InvalidOperationException($"Entity type {typeof(TEntity).Name} is not supported.")
            };
        }
    }
}
