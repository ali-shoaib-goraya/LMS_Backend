using LMS.Features.CourseSectionManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS.Features.ActivityManagement.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        public decimal Weightage { get; set; } // Contribution to final grade (e.g., 10%)

        [Required]
        public string Title { get; set; } // e.g., "Quiz 1", "Assignment 2"

        [Required]
        public ActivityType Type { get; set; } // Enum: Quiz, Assignment, Midterm, etc.

        public bool IsSubActivityEnabled { get; set; } // If false, grading is on total marks

        public decimal TotalMarks { get; set; } // Total marks for the whole activity

        public DateTime DueDate { get; set; } // Submission deadline

        public bool IsPublished { get; set; } // Visibility control


        // Navigation properties and foreign keys are defined here
        [Required]
        public int CourseSectionId { get; set; }

        [ForeignKey("CourseSectionId")] 
        public CourseSection CourseSection { get; set; } 

        public ICollection<SubActivity> SubActivities { get; set; } = new List<SubActivity>();
    }


    public enum ActivityType
    {
        Quiz,
        Assignment,
        Midterm,
        FinalTerm,
        ClassParticipation,
        Project,
        Presentation
    }

}
