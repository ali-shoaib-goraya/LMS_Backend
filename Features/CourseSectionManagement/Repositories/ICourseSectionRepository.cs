using LMS.Features.CourseSectionManagement.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace LMS.Features.CourseSectionManagement.Repositories
{
    public interface ICourseSectionRepository
    {
        Task<bool> IsFacultyFromSameCampus(string facultyId, int campus);
        Task<bool> CourseSectionExistsAsync(string courseSectionName, int semesterId, int schoolId, string section, int? excludeId= null);
        Task<CourseSection?> AddCourseSectionAsync(CourseSection courseSection); 
        Task<CourseSection?> GetCourseSectionByIdAsync(int id);
        Task<IEnumerable<CourseSection>> GetAllCourseSectionsByCampusAsync(int campusId);
        Task<CourseSection?> UpdateCourseSectionAsync(int id, CourseSection courseSection);
        Task<bool> DeleteCourseSectionAsync(int id);
        Task<bool> SoftDeleteCourseSectionAsync(int id);
        Task BulkAddCourseSectionsAsync(List<CourseSection> courseSections);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
} 

