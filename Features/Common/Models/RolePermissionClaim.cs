using Microsoft.AspNetCore.Identity;
using Dynamic_RBAMS.Features.PermissionsManagement;
namespace Dynamic_RBAMS.Features.Common.Models
{
    public class RolePermissionClaim : IdentityRoleClaim<string>
    {
        public int PermissionId { get; set; }
        public Permission Permission { get; set; } // Navigation property
    }
}
