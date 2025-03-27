using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS.Features.ActivityManagement.Models
{
    public class StudentSubActivity
    {
        [Key]
        public int StudentSubActivityId { get; set; }
         
        public decimal ObtainedMarks { get; set; } // Marks obtained for this question

        public string? ResponseText { get; set; } // Answer text (for text-based questions)

        public int? SelectedMCQChoiceId { get; set; } // If MCQ, stores the selected choice

        // Navigation properties and foreign keys are defined here
        [Required]
        public int StudentActivityId { get; set; }

        [ForeignKey("StudentActivityId")]
        public StudentActivity StudentActivity { get; set; }

        [Required]
        public int SubActivityId { get; set; }

        [ForeignKey("SubActivityId")]
        public SubActivity SubActivity { get; set; }

        [ForeignKey("SelectedMCQChoiceId")]
        public MCQChoice SelectedMCQChoice { get; set; }
    }

}
