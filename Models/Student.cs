using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Models
{
    public class Student
    {
        public string StudentId { get; set; } 

        public ApplicationUser User { get; set; }

        public string RollNumber { get; set; } 

        public DateTime EnrollmentDate { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        
        public int SchoolId { get; set; }
        public School School { get; set; }

        public int CampusId { get; set; }
        public Campus Campus { get; set; }
    }
}

