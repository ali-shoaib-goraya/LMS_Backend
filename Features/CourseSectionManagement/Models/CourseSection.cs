using LMS.Features.SemesterManagement;
using LMS.Features.SectionManagement;
using LMS.Features.UserManagement.Models;
using LMS.Features.CourseManagement;
using LMS.Features.SchoolManagement;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using LMS.Features.EnrollmentManagement;

namespace LMS.Features.CourseSectionManagement.Models
{
    public class CourseSection 
    {
        [Key]  
        public int CourseSectionId { get; set; }
        public string CourseSectionName { get; set; } 
        public string? Status { get; set; }
        public string? Section { get; set; }
        public string? Prefix { get; set; } 
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Foriegn Keys and Navigation Properties are defiend here

        public string FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public int SemesterId { get; set; }
        public Semester Semester { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int SchoolId { get; set; } 
        public School School { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    }
}
