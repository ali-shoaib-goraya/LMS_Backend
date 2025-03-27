using Microsoft.AspNetCore.Identity;
using LMS.Features.PermissionsManagement;
namespace LMS.Features.Common.Models
{
    public class RolePermissionClaim : IdentityRoleClaim<string>
    {
        public int PermissionId { get; set; }
        public Permission Permission { get; set; } // Navigation property
    }
}
