using Dynamic_RBAMS.Models;

namespace Dynamic_RBAMS.DTOs
{
    public class CreateProgramDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public string Duration { get; set; }

        public string DegreeType { get; set; }

        public int CreditRequired { get; set; }

        public int DepartmentId { get; set; }
    }
}
