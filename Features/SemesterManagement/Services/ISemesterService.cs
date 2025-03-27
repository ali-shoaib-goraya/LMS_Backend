using LMS.Features.SemesterManagement.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Features.SemesterManagement.Services
{
    public interface ISemesterService
    {
        Task<IEnumerable<SemesterResponseDto>> GetAllSemestersByCampusAsync(int campusId);
        Task<SemesterResponseDto?> GetSemesterByIdAsync(int semesterId);
        Task<SemesterResponseDto> CreateSemesterAsync(CreateSemesterDto createDto);
        Task<SemesterResponseDto> UpdateSemesterAsync(int semesterId, UpdateSemesterDto updateDto);
        Task<bool> DeleteSemesterAsync(int semesterId);
        Task<bool> SoftDeleteSemesterAsync(int semesterId);
    }
}
