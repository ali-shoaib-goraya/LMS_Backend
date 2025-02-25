using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Features.DepartmentManagement.Dtos
{
    public class CreateDepartmentDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Department name must be between 2 and 100 characters.")]
        public string DepartmentName { get; set; }

        [StringLength(20, ErrorMessage = "Short name must not exceed 20 characters.")]
        public string? ShortName { get; set; }

        [StringLength(500, ErrorMessage = "Vision must not exceed 500 characters.")]
        public string? Vision { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "SchoolId is required.")]
        public int SchoolId { get; set; }
    }
}
