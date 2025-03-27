using System.ComponentModel.DataAnnotations;

namespace LMS.Features.SemesterManagement.Dtos
{
    public class CreateSemesterDto
    {
        [Required(ErrorMessage = "SemesterName is Required")]
        public string SemesterName { get; set; }
        public string? Status { get; set; } = "Active";

        public DateTime? StartDate { get; set; } 

        public DateTime? EndDate { get; set; } 
        public string? Notes { get; set; }

    }
}
