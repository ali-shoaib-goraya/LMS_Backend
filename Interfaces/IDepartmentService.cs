using System.Collections.Generic;
using System.Threading.Tasks;
using Dynamic_RBAMS.DTOs;
using Dynamic_RBAMS.DTOs.Dynamic_RBAMS.DTOs;

namespace Dynamic_RBAMS.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponseDto>?> GetDepartmentsByCampusAsync(int campusId);
        Task<DepartmentResponseDto?> GetDepartmentByIdAsync(int departmentId);
        Task<DepartmentResponseDto> CreateDepartmentAsync(CreateDepartmentDto dto);
        Task<DepartmentResponseDto?> UpdateDepartmentAsync(int departmentId, UpdateDepartmentDto dto);
        Task<bool> SoftDeleteDepartmentAsync(int departmentId);
        Task<bool> DeleteDepartmentAsync(int departmentId);
    }
}
