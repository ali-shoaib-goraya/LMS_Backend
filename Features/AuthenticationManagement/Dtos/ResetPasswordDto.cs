﻿using System.ComponentModel.DataAnnotations;

namespace LMS.Features.AuthenticationManagement.Dtos
{
    public class ResetPasswordDto
    { 
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}
