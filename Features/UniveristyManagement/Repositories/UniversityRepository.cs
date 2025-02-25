using Dynamic_RBAMS.Data;
using Dynamic_RBAMS.Features.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynamic_RBAMS.Features.UniveristyManagement.Repositories
{
    public class UniversityRepository : IUniversityRepository
    {
        private readonly ApplicationDbContext _context;

        public UniversityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(University university)
        {
            await _context.Universities.AddAsync(university);
            await _context.SaveChangesAsync();
        }

        public async Task<University?> GetByIdAsync(int id)
        {
            return await _context.Universities
                .Where(u => !u.IsDeleted) // Only fetch active universities
                .FirstOrDefaultAsync(u => u.UniversityId == id);
        }

        public async Task<IEnumerable<University>> GetAllAsync()
        {
            return await _context.Universities
                .Where(u => !u.IsDeleted) // Only fetch active universities
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<University?> GetByNameAsync(string name)
        {
            return await _context.Universities
                .AsNoTracking()
                .Where(u => !u.IsDeleted) // Only fetch active universities
                .FirstOrDefaultAsync(u => u.UniversityName.ToLower() == name.ToLower());
        }

        public async Task DeleteAsync(University university)
        {
            _context.Universities.Remove(university); // Hard delete
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(University university)
        {
            university.IsDeleted = true; // Soft delete
            _context.Universities.Update(university);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(University university)
        {
            _context.Universities.Update(university);
            await _context.SaveChangesAsync();
        }
        public async Task AddFacultyCampusAsync(FacultyCampus facultyCampus)
        {
            await _context.FacultiesCampuses.AddAsync(facultyCampus);
            await _context.SaveChangesAsync();
        }
    }

}