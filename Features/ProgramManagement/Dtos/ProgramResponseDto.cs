using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Features.ProgramManagement.Dtos
{
    public class ProgramResponseDto
    {   
        public int ProgramId { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Duration { get; set; }
        public string DegreeType { get; set; }
        public int CreditRequired { get; set; }
        public int DepartmentId { get; set; }   
        public string DepartmentName { get; set; }
    }
}
