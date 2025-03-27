using System.ComponentModel.DataAnnotations;

namespace LMS.Features.BatchManagement.Dtos
{
    public class UpdateBatchDto
    { 
        [Required]   
        public string BatchName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
