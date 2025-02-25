namespace Dynamic_RBAMS.Features.SemesterManagement
{
    public class Semester
    {
        public int SemesterId { get; set; }

        public string SemesterName { get; set; }

        public string Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
