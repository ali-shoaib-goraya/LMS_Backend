namespace Features.PermissionsManagement.Dtos
{
    public class PermissionResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public List<string> Roles { get; set; } // E.g., roles associated with the permission
    }
}
