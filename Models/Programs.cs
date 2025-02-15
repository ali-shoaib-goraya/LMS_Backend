using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Models
{
    public class Programs
    {
        [Key]
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }

        public string Description { get; set; }
         
        public string Code { get; set; }

        public string Duration { get; set; }

        public string DegreeType { get; set; } 

        public int CreditRequired { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }

        // Foreign keys and Navigation properties are defined here

        public Department Department { get; set; }
        public int DepartmentId { get; set; }    

    }
} 
