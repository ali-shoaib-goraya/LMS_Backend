using Dynamic_RBAMS.Features.ProgramManagement;
using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Features.BatchManagment
{
    public class ProgramBatch
    {
        [Key]
        public int BatchId { get; set; }

        public string BatchName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }

        // Foriegn key and Navigation properties are defined here 
        public int ProgramId { get; set; }
        public Programs Program { get; set; }

    }
}
