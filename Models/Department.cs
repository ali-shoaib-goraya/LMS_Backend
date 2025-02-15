using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Models
{   
public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string? ShortName { get; set; } 
        public string? Vision { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public string Type { get; set; }
        public bool IsDeleted { get; set; }
        // Navigation Properties and Foreign Keys are defined here
        public ICollection<DepartmentFaculty> DepartmentFaculties { get; set; }
        public int SchoolId { get; set; }
        public School School { get; set; }
    }
}

