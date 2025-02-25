using Dynamic_RBAMS.Features.PermissionsManagement;

namespace Dynamic_RBAMS.Data
{
    public static class PredefinedPermissions
    {
        public static readonly Permission ViewUsers = new() { Name = "ViewUsers", Description = "View Users" };

        public static readonly Permission ManageUsers = new() { Name = "ManageUsers", Description = "Manage Users" };

        public static readonly Permission ViewRoles = new() { Name = "ViewRoles", Description = "View Roles" };

        public static readonly Permission ManageRoles = new() { Name = "ManageRoles", Description = "Manage Roles" };

        public static readonly Permission AssignPermissions = new() { Name = "AssignPermissions", Description = "Assign Permissions" };

        public static List<Permission> AllPermissions => new()
            {
                ViewUsers,
                ManageUsers,
                ViewRoles,
                ManageRoles,
                AssignPermissions
            };
    }

}
