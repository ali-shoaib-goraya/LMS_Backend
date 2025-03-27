using System.Collections.Generic;
using System.Threading.Tasks;
using LMS.Features.SectionManagement;

namespace LMS.Features.SectionManagement.Repositories
{
    public interface ISectionRepository
    {
        Task<IEnumerable<ProgramBatchSection>> GetSectionsByCampusAsync(int campusId);
        Task<ProgramBatchSection?> GetSectionByIdAsync(int sectionId); 
        Task<ProgramBatchSection> CreateSectionAsync(ProgramBatchSection section);
        Task<ProgramBatchSection?> UpdateSectionAsync(ProgramBatchSection section);
        Task<bool> DeleteSectionAsync(int sectionId);
        Task<bool> SoftDeleteSectionAsync(int sectionId);
        Task<bool> IsSectionNameExistsAsync(int batchId, string sectionName, int? excludingSectionId = null);
    }
}
