using Dynamic_RBAMS.Features.Common.Dtos;
using Dynamic_RBAMS.Features.UserManagement.Dtos;

namespace Dynamic_RBAMS.Features.UserManagement.Services
{
    public interface IUserService
    {
        Task<ApiResponseDto> RegisterTeacherAsync(RegisterEmployeeDto dto);
        Task<ApiResponseDto> RegisterUniversityAdminAsync(RegisterUniversityAdminDto dto);
        Task<ApiResponseDto> RegisterCampusAdminAsync(RegisterCampusAdminDto dto);
        Task<List<UsersResponseDto>> GetAllUsersForCampusAsync(int campusId);
        Task<ApiResponseDto> DeleteUserAsync(string id);
    }
}
