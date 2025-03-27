using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LMS.Features.UserManagement.Dtos
{ 
    public class RegisterCampusAdminDto : RegisterBaseDto
    {
        [Required]
        public int CampusId { get; set; }

        public int UniversityId { get; set; }

        [JsonIgnore] // 👈 Prevents modification from API requests
        public new string Type { get; set; } = "CampusAdmin";
        public RegisterCampusAdminDto()
        {
            Type = "CampusAdmin";  // Ensures default value
        }
    }
}
