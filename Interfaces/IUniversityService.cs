using Dynamic_RBAMS.DTOs;

namespace Dynamic_RBAMS.Interfaces
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
