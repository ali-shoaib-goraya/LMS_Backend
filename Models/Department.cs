using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Models
{   
    public class Department
    {   
        public int Id { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        [StringLength(100, ErrorMessage = "Department Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Short Name cannot exceed 50 characters")]
        public string? ShortName { get; set; }

        [Required(ErrorMessage = "School is required")]
        public int SchoolId { get; set; } 
        public School School { get; set; }
        

        [Range(0, 100, ErrorMessage = "Attendance Percentage must be between 0 and 100")]
        public int AttendancePercentage { get; set; }


        [StringLength(50, ErrorMessage = "Assessment Method cannot exceed 50 characters")]
        public string? AssessmentMethod { get; set; }

        public bool IsActive { get; set; } = true;

        public string? AllowedGPAMethods { get; set; }

        public string? Vision { get; set; }

        public string? Signature { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } // "Academic" or "Administrative"

        [Required(ErrorMessage = "Default GPA Method is required")]
        public string DefaultGPAMethod { get; set; } // "Absolute" or "Relative"
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();

    }
}

 