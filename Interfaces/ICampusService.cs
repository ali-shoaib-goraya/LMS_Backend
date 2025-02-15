using Dynamic_RBAMS.DTOs;

namespace Dynamic_RBAMS.Interfaces
{
    public interface ICampusService
    {
        Task<ApiResponseDto> CreateCampusAsync(CreateCampusDto dto);
        Task<ApiResponseDto> GetCampusByIdAsync(int id);
        Task<ApiResponseDto> GetAllCampusesByUniversityIdAsync(int id);  
        Task<ApiResponseDto> UpdateCampusAsync(int id, UpdateCampusDto dto);
        Task<ApiResponseDto> DeleteCampusAsync(int id);
        Task<ApiResponseDto> SoftDeleteCampusAsync(int id);
    }
}