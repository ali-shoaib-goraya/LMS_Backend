using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LMS.Features.DepartmentManagement;
using LMS.Features.UniveristyManagement;
using LMS.Features.CampusManagement;
using LMS.Features.Common.Models;

namespace LMS.Features.UserManagement.Models 
{
    public class Faculty : ApplicationUser
    {
        public string? Designation { get; set; }

        public string? ProfilePicture { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }

        public string? EmploymentType { get; set; }

        public string? Qualification { get; set; }

        public string? EmploymentStatus { get; set; }

        // Navigation Properties and Foreign Keys are defined here

        public int? UniversityId { get; set; }
        public University University { get; set; }

        public virtual ICollection<FacultyCampus> FacultyCampuses { get; set; }

        public ICollection<DepartmentFaculty> DepartmentFaculties { get; set; } = new List<DepartmentFaculty>();
    }
}
