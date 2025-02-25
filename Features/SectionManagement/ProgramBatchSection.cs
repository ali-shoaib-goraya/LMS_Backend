using Dynamic_RBAMS.Features.BatchManagment;
using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Features.SectionManagement
{
    public class ProgramBatchSection
    {
        [Key]
        public int SectionId { get; set; }

        public string SectionName { get; set; }

        public int Capacity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }

        // Foreign Keys and Navigation Properties are defiend here
        public int BatchId { get; set; }
        public ProgramBatch ProgramBatch { get; set; }


    }
}
