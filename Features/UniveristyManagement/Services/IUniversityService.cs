using LMS.Features.Common.Dtos;
using LMS.Features.UniveristyManagement.Dtos;

namespace LMS.Features.UniveristyManagement.Services
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
