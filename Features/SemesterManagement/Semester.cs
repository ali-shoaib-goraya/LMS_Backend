using System.ComponentModel.DataAnnotations;
using LMS.Features.CampusManagement;
namespace LMS.Features.SemesterManagement
{
    public class Semester
    {
        public int SemesterId { get; set; }
        [Required]
        public string SemesterName { get; set; }

        public string? Status { get; set; } = "Active";

        public DateTime? StartDate { get; set; } = DateTime.Now;

        public DateTime? EndDate { get; set; } 
        public string? Notes { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        //  Foriegn Kays and Navigation properties are defined here
        public int CampusId { get; set; }
        public Campus Campus { get; set; }

    }
}
