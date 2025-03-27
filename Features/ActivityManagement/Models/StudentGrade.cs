using LMS.Features.CourseSectionManagement.Models;
using LMS.Features.StudentManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS.Features.ActivityManagement.Models
{
    public class StudentGrade
    {
        [Key]
        public int StudentGradeId { get; set; }
         
        [Required]
        public string StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        [Required]
        public int CourseSectionId { get; set; }

        [ForeignKey("CourseSectionId")]
        public CourseSection CourseSection { get; set; }

        [Required]
        public decimal FinalPercentage { get; set; } // 85.4%

        [Required]
        public string LetterGrade { get; set; } // A+, A, B+, etc.

        [Required]
        public decimal GPA { get; set; } // 4.0, 3.67, etc.
    }

}
