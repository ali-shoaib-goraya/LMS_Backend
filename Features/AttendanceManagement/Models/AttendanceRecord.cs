using LMS.Features.StudentManagement.Models;

namespace LMS.Features.AttendanceManagement.Models
{
    public class AttendanceRecord
    {   
        public int AttendanceRecordId { get; set; }
        public bool IsPresent { get; set; }

        // Foreign keys and Navigation Properties
        public int ClassSessionId { get; set; }
        public ClassSession ClassSession { get; set; }

        public string StudentId { get; set; } 
        public Student Student { get; set; }

        public string MarkedByTeacherId { get; set; }  // Track who marked attendance
    }

}
