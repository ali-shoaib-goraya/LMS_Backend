namespace LMS.Features.StudentManagement.Dtos
{
    public class StudentResponseDto
    {
        public string Id { get; set; }
        public string EnrollmentNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GuardianName { get; set; }
        public string GuardianContact { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int SectionId { get; set; }
        public string Email { get; set; }
    }

}
