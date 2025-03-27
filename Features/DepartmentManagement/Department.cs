using LMS.Features.Common.Models;
using LMS.Features.SchoolManagement;
using LMS.Features.ProgramManagement;
using System.ComponentModel.DataAnnotations;

namespace LMS.Features.DepartmentManagement
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
        public ICollection<Programs> Programs { get; set; } = new List<Programs>();

    }
}

