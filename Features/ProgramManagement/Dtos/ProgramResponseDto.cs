using System.ComponentModel.DataAnnotations;

namespace LMS.Features.ProgramManagement.Dtos
{
    public class ProgramResponseDto
    {   
        public int ProgramId { get; set; } 
        public string ProgramName { get; set; }
        public string Description { get; set; } 
        public string Code { get; set; }
        public string Duration { get; set; }
        public string DegreeType { get; set; }
        public int CreditRequired { get; set; }
        public int DepartmentId { get; set; }   
    }
}
