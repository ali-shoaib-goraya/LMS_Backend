using System.ComponentModel.DataAnnotations;
using LMS.Features.CampusManagement;
using LMS.Features.DepartmentManagement;

namespace LMS.Features.SchoolManagement
{
    public class School
    {
        public int SchoolId { get; set; }

        [Required(ErrorMessage = "School Name is required")]
        [StringLength(100, ErrorMessage = "School Name cannot exceed 100 characters")]
        public string SchoolName { get; set; }
        [StringLength(50, ErrorMessage = "Short Name cannot exceed 50 characters")]
        public string ShortName { get; set; }
        public string Address { get; set; }
        public bool Academic { get; set; } = true;
        public string City { get; set; }
        public string Notes { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

        // Foreign keys and Navigation properties are defined here 
        public int CampusId { get; set; }
        public Campus Campus { get; set; }
        public ICollection<Department> Departments { get; set; } = new List<Department>();

    }
}