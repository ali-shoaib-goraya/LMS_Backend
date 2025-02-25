using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Dynamic_RBAMS.Features.Common.Services
{
    public interface IUserContextService
    {
        int? GetCampusId(); // Returns nullable int
        int? GetUserId();
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
        public int? GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
                return userId;
            return null;
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
