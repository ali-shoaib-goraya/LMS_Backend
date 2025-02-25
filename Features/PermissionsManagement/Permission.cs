using Dynamic_RBAMS.Features.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Dynamic_RBAMS.Features.PermissionsManagement
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Status { get; set; } = true;

        public ICollection<RolePermissionClaim> RoleClaims { get; set; }
    }

}
