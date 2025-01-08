using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Models
{
    public class Campus  
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campus Name is required")]
        [StringLength(100, ErrorMessage = "Campus Name cannot exceed 100 characters")]
        public required string Name { get; set; }

        [StringLength(50, ErrorMessage = "Short Name cannot exceed 50 characters")]
        public string? ShortName { get; set; }

        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string? City { get; set; }

        public string? Notes { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public required string Type { get; set; } // "Main Campus" or "Sub Campus"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<School>? Schools { get; set; } = new List<School>();

        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<FacultyCampus> FacultyCampuses { get; set; } = new List<FacultyCampus>();
    }
}
