namespace LMS.Features.SectionManagement.Dtos
{
    public class SectionResponseDto
    {
        public int ProgramBatchSectionId { get; set; }
        public string SectionName { get; set; }
         
        public int Capacity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }

        // Foreign Keys and Navigation Properties are defiend here
        public int ProgramBatchId { get; set; }
    }
}
 