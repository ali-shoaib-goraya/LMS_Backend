using Dynamic_RBAMS.Features.DepartmentManagement.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dynamic_RBAMS.Features.DepartmentManagement.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponseDto>?> GetDepartmentsByCampusAsync(int campusId);
        Task<DepartmentResponseDto?> GetDepartmentByIdAsync(int departmentId);
        Task<IEnumerable<DepartmentResponseDto>?> GetAllDepartmentsAsync(); 
        Task<DepartmentResponseDto> CreateDepartmentAsync(CreateDepartmentDto dto); 
        Task<DepartmentResponseDto?> UpdateDepartmentAsync(int departmentId, UpdateDepartmentDto dto);
        Task<bool> SoftDeleteDepartmentAsync(int departmentId);
        Task<bool> DeleteDepartmentAsync(int departmentId);
    }
}
