using System.ComponentModel.DataAnnotations;

namespace LMS.Features.BatchManagement.Dtos
{
    public class CreateBatchDto
    {
        [Required]
        public string BatchName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        [Required]
        public int ProgramId { get; set; }
    }
}
