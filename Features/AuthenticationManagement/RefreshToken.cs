using Dynamic_RBAMS.Features.UserManagement.Models;

namespace Dynamic_RBAMS.Features.AuthenticationManagment
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = null!;  // Ensures Token is never null

        public string UserId { get; set; } = null!;  // Foreign Key
        public ApplicationUser User { get; set; } = null!;

        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
