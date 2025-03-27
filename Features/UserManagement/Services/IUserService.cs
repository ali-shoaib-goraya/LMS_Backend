using LMS.Features.Common.Dtos;
using LMS.Features.UserManagement.Dtos;

namespace LMS.Features.UserManagement.Services
{
    public interface IUserService
    {
        Task<ApiResponseDto> RegisterTeacherAsync(RegisterEmployeeDto dto);
        Task<ApiResponseDto> RegisterUniversityAdminAsync(RegisterUniversityAdminDto dto);
        Task<ApiResponseDto> RegisterCampusAdminAsync(RegisterCampusAdminDto dto);
        Task<List<UsersResponseDto>> GetAllFacultyForCampusAsync(int campusId);
        Task<ApiResponseDto> DeleteUserAsync(string id);
        Task<UsersResponseDto?> GetUserByIdAsync(string id); 
    }
}
