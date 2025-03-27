using LMS.Data;
using LMS.Features.DepartmentManagement;
using LMS.Features.ProgramManagement.Dtos;
using LMS.Features.ProgramManagement;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Features.ProgramManagement.Repositories
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly ApplicationDbContext _context;

        public ProgramRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Programs>> GetProgramsByCampusAsync(int campusId)
        {
            return await _context.Programs
               .Include(p => p.Department)
               .ThenInclude(d => d.School)
               .Where(p => p.Department != null && p.Department.School != null &&
                           p.Department.School.CampusId == campusId &&
                           !p.IsDeleted)
               .ToListAsync();
        }

        public async Task<Programs?> GetProgramByIdAsync(int programId)
        {
            return await _context.Programs
                .Include(p => p.Department)
                .ThenInclude(d => d.School)
                .FirstOrDefaultAsync(p => p.ProgramId == programId && !p.IsDeleted);
        }

        public async Task<bool> IsProgramNameExistsAsync(int campusId, string programName, int? excludingProgramId = null)
        {
            // Guard against null programName parameter
            if (string.IsNullOrEmpty(programName))
                return false;

            return await _context.Programs
                .Include(p => p.Department)
                .ThenInclude(d => d.School)
                .Where(p => p.Department != null &&
                            p.Department.School != null &&
                            p.Department.School.CampusId == campusId &&
                            p.ProgramName.ToLower() == programName.ToLower() &&
                            !p.IsDeleted &&
                            (excludingProgramId == null || p.ProgramId != excludingProgramId))
                .AnyAsync();
        }

        public async Task<Programs> CreateProgramAsync(Programs program)
        {
            var department = await _context.Departments
                .Include(d => d.School)
                .FirstOrDefaultAsync(d => d.DepartmentId == program.DepartmentId);

            if (department == null)
                throw new Exception($"Department not found. Provided DepartmentId: {program.DepartmentId}");

          
            int campusId = department.School.CampusId;

            bool nameExists = await IsProgramNameExistsAsync(campusId, program.ProgramName);
            if (nameExists)
                throw new Exception("A program with this name already exists in the campus.");

            _context.Programs.Add(program);
            await _context.SaveChangesAsync();
            return program;
        }

        public async Task<Programs?> UpdateProgramAsync(Programs program)
        {
            var existingProgram = await _context.Programs.AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProgramId == program.ProgramId);
            if (existingProgram == null)
                return null;

            var department = await _context.Departments
                .Include(d => d.School)
                .FirstOrDefaultAsync(d => d.DepartmentId == program.DepartmentId);
            if (department == null)
                throw new Exception("Invalid DepartmentId provided");

            if (await IsProgramNameExistsAsync(department.School.CampusId, program.ProgramName, program.ProgramId))
                throw new Exception("A program with this name already exists in the campus.");

            _context.Programs.Update(program);
            await _context.SaveChangesAsync();
            return program;
        }

        public async Task<bool> DeleteProgramAsync(int programId)
        {
            var program = await _context.Programs.FindAsync(programId);
            if (program == null)
                return false;

            _context.Programs.Remove(program);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteProgramAsync(int programId)
        {
            var program = await _context.Programs.FindAsync(programId);
            if (program == null || program.IsDeleted)
                return false;

            program.IsDeleted = true;
            program.UpdatedAt = DateTime.UtcNow;

            _context.Programs.Update(program);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
