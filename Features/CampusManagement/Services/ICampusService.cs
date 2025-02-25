using Dynamic_RBAMS.Features.CampusManagement.Dtos;
using Dynamic_RBAMS.Features.Common.Dtos;

namespace Dynamic_RBAMS.Features.CampusManagement.Services
{
    public interface ICampusService
    {
        Task<ApiResponseDto> CreateCampusAsync(CreateCampusDto dto);
        Task<ApiResponseDto> GetCampusByIdAsync(int id);
        Task<ApiResponseDto> GetAllCampusesByUniversityAsync();
        Task<ApiResponseDto> UpdateCampusAsync(int id, UpdateCampusDto dto);
        Task<ApiResponseDto> DeleteCampusAsync(int id);
        Task<ApiResponseDto> SoftDeleteCampusAsync(int id);

    }
}