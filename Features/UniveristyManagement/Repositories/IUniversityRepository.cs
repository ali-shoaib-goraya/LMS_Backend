using LMS.Features.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Features.UniveristyManagement.Repositories
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
