using Dynamic_RBAMS.Features.Common.Dtos;
using Dynamic_RBAMS.Features.SchoolManagement.Dtos;

namespace Dynamic_RBAMS.Features.SchoolManagement.Services
{
    public interface ISchoolService
    {
        Task<ApiResponseDto> CreateSchoolAsync(CreateSchoolDto dto);
        Task<ApiResponseDto> GetSchoolByIdAsync(int id);
        Task<ApiResponseDto> GetAllSchoolsByCampusAsync();
        Task<ApiResponseDto> UpdateSchoolAsync(int id, UpdateSchoolDto dto);
        Task<ApiResponseDto> DeleteSchoolAsync(int id);
        Task<ApiResponseDto> SoftDeleteSchoolAsync(int id);
    }
}
