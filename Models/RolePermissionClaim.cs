using Microsoft.AspNetCore.Identity;

namespace Dynamic_RBAMS.Models
{
    public class RolePermissionClaim : IdentityRoleClaim<string>
    {
        public int PermissionId { get; set; }
        public Permission Permission { get; set; } // Navigation property
    }
}
