using Dynamic_RBAMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Dynamic_RBAMS.Features.RoleManagement;


namespace Dynamic_RBAMS.Features.PermissionsManagement
{

    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context; // Add this line

        public PermissionController(RoleManager<ApplicationRole> roleManager, ApplicationDbContext context) // Modify constructor
        {
            _roleManager = roleManager;
            _context = context; // Initialize _context
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionDto permissionDto)
        {
            if (string.IsNullOrWhiteSpace(permissionDto.Name))
                return BadRequest("Permission name is required.");

            if (_context.Permissions.Any(p => p.Name == permissionDto.Name))
                return BadRequest($"A permission with the name '{permissionDto.Name}' already exists.");

            var permission = new Permission
            {
                Name = permissionDto.Name,
                Description = permissionDto.Description,
                Status = permissionDto.Status
            };

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();

            var response = new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description,
                Status = permission.Status
            };

            // Return the created permission with the URL for GetPermissionById
            return CreatedAtAction(nameof(GetPermissionById), new { id = permission.Id }, response);
        }

        [HttpGet("{id}")]
        public IActionResult GetPermissionById(int id)
        {
            var permission = _context.Permissions.FirstOrDefault(p => p.Id == id);
            if (permission == null)
                return NotFound();

            var response = new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description,
                Status = permission.Status
            };

            return Ok(response);
        }


        [Authorize]
        [HttpGet("Permissions")]
        public IActionResult GetAllPermissions()
        {
            var permissions = _context.Permissions
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Status = p.Status
                })
                .ToList();

            return Ok(permissions);
        }



        [HttpGet("RolePermissions/{roleId}")]
        public async Task<IActionResult> GetRolePermissions(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound("Role not found.");

            // fetch Permissions for the role from RolePermissionClaims
            var permissions = _context.RolePermissionClaims.Where(rp => rp.RoleId == role.Id)
                                                            .Select(rp => rp.Permission.Name)
                                                            .ToList();
            return Ok(permissions);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            try
            {
                var permission = await _context.Permissions.FindAsync(id);
                if (permission == null)
                    return NotFound("Permission not found.");

                //// Optional: Check for related data
                //var hasRelatedData = await _context.RolePermissionClaims.AnyAsync(rc => rc.PermissionId == id);
                //if (hasRelatedData)
                //    return BadRequest("Cannot delete permission because it is associated with existing roles or claims.");

                _context.Permissions.Remove(permission);
                await _context.SaveChangesAsync();

                var response = new PermissionDto { Id = permission.Id, Name = permission.Name, Description = permission.Description, Status = permission.Status };
                // Return a confirmation message
                return Ok(new { message = "Permission deleted successfully", response });
            }
            catch (Exception)
            {
                // Log exception (if using a logging framework)
                return StatusCode(500, "An error occurred while deleting the permission.");
            }
        }

    }

}
