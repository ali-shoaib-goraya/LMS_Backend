namespace LMS.Features.SchoolManagement.Repositories
{
    public interface ISchoolRepository
    {
        Task<School?> GetByIdAsync(int id); 
        Task<IEnumerable<School>> GetAllByCampusIdAsync(int campusId);
        Task<School?> GetByNameAndCampusAsync(int campusId, string schoolName);
        Task<School> AddAsync(School school);
        Task<School?> UpdateAsync(School school);
        Task<bool> DeleteAsync(int id);
        Task<bool> SoftDeleteAsync(int id);
    }
}
