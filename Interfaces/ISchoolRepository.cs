using Dynamic_RBAMS.DTOs;
using Dynamic_RBAMS.Models;

namespace Dynamic_RBAMS.Interfaces
{
    public interface ISchoolRepository
    {
        Task<School?> GetByIdAsync(int id);
        Task<IEnumerable<School>> GetAllByCampusIdAsync(int campusId);
        Task<School?> GetByNameAsync(string schoolName);
        Task<School> AddAsync(School school);
        Task<School?> UpdateAsync(School school);
        Task<bool> DeleteAsync(int id);
        Task<bool> SoftDeleteAsync(int id); 
    }
}
