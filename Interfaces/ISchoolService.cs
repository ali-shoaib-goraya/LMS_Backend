using Dynamic_RBAMS.DTOs;

namespace Dynamic_RBAMS.Interfaces
{
    public interface ISchoolService
    {
        Task<ApiResponseDto> CreateSchoolAsync(CreateSchoolDto dto);
        Task<ApiResponseDto> GetSchoolByIdAsync(int id);
        Task<ApiResponseDto> GetAllSchoolsByCampusIdAsync(int campusId);    
        Task<ApiResponseDto> UpdateSchoolAsync(int id, UpdateSchoolDto dto);
        Task<ApiResponseDto> DeleteSchoolAsync(int id);
        Task<ApiResponseDto> SoftDeleteSchoolAsync(int id);
    }
}
