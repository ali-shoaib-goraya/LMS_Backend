using LMS.Features.SemesterManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Features.SemesterManagement.Repositories
{
    public interface ISemesterRepository
    {
        Task<IEnumerable<Semester>> GetAllSemestersByCampusAsync(int campusId);
        Task<Semester?> GetSemesterByIdAsync(int semesterId);
        Task<Semester> CreateSemesterAsync(Semester semester);
        Task<bool> UpdateSemesterAsync(Semester semester);
        Task<bool> DeleteSemesterAsync(int semesterId);
        Task<bool> SoftDeleteSemesterAsync(int semesterId);
    }
}
