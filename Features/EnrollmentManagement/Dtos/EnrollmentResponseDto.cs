namespace LMS.Features.EnrollmentManagement.Dtos
{
    public class EnrollmentResponseDto
    {
        public int EnrollmentId { get; set; }
        public string StudentId { get; set; }
        public int CourseSectionId { get; set; }
        public string? Status { get; set; }
        public bool? IsApproved { get; set; }
    }
}
