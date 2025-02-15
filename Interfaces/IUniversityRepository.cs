using Dynamic_RBAMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dynamic_RBAMS.Interfaces
{
    public interface IUniversityRepository
    {
        Task AddAsync(University university);
        Task<University?> GetByIdAsync(int id);
        Task<IEnumerable<University>> GetAllAsync();
        Task<University?> GetByNameAsync(string name);
        Task UpdateAsync(University university);
        Task DeleteAsync(University university); // Hard delete
        Task SoftDeleteAsync(University university); // Soft delete
        Task AddFacultyCampusAsync(FacultyCampus facultyCampus);
    }

}
