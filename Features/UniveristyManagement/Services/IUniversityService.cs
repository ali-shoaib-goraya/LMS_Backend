using Dynamic_RBAMS.Features.Common.Dtos;
using Dynamic_RBAMS.Features.UniveristyManagement.Dtos;

namespace Dynamic_RBAMS.Features.UniveristyManagement.Services
{
    // IUniversityService.cs
    public interface IUniversityService
    {
        Task<ApiResponseDto> CreateUniversityAsync(CreateUniversityDto dto);
        Task<ApiResponseDto> GetUniversityByIdAsync(int id);
        Task<ApiResponseDto> GetAllUniversitiesAsync();
        Task<ApiResponseDto> DeleteUniversityAsync(int id);
        Task<ApiResponseDto> UpdateUniversityAsync(int id, CreateUniversityDto dto);
        Task<ApiResponseDto> SoftDeleteUniversityAsync(int id);
    }
}
