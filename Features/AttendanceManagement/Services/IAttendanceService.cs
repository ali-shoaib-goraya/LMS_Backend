using LMS.Features.AttendanceManagement.Dtos;

namespace LMS.Features.AttendanceManagement.Services
{
    public interface IAttendanceService
    {
        Task<int> MarkAttendanceAsync(MarkAttendanceDto markAttendanceDto);
        Task<AttendanceResponseDto> GetAttendanceBySessionAsync(int classSessionId);
        Task<bool> UpdateAttendanceAsync(UpdateAttendanceDto updateAttendanceDto);
        Task<bool> DeleteAttendanceBySessionAsync(int classSessionId);
    }
}
