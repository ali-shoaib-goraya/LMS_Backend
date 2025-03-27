using LMS.Features.AttendanceManagement.Models;

namespace LMS.Features.AttendanceManagement.Dtos
{
    public class AttendanceResponseDto
    {
        public int ClassSessionId { get; set; }
        public DateOnly SessionDate { get; set; }
        public string LectureName { get; set; }
        public int Duration { get; set; }
        public string Venue { get; set; }
        public SessionType Type { get; set; }
        public string Topic { get; set; }
        public List<StudentAttendanceDto> AttendanceRecords { get; set; }
    }

}
