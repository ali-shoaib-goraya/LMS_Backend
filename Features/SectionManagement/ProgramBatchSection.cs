using LMS.Features.BatchManagement;
using LMS.Features.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace LMS.Features.SectionManagement
{
    public class ProgramBatchSection
    {
        [Key]
        public int ProgramBatchSectionId { get; set; }

        public string SectionName { get; set; } 

        public int Capacity { get; set; }

        public bool IsDeleted { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }

        // Foreign Keys and Navigation Properties are defiend here
        public int ProgramBatchId { get; set; } 
        public ProgramBatch ProgramBatch { get; set; }

    }
}
