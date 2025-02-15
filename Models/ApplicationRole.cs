using Microsoft.AspNetCore.Identity;

namespace Dynamic_RBAMS.Models
{
    public class ApplicationRole : IdentityRole
    {
        // Additional properties for roles can be added here if required.
        public bool Status { get; set; } =true;

        public string Description { get; set; }
    }
}

