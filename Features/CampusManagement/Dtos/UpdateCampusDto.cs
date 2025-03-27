using System.ComponentModel.DataAnnotations;

namespace LMS.Features.CampusManagement.Dtos
{
    public class UpdateCampusDto 
    {
        [Required(ErrorMessage = "Campus Name is required")]
        [StringLength(100, ErrorMessage = "Campus Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Short Name cannot exceed 50 characters")]
        public string ShortName { get; set; }

        public string Address { get; set; }

        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string City { get; set; }

        public string Notes { get; set; }
        public string Type { get; set; } // "Main Campus" or "Sub Campus"
    }
}
