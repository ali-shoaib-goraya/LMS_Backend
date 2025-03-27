namespace LMS.Features.AttendanceManagement.Dtos
{
    public class UpdateAttendanceDto
    {
        public int ClassSessionId { get; set; }
        public List<StudentAttendanceDto> UpdatedAttendance { get; set; }
    }
}
