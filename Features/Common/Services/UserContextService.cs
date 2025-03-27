using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LMS.Features.Common.Services
{
    public interface IUserContextService
    {
        int? GetCampusId(); // Returns nullable int
        string? GetUserId();
        int? GetUniversityId();
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves the CampusId from the logged-in user's claims.
        /// </summary>
        public int? GetCampusId()
        {
            var campusIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("CampusId")?.Value;

            if (int.TryParse(campusIdClaim, out int campusId))
                return campusId;

            return null; // Explicitly return null if not found
        }

        /// <summary>
        /// Retrieves the UserId from the logged-in user's claims.
        /// </summary>
        public string? GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? user?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        } 

        public int? GetUniversityId()
        {
            var universityIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UniversityId")?.Value;
            if (int.TryParse(universityIdClaim, out int universityId))
                return universityId;
            return null;
        }
    }
}
