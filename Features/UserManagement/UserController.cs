using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Dynamic_RBAMS.Features.Common.Dtos;
using Dynamic_RBAMS.Features.UserManagement.Services;
using Dynamic_RBAMS.Features.UserManagement.Dtos;
using Dynamic_RBAMS.Features.Common.Services;

namespace Dynamic_RBAMS.Features.UserManagement
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserContextService _userContextService;

        public UserController(IUserService userService, IUserContextService userContextService)
        {
            _userService = userService;
            _userContextService = userContextService;
        }

        /// Register a new Teacher
        /// </summary>
        [HttpPost("register-employee")]
        [Authorize(Roles = "UniversityAdmin, CampusAdmin")] // Restrict access
        public async Task<IActionResult> RegisterTeacher([FromBody] RegisterEmployeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDto(400, "Invalid request data", ModelState.Values));
            }

            var response = await _userService.RegisterTeacherAsync(dto);
            return StatusCode(response.StatusCode, response);
        }


        /// Register a new University Admin
        /// </summary>
        [HttpPost("register-university-admin")]
        //[Authorize(Roles = "SuperAdmin")] // Restrict to SuperAdmin
        public async Task<IActionResult> RegisterUniversityAdmin([FromBody] RegisterUniversityAdminDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDto(400, "Invalid request data", ModelState.Values));
            }

            var response = await _userService.RegisterUniversityAdminAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        /// Register a new Campus Admin
        /// </summary>
        [HttpPost("register-campus-admin")]
        //[Authorize(Roles = "UniversityAdmin")] // University Admins can add Campus Admins
        public async Task<IActionResult> RegisterCampusAdmin([FromBody] RegisterCampusAdminDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDto(400, "Invalid request data", ModelState.Values));
            }

            var response = await _userService.RegisterCampusAdminAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("mine")]
        //[Authorize(Roles = "CampusAdmin")]  //Restrict to Campus Admins
        public async Task<IActionResult> GetAllUsersForCampus()
        {
            try
            {
                // Fetch CampusId from UserContextService
                var campusId = _userContextService.GetCampusId();
                if (campusId == null)
                {
                    return Unauthorized(new ApiResponseDto(401, "Unauthorized access: No campus assigned"));
                }

                // Fetch users for the campus
                var users = await _userService.GetAllUsersForCampusAsync(campusId.Value);

                return Ok(new ApiResponseDto(200, "Users fetched successfully", users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto(500, "An error occurred", ex.Message));
            }
        }


        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var response = await _userService.DeleteUserAsync(userId);
            return StatusCode(response.StatusCode, response);
        }


    }
}
