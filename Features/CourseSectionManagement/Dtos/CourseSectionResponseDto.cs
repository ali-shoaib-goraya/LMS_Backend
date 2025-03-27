using LMS.Features.CampusManagement;
using LMS.Features.CourseManagement;
using LMS.Features.SectionManagement;
using LMS.Features.SemesterManagement;
using LMS.Features.UserManagement.Models;

namespace LMS.Features.CourseSectionManagement.Dtos
{
    public class CourseSectionResponseDto
    {
        public int CourseSectionId { get; set; }
        public string CourseSectionName { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public string? Section { get; set; }
        public string? Prefix { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }

        // Faculty (Teacher) Info
        public string FacultyId { get; set; }
        public string FacultyName { get; set; }

        // Semester Info
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }

        // Course Info
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }

        // School Info
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
    }
}
