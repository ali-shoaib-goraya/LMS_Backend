using System.ComponentModel.DataAnnotations;

namespace LMS.Features.SectionManagement.Dtos
{
    public class UpdateSectionDto
    {
        [Required(ErrorMessage = "Section name is required.")]
        [MinLength(1, ErrorMessage = "Section name cannot be empty.")]
        public string SectionName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Capacity is required.")]
        public int Capacity { get; set; }
    }
}
