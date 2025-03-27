using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS.Features.ActivityManagement.Models
{
    public class MCQChoice
    {
        [Key]
        public int MCQChoiceId { get; set; } 

        [Required]
        public string ChoiceText { get; set; } // Option text (e.g., "Paris", "London", "New York")

        public bool IsCorrect { get; set; } // True if this is the correct answer

        // Navigation properties and foreign keys are defined here
        [Required]
        public int SubActivityId { get; set; }

        [ForeignKey("SubActivityId")]
        public SubActivity SubActivity { get; set; }
    }

}
