using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS.Features.ActivityManagement.Models
{
    public class SubActivity
    {
        [Key]
        public int SubActivityId { get; set; }

        [Required]
        public string Title { get; set; } // Question Text / Grading Criterion Name
        public SubActivityType Type { get; set; } // Enum: MCQ, Text, Rubric, etc.
        public decimal MaxMarks { get; set; } // Maximum marks for this sub-activity
        public bool RequiresManualGrading { get; set; } // True if manual grading is needed


        // Navigation properties and foreign keys are defined here
        [Required]
        public int ActivityId { get; set; }
        public Activity Activity { get; set; } 
        public ICollection<MCQChoice> MCQChoices { get; set; } = new List<MCQChoice>(); // Only for MCQs
    }


    public enum SubActivityType
    {
        MCQ,
        Text,
        Rubric
    }
}
