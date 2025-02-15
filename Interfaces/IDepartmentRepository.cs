using Dynamic_RBAMS.Models;

namespace Dynamic_RBAMS.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetDepartmentsByCampusAsync(int campusId);
        Task<Department?> GetDepartmentByIdAsync(int departmentId);
        Task<Department> CreateDepartmentAsync(Department department);
        Task<Department?> UpdateDepartmentAsync(Department department);
        Task<bool>DeleteDepartmentAsync(int departmentId);
        Task<bool> SoftDeleteDepartmentAsync(int departmentId);
    }
}
