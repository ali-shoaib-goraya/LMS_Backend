using LMS.Features.CourseSectionManagement.Dtos;

namespace LMS.Features.CourseSectionManagement.Services
{
    public interface ICourseSectionService
    {
        Task<CourseSectionResponseDto> CreateCourseSectionAsync(CreateCourseSectionDto dto);
        Task<CourseSectionResponseDto> GetCourseSectionByIdAsync(int id);
        Task<IEnumerable<CourseSectionResponseDto>> GetAllCourseSectionsByCampusAsync();
        Task<CourseSectionResponseDto> UpdateCourseSectionAsync(int id, UpdateCourseSectionDto dto);
        Task<bool> DeleteCourseSectionAsync(int id);
        Task<bool> SoftDeleteCourseSectionAsync(int id);
        Task<IEnumerable<CourseSectionResponseDto>> BulkCreateCourseSectionsAsync(BulkCreateCourseSectionDto dto); 
    }
}
