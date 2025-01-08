//using Dynamic_RBAMS.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;


//namespace Dynamic_RBAMS.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class UserController : ControllerBase
//    {
//        private readonly UserManager<ApplicationUser> _userManager;

//        public UserController(UserManager<ApplicationUser> userManager)
//        {
//            _userManager = userManager;
//        }

//        [HttpPost("CreateUser")]
//        public async Task<IActionResult> CreateUser([FromBody] UserRegistrationDto model)
//        {
//            var user = new ApplicationUser
//            {
//                UserName = model.Username,
//                Email = model.Email
//            };

//            var result = await _userManager.CreateAsync(user, model.Password);
//            if (result.Succeeded)
//                return Ok("User created successfully.");

//            return BadRequest(result.Errors);
//        }

//        [HttpPost("AssignRoleToUser")]
//        public async Task<IActionResult> AssignRoleToUser(string userId, string roleName)
//        {
//            var user = await _userManager.FindByIdAsync(userId);
//            if (user == null)
//                return NotFound("User not found.");

//            var result = await _userManager.AddToRoleAsync(user, roleName);
//            if (result.Succeeded)
//                return Ok("Role assigned to user successfully.");

//            return BadRequest(result.Errors);
//        }

//        [HttpGet("GetUserRoles/{userId}")]
//        public async Task<IActionResult> GetUserRoles(string userId)
//        {
//            var user = await _userManager.FindByIdAsync(userId);
//            if (user == null)
//                return NotFound("User not found.");

//            var roles = await _userManager.GetRolesAsync(user);
//            return Ok(roles);
//        }
//    }

//}
