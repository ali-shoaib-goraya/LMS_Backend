using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dynamic_RBAMS.DTOs.AuthDtos
{
    public class RegisterUniversityAdminDto : RegisterBaseDto
    {
        [Required]
        public int UniversityId { get; set; }
         
        [JsonIgnore] 
        public new string Type { get; set; } = "UniversityAdmin";

        public RegisterUniversityAdminDto()
        {
            Type = "UniversityAdmin";  // Ensures default even if user doesn't send "Type"
        }
    } 
}

