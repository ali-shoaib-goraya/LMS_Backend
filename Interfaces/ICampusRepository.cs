using Dynamic_RBAMS.Models;
using Dynamic_RBAMS.DTOs; 
namespace Dynamic_RBAMS.Interfaces
{
    public interface ICampusRepository
    {
        Task AddAsync(Campus campus); 
        Task<CampusResponseDto?> GetByIdAsync(int id);  // Returns null if not found
        Task<IEnumerable<CampusResponseDto>> GetAllByUniversityIdAsync(int UniversityId); 
        Task<Campus?> GetByNameAsync(string name); // For unique name validation
        Task DeleteAsync(Campus campus); 
        Task SoftDeleteAsync(Campus campus); // Soft delete 
        Task UpdateAsync(Campus campus);
        Task<Campus?> GetEntityByIdAsync(int id);
    }
} 

 