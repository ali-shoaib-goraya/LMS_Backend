using Dynamic_RBAMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Dynamic_RBAMS.Features.DepartmentManagement;
using Dynamic_RBAMS.Features.ProgramManagement;
using Dynamic_RBAMS.Features.CourseManagement;
using Dynamic_RBAMS.Features.SchoolManagement;

namespace Dynamic_RBAMS.Features.Common.Repositories
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

                _ when typeof(TEntity) == typeof(Course) => await _context.Courses
                    .Include(c => c.Department)
                    .ThenInclude(d => d.School)
                    .FirstOrDefaultAsync(c => c.CourseId == entityId) as TEntity,

                _ when typeof(TEntity) == typeof(School) => await _context.Schools
                    .FirstOrDefaultAsync(s => s.SchoolId == entityId) as TEntity,

                _ => throw new InvalidOperationException($"Entity type {typeof(TEntity).Name} is not supported.")
            };
        }
    }
}
