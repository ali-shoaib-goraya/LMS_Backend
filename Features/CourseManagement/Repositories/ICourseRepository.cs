namespace LMS.Features.CourseManagement.Repositories
{
    public interface ICourseRepository
    {
        Task<Course?> AddCourseAsync(Course course);
        Task<Course?> UpdateCourseAsync(Course course);
        Task<Course?> GetCourseByIdAsync(int courseId);
        Task<IEnumerable<Course>> GetCoursesByDepartmentAsync(int departmentId);
        Task<IEnumerable<Course>> GetCoursesByCampusAsync(int campusId);
        Task<bool> CourseExistsAsync(int campusId, string courseName, string courseCode);
        Task<bool> SoftDeleteCourseAsync(int courseId);
        Task<bool> HardDeleteCourseAsync(int courseId);
    }
}
