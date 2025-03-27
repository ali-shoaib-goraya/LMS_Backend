using LMS.Features.CourseManagement.Dtos;

namespace LMS.Features.CourseManagement.Services
{
    public interface ICourseService
    {
        Task<CourseResponseDto?> GetCourseByIdAsync(int courseId);
        Task<IEnumerable<CourseResponseDto>> GetCoursesByDepartmentAsync(int departmentId);
        Task<IEnumerable<CourseResponseDto>> GetCoursesByCampusAsync(int campusId);
        Task<CourseResponseDto> CreateCourseAsync(AddCourseDto createCourseDto);
        Task<CourseResponseDto?> UpdateCourseAsync(int courseId, UpdateCourseDto updateCourseDto);
        Task<bool> SoftDeleteCourseAsync(int courseId);
        Task<bool> HardDeleteCourseAsync(int courseId);
    }
}
