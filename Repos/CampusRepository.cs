using Dynamic_RBAMS.Data;
using Dynamic_RBAMS.Models;
using Microsoft.EntityFrameworkCore;
using Dynamic_RBAMS.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dynamic_RBAMS.DTOs;

namespace Dynamic_RBAMS.Repos
{
    public class CampusRepository : ICampusRepository
    {
        private readonly ApplicationDbContext _context;

        public CampusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Campus campus)
        {
            await _context.Campuses.AddAsync(campus);
            await _context.SaveChangesAsync();
        }

        public async Task<CampusResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Campuses
                .Where(c => !c.IsDeleted && c.CampusId == id)
                .Select(c => new CampusResponseDto
                {
                    CampusId = c.CampusId,
                    CampusName = c.CampusName,
                    ShortName = c.ShortName,
                    Address = c.Address,
                    City = c.City,
                    Notes = c.Notes,
                    Type = c.Type,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CampusResponseDto>> GetAllByUniversityIdAsync(int universityId)
        {
            return await _context.Campuses
                .Where(c => !c.IsDeleted && c.UniversityId == universityId)
                .AsNoTracking()
                .Select(c => new CampusResponseDto
                {
                    CampusId = c.CampusId,
                    CampusName = c.CampusName,
                    ShortName = c.ShortName,
                    Address = c.Address,
                    City = c.City,
                    Notes = c.Notes,
                    Type = c.Type,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<Campus?> GetByNameAsync(string name)
        {
            return await _context.Campuses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => !c.IsDeleted && c.CampusName.ToLower() == name.ToLower());
        }

        public async Task DeleteAsync(Campus campus)
        {
            _context.Campuses.Remove(campus);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Campus campus)
        {
            _context.Campuses.Update(campus);
            campus.UpdatedAt = DateTime.UtcNow; // Ensure timestamp is updated
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(Campus campus)
        {
            campus.IsDeleted = true;
            campus.UpdatedAt = DateTime.UtcNow; // Ensure timestamp is updated
            _context.Campuses.Update(campus);
            await _context.SaveChangesAsync();
        }

        public async Task<Campus?> GetEntityByIdAsync(int id)
        {
            return await _context.Campuses.FirstOrDefaultAsync(c => !c.IsDeleted && c.CampusId == id);
        }
    }
}
