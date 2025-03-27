using Microsoft.AspNetCore.Identity;

namespace LMS.Features.RoleManagement
{
    public class ApplicationRole : IdentityRole
    {
        // Additional properties for roles can be added here if required.
        public bool Status { get; set; } = true;

        public string Description { get; set; }
    }
}

