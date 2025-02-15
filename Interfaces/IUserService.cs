using Dynamic_RBAMS.DTOs.AuthDtos;
using Dynamic_RBAMS.DTOs;

namespace Dynamic_RBAMS.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponseDto> RegisterTeacherAsync(RegisterEmployeeDto dto);
        Task<ApiResponseDto> RegisterUniversityAdminAsync(RegisterUniversityAdminDto dto);
        Task<ApiResponseDto> RegisterCampusAdminAsync(RegisterCampusAdminDto dto);
    }
}
