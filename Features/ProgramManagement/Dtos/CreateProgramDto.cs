using System.ComponentModel.DataAnnotations;

namespace LMS.Features.ProgramManagement.Dtos
{
    public class CreateProgramDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string ProgramName { get; set; }
         
        public string Description { get; set; }

        [Required(ErrorMessage = "Code is required.")]
        public string Code { get; set; }

        public string Duration { get; set; }
        [Required(ErrorMessage = "DegreeType is required.")]
        public string DegreeType { get; set; }
        [Required(ErrorMessage = "CreditRequired is required.")]
        public int CreditRequired { get; set; }

        [Required(ErrorMessage = "DepartmentId is required.")]
        public int DepartmentId { get; set; }
    }
}
