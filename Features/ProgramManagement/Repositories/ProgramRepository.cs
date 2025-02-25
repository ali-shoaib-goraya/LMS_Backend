
using Dynamic_RBAMS.Data;
using Dynamic_RBAMS.Features.DepartmentManagement;
using Dynamic_RBAMS.Features.ProgramManagement.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Dynamic_RBAMS.Features.ProgramManagement.Repositories
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
               .Where(p => p.Department.School.CampusId == campusId && !p.IsDeleted)
               .ToListAsync();
        }

        public async Task<Programs?> GetProgramByIdAsync(int programId)
        {
            return await _context.Programs
                .Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.ProgramId == programId && !p.IsDeleted);
        }

        public async Task<Programs> CreateProgramAsync(Programs program)
        {
            _context.Programs.Add(program);
            await _context.SaveChangesAsync();
            return program;
        }

        public async Task<Programs?> UpdateProgramAsync(Programs program)
        {
            var existingProgram = await _context.Programs.AsNoTracking().FirstOrDefaultAsync(p => p.ProgramId == program.ProgramId);
            if (existingProgram == null)
                return null;

            // Ensure the DepartmentId is valid
            var departmentExists = await _context.Departments.AnyAsync(d => d.DepartmentId == program.DepartmentId);
            if (!departmentExists)
                throw new Exception("Invalid SchoolId provided");

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
            if (program == null)
                return false;

            if (program.IsDeleted)
                return false;

            program.IsDeleted = true;
            program.UpdatedAt = DateTime.UtcNow;

            _context.Programs.Update(program);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
