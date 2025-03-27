using LMS.Features.AttendanceManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace LMS.Features.AttendanceManagement.Dtos
{
    public class MarkAttendanceDto
    {
        [Required(ErrorMessage = "CourseSectionId is Required")]
        public int CourseSectionId { get; set; }

        [Required(ErrorMessage = "SessionDate is Required")]
        public DateOnly SessionDate { get; set; }

        [Required(ErrorMessage = "LectureName is Required")]
        public string LectureName { get; set; }

        [Required(ErrorMessage = "Duration is Required")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Venue is Required")]
        public string Venue { get; set; }

        [Required(ErrorMessage = "Type is Required")]
        public SessionType Type { get; set; }

        [Required(ErrorMessage = "Topic is Required")]
        public string Topic { get; set; }

        [Required(ErrorMessage = "Students is Required")]
        public List<StudentAttendanceDto> Students { get; set; }
    }
    public class StudentAttendanceDto
    {
        [Required(ErrorMessage = "StudentId is Required")]
        public string StudentId { get; set; }
        [Required(ErrorMessage = "IsPresent is Required")]
        public bool IsPresent { get; set; }
    }

}
