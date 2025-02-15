using Dynamic_RBAMS.Data;
using Dynamic_RBAMS.Interfaces;
using Dynamic_RBAMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dynamic_RBAMS.Repos
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

        public async Task<Department?> GetDepartmentByIdAsync(int departmentId)
        {
            return await _context.Departments
                .Include(d => d.School)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId && !d.IsDeleted);
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
            var department = await _context.Departments.FindAsync(departmentId);
            if (department == null)
                return false;

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
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
