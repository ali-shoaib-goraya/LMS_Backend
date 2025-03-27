using System.ComponentModel.DataAnnotations;

namespace LMS.Features.SchoolManagement.Dtos
{ 
    public class UpdateSchoolDto
    {
        [Required(ErrorMessage = "School Name is required")]
        [StringLength(100, ErrorMessage = "School Name cannot exceed 100 characters")]
        public required string Name { get; set; }
        [StringLength(50, ErrorMessage = "Short Name cannot exceed 50 characters")]
        public string ShortName { get; set; }
        public string Address { get; set; }
        public bool Academic { get; set; }

        public string City { get; set; }
        public string Notes { get; set; }
    }
}
