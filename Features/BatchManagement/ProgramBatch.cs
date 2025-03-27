using LMS.Features.Common.Models;
using LMS.Features.ProgramManagement;
using LMS.Features.SectionManagement;
using System.ComponentModel.DataAnnotations;

namespace LMS.Features.BatchManagement
{
    public class ProgramBatch
    {
        
        public int ProgramBatchId { get; set; }

        [Required]
        public string BatchName { get; set; } 

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Foriegn key and Navigation properties are defined here 
        public int ProgramId { get; set; }
        public Programs Program { get; set; }
        public ICollection<ProgramBatchSection> ProgramBatchSections { get; set; } = new List<ProgramBatchSection>();

    }
}
