using System.ComponentModel.DataAnnotations;

namespace LMS.Features.SectionManagement.Dtos
{
    public class CreateSectionDto
    {
        [Required(ErrorMessage = "Section name is required.")]
        [MinLength(1, ErrorMessage = "Section name cannot be empty.")]
        public string SectionName { get; set; }


        [Required(ErrorMessage = "Capacity is required.")]
        public int Capacity { get; set; }


        [Required(ErrorMessage = "Batch id is required.")]
        public int ProgramBatchId { get; set; }
    }
} 
