﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LMS.Features.UserManagement.Dtos
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

