using System.ComponentModel.DataAnnotations;

namespace LMS.Features.UserManagement.Dtos
{
    public class RegisterEmployeeDto : RegisterBaseDto
    {
        [Required(ErrorMessage = "At least one department must be selected.")]
        [MinLength(1, ErrorMessage = "At least one department must be selected.")]
        public List<int> DepartmentIds { get; set; }  // ✅ Allow assigning multiple departments

        public string Designation { get; set; }

        public string ProfilePicture { get; set; }

        public string EmploymentType { get; set; }

        public string Qualification { get; set; }

        public string EmploymentStatus { get; set; }
    }

}
