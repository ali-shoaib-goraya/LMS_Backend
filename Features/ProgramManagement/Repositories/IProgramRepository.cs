using Dynamic_RBAMS.Features.DepartmentManagement;
using Dynamic_RBAMS.Features.ProgramManagement.Dtos;
namespace Dynamic_RBAMS.Features.ProgramManagement.Repositories
{
    public interface IProgramRepository
    {
        Task<IEnumerable<Programs>> GetProgramsByCampusAsync(int campusId);
        Task<Programs?> GetProgramByIdAsync(int programId);
        Task<Programs> CreateProgramAsync(Programs program);  
        Task<Programs?> UpdateProgramAsync(Programs program);  
        Task<bool> DeleteProgramAsync(int programId); 
        Task<bool> SoftDeleteProgramAsync(int programId);
    }
} 
 