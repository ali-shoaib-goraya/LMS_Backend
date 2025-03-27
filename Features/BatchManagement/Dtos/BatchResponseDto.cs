namespace LMS.Features.BatchManagement.Dtos
{
    public class BatchResponseDto
    {
        public int ProgramBatchId { get; set; }
        public string BatchName { get; set; }
         
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; } 

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int ProgramId { get; set; }
    }
}
