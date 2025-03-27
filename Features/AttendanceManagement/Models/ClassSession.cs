using LMS.Features.CourseSectionManagement.Models;

namespace LMS.Features.AttendanceManagement.Models
{
    public class ClassSession
    {
        public int ClassSessionId { get; set; }
        public DateOnly SessionDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public string LectureName { get; set; } 
        public int Duration { get; set; } // in minutes 
        public string Venue { get; set; }
        public SessionType Type { get; set; }  // Enum instead of string
        public string Topic { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys and Navigation Properties
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
        public int CourseSectionId { get; set; }
        public CourseSection CourseSection { get; set; }
    }










    public enum SessionType
    {
        Lecture,
        Lab,
        Seminar,
        Other
    }

}
