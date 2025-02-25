namespace Dynamic_RBAMS.Features.DepartmentManagement.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetDepartmentsByCampusAsync(int campusId);
        Task<IEnumerable<Department>> GetAllDepartmentsAsync(); 
        Task<Department?> GetDepartmentByIdAsync(int departmentId); 
        Task<Department> CreateDepartmentAsync(Department department);
        Task<Department?> UpdateDepartmentAsync(Department department); 
        Task<bool> DeleteDepartmentAsync(int departmentId);
        Task<bool> SoftDeleteDepartmentAsync(int departmentId);
    }
}
