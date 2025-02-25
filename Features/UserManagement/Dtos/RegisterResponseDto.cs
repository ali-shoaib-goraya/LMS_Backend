using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Features.UserManagement.Dtos
{
    public class RegisterResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public string Type { get; set; }
    }
}
