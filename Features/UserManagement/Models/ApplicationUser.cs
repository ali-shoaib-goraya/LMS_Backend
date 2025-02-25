using Dynamic_RBAMS.Features.AuthenticationManagment;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Features.UserManagement.Models
{
    public enum UserType // Changed to public
    {
        UniversityAdmin,
        CampusAdmin,
        Teacher,
        Hod,
        StaffMember,
        Student,

    }
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }

        public string? EmergencyContact { get; set; }

        public string Type { get; set; }
        // Navigation propoerties and foreign keys are defined here
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}

