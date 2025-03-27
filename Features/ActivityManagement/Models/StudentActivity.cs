using LMS.Features.StudentManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS.Features.ActivityManagement.Models
{
    public class StudentActivity
    {
        [Key]
        public int StudentActivityId { get; set; }

        public decimal ObtainedMarks { get; set; } // Total obtained marks

        public string SubmissionFilePath { get; set; } // File link (for assignments)

        public DateTime SubmittedAt { get; set; } // Submission time

        public bool IsGraded { get; set; } // Whether the submission has been graded

        // Navigation properties and foreign keys are defined here
        [Required]
        public string StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        [Required]
        public int ActivityId { get; set; }

        [ForeignKey("ActivityId")]
        public Activity Activity { get; set; }
        public ICollection<StudentSubActivity> StudentSubActivities { get; set; } = new List<StudentSubActivity>();
    }
}
