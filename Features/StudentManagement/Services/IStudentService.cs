using LMS.Features.Common.Dtos;
using LMS.Features.StudentManagement.Dtos;

namespace LMS.Features.StudentManagement.Services
{
    public interface IStudentService
    {
        Task<ApiResponseDto> BulkEnrollStudentsAsync(IFormFile file, int batchSectionId);
        Task<ApiResponseDto> EnrollStudentAsync(StudentEnrollmentDto dto);
        Task<StudentResponseDto> GetStudentByIdAsync(string studentId);
        Task<List<StudentResponseDto>> GetAllStudentsByCampusAsync();
        Task<ApiResponseDto> DeleteStudentAsync(string studentId);
    }
}