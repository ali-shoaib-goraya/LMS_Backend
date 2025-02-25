using Dynamic_RBAMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dynamic_RBAMS.Features.DepartmentManagement.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetDepartmentsByCampusAsync(int campusId)
        {
            return await _context.Departments
                .Include(d => d.School)
                .Where(d => d.School.CampusId == campusId && !d.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments
                .Include(d => d.School)
                .Where(d => !d.IsDeleted)
                .ToListAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(int departmentId)
        {
            return await _context.Departments
                .Include(d => d.School)
                .Where(d =>  !d.IsDeleted && d.DepartmentId == departmentId)
                .FirstOrDefaultAsync();
        }

        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department?> UpdateDepartmentAsync(Department department)
        {
            var existingDepartment = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.DepartmentId == department.DepartmentId);
            if (existingDepartment == null)
                return null;

            // Ensure the SchoolId is valid
            var schoolExists = await _context.Schools.AnyAsync(s => s.SchoolId == department.SchoolId);
            if (!schoolExists)
                throw new Exception("Invalid SchoolId provided");

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(); // ✅ Ensures consistency

            var department = await _context.Departments
                .Include(d => d.DepartmentFaculties)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            if (department == null)
                return false;

            // ✅ Get all faculty IDs linked to this department
            var facultyIds = department.DepartmentFaculties
                .Select(df => df.FacultyId)
                .Distinct()
                .ToList();

            // ✅ Find faculty members who are ONLY linked to this department
            var facultyToDelete = await _context.Faculties
                .Where(f => facultyIds.Contains(f.Id))
                .Where(f => !_context.DepartmentFaculties.Any(df => df.FacultyId == f.Id && df.DepartmentId != departmentId)) // Ensure no other department links exist
                .ToListAsync();

            // ✅ Now remove department-faculty mappings
            _context.DepartmentFaculties.RemoveRange(department.DepartmentFaculties);

            // ✅ Delete faculty members if they have no other department
            _context.Faculties.RemoveRange(facultyToDelete);

            // ✅ Finally, delete the department
            _context.Departments.Remove(department);

            await _context.SaveChangesAsync(); // ✅ Single SaveChanges for performance
            await transaction.CommitAsync(); // ✅ Commit transaction

            return true;
        }



        public async Task<bool> SoftDeleteDepartmentAsync(int departmentId)
        {
            var department = await _context.Departments.FindAsync(departmentId);
            if (department == null)
                return false;

            if (department.IsDeleted)
                return false;

            department.IsDeleted = true;
            department.UpdatedAt = DateTime.UtcNow;

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
