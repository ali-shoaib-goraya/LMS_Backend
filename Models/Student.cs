using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Dynamic_RBAMS.Models
{
    public class Student: ApplicationUser
    {
        public string EnrollmentNo { get; set; } 

        public DateTime EnrollmentDate { get; set; }

        public String GuardianName {  get; set; }

        public string GuardianContact { get; set; }  

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }
        
    }
}

