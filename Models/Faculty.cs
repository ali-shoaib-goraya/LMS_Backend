using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Models
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
        public int? DepartmentID { get; set; } 
        public Department Department { get; set; }

        public int? UniversityId { get; set; }
        public University University { get; set; }

        public virtual ICollection<FacultyCampus> FacultyCampuses { get; set; }

        public ICollection<DepartmentFaculty> DepartmentFaculties { get; set; } = new List<DepartmentFaculty>();
    }
}
