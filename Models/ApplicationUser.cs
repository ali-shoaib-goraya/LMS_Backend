using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Models
{
    public enum UserType // Changed to public
    {
        Faculty,
        Student
    }
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public bool Status { get; set; } = true; // Changed from "Active" to true

        public string Type { get; set; } = UserType.Student.ToString(); // Changed to use UserType enum directly

        public string? Role { get; set; }
    }
} 

