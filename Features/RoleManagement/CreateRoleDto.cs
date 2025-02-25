using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Features.RoleManagement
{
    public class CreateRoleDto
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        public string[] Permissions { get; set; }


    }
}
