using LMS.Features.BatchManagement.Dtos;
using LMS.Features.SectionManagement.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Features.SectionManagement.Services 
{
    public interface ISectionService
    {
        Task<IEnumerable<SectionResponseDto>?> GetSectionsByCampusAsync();
        Task<SectionResponseDto?> GetSectionByIdAsync(int batchSectionId);
        Task<SectionResponseDto> CreateSectionAsync(CreateSectionDto createBatchSectionDto);
        Task<SectionResponseDto?> UpdateSectionAsync(int batchSectionId, UpdateSectionDto updateBatchSectionDto);
        Task<bool> DeleteSectionAsync(int batchSectionId);
        Task<bool> SoftDeleteSectionAsync(int batchSectionId);
    }
}
