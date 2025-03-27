using LMS.Features.DepartmentManagement;
using LMS.Features.ProgramManagement.Dtos;
namespace LMS.Features.ProgramManagement.Repositories
{
    public interface IProgramRepository
    {
        Task<IEnumerable<Programs>> GetProgramsByCampusAsync(int campusId);
        Task<Programs?> GetProgramByIdAsync(int programId);
        Task<Programs> CreateProgramAsync(Programs program);  
        Task<Programs?> UpdateProgramAsync(Programs program);  
        Task<bool> DeleteProgramAsync(int programId); 
        Task<bool> SoftDeleteProgramAsync(int programId);
        Task<bool> IsProgramNameExistsAsync(int campusId, string programName, int? excludingProgramId = null);
    }
} 
 