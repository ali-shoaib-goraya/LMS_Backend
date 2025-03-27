using LMS.Data;
using LMS.Features.CampusManagement;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Features.CampusManagement.Repositories
{
    public class CampusRepository : ICampusRepository
    {
        private readonly ApplicationDbContext _context;

        public CampusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Add a new campus and return model
        public async Task<Campus> AddAsync(Campus campus)
        {
            await _context.Campuses.AddAsync(campus);
            await _context.SaveChangesAsync();
            return campus;
        }

        // ✅ Get campus by ID and return model
        public async Task<Campus?> GetByIdAsync(int id)
        {
            return await _context.Campuses
                .FirstOrDefaultAsync(c => c.CampusId == id);
        }

        // ✅ Get all campuses by university
        public async Task<IEnumerable<Campus>> GetAllByUniversityIdAsync(int universityId)
        {
            return await _context.Campuses
                .Where(c => !c.IsDeleted && c.UniversityId == universityId)
                .ToListAsync();
        }

        // ✅ Get campus by name & university ID
        public async Task<Campus?> GetByNameAndUniversityAsync(int universityId, string campusName)
        {
            return await _context.Campuses
                .Where(c => !c.IsDeleted && c.CampusName == campusName && c.UniversityId == universityId)
                .FirstOrDefaultAsync();
        }

        // ✅ Update campus details and return updated model
        public async Task<Campus?> UpdateAsync(Campus campus)
        {
            _context.Campuses.Update(campus);
            await _context.SaveChangesAsync();
            return campus;
        }

        // ✅ Hard delete campus by ID
        public async Task<bool> DeleteAsync(int id)
        {
            var campus = await _context.Campuses
                .Include(c => c.Schools)
                .FirstOrDefaultAsync(c => c.CampusId == id);

            if (campus == null)
            {
                return false;
            }

            // ❌ Prevent deletion if there are linked schools
            if (campus.Schools.Any())
            {
                return false;
            }

            _context.Campuses.Remove(campus);
            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ Soft delete campus by ID (with validation)
        public async Task<bool> SoftDeleteAsync(int id)
        {
            var campus = await _context.Campuses.FindAsync(id);
            if (campus == null || campus.IsDeleted) return false;

            campus.IsDeleted = true;
            _context.Campuses.Update(campus);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
