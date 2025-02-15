namespace Dynamic_RBAMS.Models
{
    public class CourseSection
    {
        public int CourseSectionId { get; set; }

        public string CourseSectionName { get; set; }

        public string Status { get; set; }  
        public string EnrolledStudents { get; set; }   
        
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }

        // Foriegn Keys and Navigation Properties are defiend here
        public int SectionId { get; set; }
        public ProgramBatchSection ProgramBatchSection { get; set; }

        public string FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public int SemesterId { get; set; } 
        public Semester Semester { get; set; }

        public int CourseId { get; set; }   
        public Course Course { get; set; }

    }
}

