namespace Dynamic_RBAMS.Features.SemesterManagement
{
    public class CreateSemesterDto
    {
        public string SemesterName { get; set; }

        public string Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string Notes { get; set; }

    }
}
