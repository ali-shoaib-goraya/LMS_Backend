using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LMS.Data;
using LMS.Features.Common.Models;
namespace LMS.Features.RoleManagement
{

     
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RoleController(RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Role name is required.");

            // Create a new role
            var role = new ApplicationRole
            {
                Name = request.Name,
                Description = request.Description,
                Status = request.Status
            };

            var result = await _roleManager.CreateAsync(role);

            // Check if the role creation was successful
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Assign permissions to the role
            if (request.Permissions != null && request.Permissions.Length != 0)
            {
                // Retrieve permissions from the database
                var permissions = _context.Permissions
                    .Where(p => request.Permissions.Contains(p.Name))
                    .ToList();

                // Check if any requested permissions were not found
                var missingPermissions = request.Permissions.Except(permissions.Select(p => p.Name)).ToList();
                if (missingPermissions.Any())
                    return BadRequest($"The following permissions were not found: {string.Join(", ", missingPermissions)}");

                // Add the permissions to the role
                foreach (var permission in permissions)
                {
                    // Avoid duplicate claims
                    if (!_context.RolePermissionClaims.Any(rp => rp.RoleId == role.Id && rp.PermissionId == permission.Id))
                    {
                        _context.RolePermissionClaims.Add(new RolePermissionClaim
                        {
                            RoleId = role.Id,
                            PermissionId = permission.Id
                        });
                    }
                }

                await _context.SaveChangesAsync();
            }

            return Ok("Role created successfully.");
        }


        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles
                                    .Select(role => new
                                    {
                                        role.Id,
                                        role.Name,
                                        role.Description,
                                        role.Status,
                                        Permissions = _context.RolePermissionClaims
                                                            .Where(rp => rp.RoleId == role.Id)
                                                            .Select(rp => rp.Permission.Name)
                                                            .ToList() // Convert IQueryable<string> to List<string>
                                    })
                                    .ToList();

            return Ok(roles);
        }

        [HttpPut("EditRole/{id}")]
        public async Task<IActionResult> EditRole(string id, [FromBody] CreateRoleDto request)
        {
            // Fetch the existing role
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound("Role not found.");

            // Update role details
            role.Name = request.Name;
            role.Description = request.Description;
            role.Status = request.Status;

            // Update the role in the database
            var updateResult = await _roleManager.UpdateAsync(role);
            if (!updateResult.Succeeded)
                return BadRequest(updateResult.Errors);

            // Remove existing role-permissions
            var existingPermissions = _context.RolePermissionClaims.Where(rp => rp.RoleId == id).ToList();
            _context.RolePermissionClaims.RemoveRange(existingPermissions);

            if (request.Permissions != null)
            {
                // Add new permissions
                foreach (var permissionName in request.Permissions)
                {
                    var permission = _context.Permissions.FirstOrDefault(p => p.Name == permissionName);
                    if (permission == null)
                        return BadRequest($"Permission '{permissionName}' not found.");

                    _context.RolePermissionClaims.Add(new RolePermissionClaim
                    {
                        RoleId = id,
                        PermissionId = permission.Id
                    });
                }

                await _context.SaveChangesAsync();
            }

            return Ok("Role updated successfully.");
        }



        [HttpDelete("DeleteRole/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound("Role not found.");

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return Ok("Role deleted successfully.");

            return BadRequest(result.Errors);
        }
    }

}
