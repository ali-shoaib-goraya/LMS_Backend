using System.ComponentModel.DataAnnotations;

namespace LMS.Features.SemesterManagement.Dtos
{
    public class UpdateSemesterDto
    {
        [MinLength(1, ErrorMessage = "Semester Name cannot be empty")]
        public string? SemesterName { get; set; } // Nullable for PATCH support

        public string? Status { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Notes { get; set; }
    }
}
