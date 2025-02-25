using Dynamic_RBAMS.Features.CampusManagement.Dtos;

namespace Dynamic_RBAMS.Features.CampusManagement.Repositories
{
    public interface ICampusRepository
    {
        Task<Campus> AddAsync(Campus campus);
        Task<Campus?> GetByIdAsync(int id);
        Task<IEnumerable<Campus>> GetAllByUniversityIdAsync(int universityId);
        Task<Campus?> GetByNameAndUniversityAsync(int universityId, string campusName);
        Task<Campus?> UpdateAsync(Campus campus);
        Task<bool> DeleteAsync(int id);
        Task<bool> SoftDeleteAsync(int id);
    }
}

