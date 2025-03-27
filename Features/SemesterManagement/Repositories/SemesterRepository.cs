using LMS.Data;
using LMS.Features.Common.Models;
using LMS.Features.SemesterManagement;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Features.SemesterManagement.Repositories
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly ApplicationDbContext _context;

        public SemesterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Get all semesters for a specific campus
        public async Task<IEnumerable<Semester>> GetAllSemestersByCampusAsync(int campusId)
        {
            return await _context.Semesters
                .Where(s => s.CampusId == campusId)
                .ToListAsync();
        }

        // 2️⃣ Get semester by ID
        public async Task<Semester?> GetSemesterByIdAsync(int semesterId)
        {
            return await _context.Semesters.FindAsync(semesterId);
        }

        // 3️⃣ Create a new semester
        public async Task<Semester> CreateSemesterAsync(Semester semester)
        {
            _context.Semesters.Add(semester);
            await _context.SaveChangesAsync();
            return semester;
        }

        // 4️⃣ Update an existing semester
        public async Task<bool> UpdateSemesterAsync(Semester semester)
        {
            _context.Semesters.Update(semester);
            return await _context.SaveChangesAsync() > 0;
        }

        // 5️⃣ Hard delete a semester
        public async Task<bool> DeleteSemesterAsync(int semesterId)
        {
            var semester = await _context.Semesters.FindAsync(semesterId);
            if (semester == null) return false;

            _context.Semesters.Remove(semester);
            return await _context.SaveChangesAsync() > 0;
        }

        // 6️⃣ Soft delete a semester (marks as "Deleted" instead of removing)
        public async Task<bool> SoftDeleteSemesterAsync(int semesterId)
        {
            var semester = await _context.Semesters.FindAsync(semesterId);
            if (semester == null) return false;

            semester.Status = "Deleted"; // Assuming "Deleted" indicates soft deletion
            _context.Semesters.Update(semester);
            return await _context.SaveChangesAsync() > 0;
        }

    }  
}
