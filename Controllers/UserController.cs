using Dynamic_RBAMS.DTOs.AuthDtos;
using Dynamic_RBAMS.DTOs;
using Dynamic_RBAMS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dynamic_RBAMS.Controllers
{
    [Route("api/users")]
        [ApiController]
        public class UserController : ControllerBase
        {
            private readonly IUserService _userService;

            public UserController(IUserService userService)
            {
                _userService = userService;
            }

            /// Register a new Teacher
            /// </summary>
            [HttpPost("register-teacher")]
            //[Authorize(Roles = "UniversityAdmin, CampusAdmin")] // Restrict access
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
        }
    }
