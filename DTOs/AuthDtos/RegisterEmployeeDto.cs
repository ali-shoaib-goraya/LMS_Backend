using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.DTOs.AuthDtos
{
    public class RegisterEmployeeDto : RegisterBaseDto
    {
        [Required]
        public int DepartmentID { get; set; }

        public string Designation { get; set; }

        public string ProfilePicture { get; set; }

        public string EmploymentType { get; set; }

        public string Qualification { get; set; }

        public string EmploymentStatus { get; set; }
    }

}
