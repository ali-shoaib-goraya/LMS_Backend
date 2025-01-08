using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Models
{
    public class Faculty
    {
        public string FacultyId { get; set; } 
        public ApplicationUser User { get; set; }
        public string Designation { get; set; } // Designation, e.g., Lecturer, Professor, etc.
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public int SchoolID { get; set; }
        public School School { get; set; }
        public ICollection<FacultyCampus> FacultyCampuses { get; set; } = new List<FacultyCampus>();
    }
}
