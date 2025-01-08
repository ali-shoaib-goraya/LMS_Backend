using System.ComponentModel.DataAnnotations;


namespace Dynamic_RBAMS.Models
{ 
    public class School 
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "School Name is required")]
        [StringLength(100, ErrorMessage = "School Name cannot exceed 100 characters")]
        public required string Name { get; set; }

        [StringLength(50, ErrorMessage = "Short Name cannot exceed 50 characters")]
        public string? ShortName { get; set; }

        public string? Address { get; set; }

        public bool Academic { get; set; } = true;

        [Required(ErrorMessage = "Campus Id is required")]
        public int CampusId { get; set; } 
        public Campus Campus { get; set; }

        public string? City { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Department> Departments { get; set; } = new List<Department>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();

    }
}

