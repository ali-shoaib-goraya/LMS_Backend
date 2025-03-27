using LMS.Features.DepartmentManagement.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Features.DepartmentManagement.Services
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
