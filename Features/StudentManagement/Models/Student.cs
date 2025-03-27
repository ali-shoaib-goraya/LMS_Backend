using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LMS.Features.UserManagement.Models;
using LMS.Features.CampusManagement;
using LMS.Features.SectionManagement;
using LMS.Features.EnrollmentManagement;
using LMS.Features.AttendanceManagement.Models;

namespace LMS.Features.StudentManagement.Models
{
    public class Student : ApplicationUser
    {
        [Required]
        public string EnrollmentNo { get; set; }
        [Required]
        public string GuardianName { get; set; }
        [Required]
        public string GuardianContact { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }
        // Foreign Keys and Navigation Properties are defiend here

        [Required, ForeignKey("SectionId")]
        public int ProgramBatchSectionId { get; set; } 
        public ProgramBatchSection ProgramBatchSection { get; set; } 
        
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();

        [Required]
        public int CampusId { get; set; }
        
        public Campus? Campus { get; set; }
    }
}

