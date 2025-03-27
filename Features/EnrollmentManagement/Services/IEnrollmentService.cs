using LMS.Features.CourseSectionManagement.Dtos;
using LMS.Features.EnrollmentManagement.Dtos;

namespace LMS.Features.EnrollmentManagement.Services
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponseDto> EnrollStudentAsync(SingleEnrollmentDto enrollmentDto);
        Task<bool> RemoveEnrollmentAsync(int enrollmentId); 
        Task<List<EnrollmentResponseDto>> BulkEnrollStudentsAsync(BulkEnrollmentDto enrollmentDto);

        Task<IEnumerable<CourseSectionResponseDto>> GetTeacherCourseSectionsAsync();
        Task<IEnumerable<CourseSectionResponseDto>> GetStudentCourseSectionsAsync();
    }

}
