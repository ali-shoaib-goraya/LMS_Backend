using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using LMS.Features.Common.Models;
using LMS.Features.RoleManagement;
using LMS.Features.UserManagement;
using LMS.Features.Common.Dtos;
using LMS.Features.UserManagement.Services;
using LMS.Features.UserManagement.Dtos;
using LMS.Features.UserManagement.Models;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using LMS.Features.CampusManagement.Repositories;
using LMS.Features.UniveristyManagement.Repositories;
using LMS.Features.Common.Services;

public class UserService : IUserService
{   private readonly ICampusRepository _campusRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IUniversityRepository _universityRepository;
    private readonly IUserContextService _userContextService;
    private readonly ApplicationDbContext _context;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IUniversityRepository universityRepository, ICampusRepository campusRepository, ApplicationDbContext context, IUserContextService userContextService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _universityRepository = universityRepository;
        _campusRepository = campusRepository;
        _context = context;
        _userContextService = userContextService;
    }

    private async Task<RegisterResponseDto> MapToRegisterResponseDtoAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user); // Fetch roles assigned to user
        return new RegisterResponseDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            Type = user.Type ?? string.Empty,
            Role = roles.FirstOrDefault() ?? string.Empty  // Get the first role (assuming single-role assignment)
        };
    }
    public async Task<ApiResponseDto> RegisterTeacherAsync(RegisterEmployeeDto dto)
    {
        // ✅ Validate Type
        if (!Enum.TryParse(typeof(UserType), dto.Type, out _))
        {
            return new ApiResponseDto(400, "Invalid user type",
                new List<string> { $"The provided type '{dto.Type}' is invalid. Allowed values: {string.Join(", ", Enum.GetNames(typeof(UserType)))}" });
        }

        // ✅ Validate Role
        if (!await _roleManager.RoleExistsAsync(dto.Role))
        {
            return new ApiResponseDto(400, "Invalid role", new List<string> { "The specified role does not exist." });
        }

        // ✅ Validate that all department IDs exist in the database
        var existingDepartmentIds = await _context.Departments
            .Where(d => dto.DepartmentIds.Contains(d.DepartmentId))
            .Select(d => d.DepartmentId)
            .ToListAsync();

        var invalidDepartmentIds = dto.DepartmentIds.Except(existingDepartmentIds).ToList();
        if (invalidDepartmentIds.Any())
        {
            return new ApiResponseDto(400, "Invalid departments",
                new List<string> { $"The following department IDs are invalid: {string.Join(", ", invalidDepartmentIds)}" });
        }

        // ✅ Retrieve the Campus ID from UserContext
        int? campusId = _userContextService.GetCampusId();
        if (campusId == null)
        {
            return new ApiResponseDto(403, "Unauthorized", new List<string> { "Campus information is missing. Ensure you have the correct permissions." });
        }

        var user = new Faculty
        {
            Email = dto.Email,
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address,
            Gender = dto.Gender,
            EmergencyContact = dto.EmergencyContact,
            Designation = dto.Designation,
            ProfilePicture = dto.ProfilePicture,
            EmploymentType = dto.EmploymentType,
            Qualification = dto.Qualification,
            EmploymentStatus = dto.EmploymentStatus,
            Type = dto.Type  // ✅ Ensure type is validated before assignment
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return new ApiResponseDto(400, "Registration failed", result.Errors.Select(e => e.Description).ToList());
        }

        // ✅ Assign role to user
        await _userManager.AddToRoleAsync(user, dto.Role);

        // ✅ Assign the faculty to the specified departments
        var departmentFaculties = dto.DepartmentIds.Select(deptId => new DepartmentFaculty
        {
            FacultyId = user.Id,
            DepartmentId = deptId
        }).ToList();

        _context.DepartmentFaculties.AddRange(departmentFaculties);

        // ✅ Assign the faculty to the campus using FacultyCampuses bridge table
        var facultyCampus = new FacultyCampus
        {
            FacultyId = user.Id,
            CampusId = campusId.Value  // Ensure it's not null
        };

        _context.FacultiesCampuses.Add(facultyCampus);

        // ✅ Save all changes in one transaction
        await _context.SaveChangesAsync();

        // ✅ Retrieve the created user and map to RegisterResponseDto
        var createdUser = await _userManager.FindByEmailAsync(dto.Email);
        var registerResponse = await MapToRegisterResponseDtoAsync(createdUser);

        return new ApiResponseDto(200, "Teacher registered successfully", registerResponse);
    }


    public async Task<ApiResponseDto> RegisterUniversityAdminAsync(RegisterUniversityAdminDto dto)
    {
        if (!await _roleManager.RoleExistsAsync(dto.Role))
        {
            return new ApiResponseDto(400, "Invalid role", new List<string> { "The specified role does not exist." });
        }

        if (dto.Type != "UniversityAdmin")
        {
            return new ApiResponseDto(400, "Invalid type");
        }

        var user = new Faculty
        {
            Email = dto.Email,
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address,
            Gender = dto.Gender, 
            EmergencyContact = dto.EmergencyContact,
            UniversityId = dto.UniversityId,   // University Admin should have UniversityId
            Type = dto.Type                // e.g., "UniversityAdmin"
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return new ApiResponseDto(400, "Registration failed", result.Errors.Select(e => e.Description).ToList());
        }

        await _userManager.AddToRoleAsync(user, dto.Role);

        // ✅ Fetch the created user and their assigned role
        var createdUser = await _userManager.FindByEmailAsync(dto.Email);
        var registerResponse = await MapToRegisterResponseDtoAsync(createdUser);

        return new ApiResponseDto(200, "University Admin registered successfully", registerResponse);
    }


    public async Task<ApiResponseDto> RegisterCampusAdminAsync(RegisterCampusAdminDto dto)
    {
        if (!await _roleManager.RoleExistsAsync(dto.Role))
        {
            return new ApiResponseDto(400, "Invalid role", new List<string> { "The specified role does not exist." });
        }

        if (dto.Type != "CampusAdmin")
        {
            return new ApiResponseDto(400, "Invalid type");
        }

        // Check if the campus exists
        var campus = await _campusRepository.GetByIdAsync(dto.CampusId);
        if (campus == null)
        {
            return new ApiResponseDto(400, "Invalid CampusID", new List<string> { "The specified campus does not exist." });
        }

        var user = new Faculty
        {
            Email = dto.Email,
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address,
            Gender = dto.Gender,
            EmergencyContact = dto.EmergencyContact,
            Type = dto.Type,                  // e.g., "CampusAdmin"
            UniversityId = dto.UniversityId,           // Optionally, if not provided.
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return new ApiResponseDto(400, "Registration failed", result.Errors.Select(e => e.Description).ToList());
        }

        await _userManager.AddToRoleAsync(user, dto.Role);

        // Update the bridge table for FacultyCampus
        var facultyCampus = new FacultyCampus
        {
            FacultyId = user.Id,
            CampusId = dto.CampusId
        };
        await _universityRepository.AddFacultyCampusAsync(facultyCampus);

        var createdUser = await _userManager.FindByEmailAsync(dto.Email);
        var registerResponse = await MapToRegisterResponseDtoAsync(createdUser);

        return new ApiResponseDto(200, "Campus Admin registered successfully", registerResponse);
    }

    public async Task<List<UsersResponseDto>> GetAllFacultyForCampusAsync(int campusId)
    {
        var users = await _context.DepartmentFaculties
            .Where(df => _context.Departments
                .Where(d => _context.Schools 
                    .Where(s => s.CampusId == campusId)
                    .SelectMany(s => s.Departments)
                    .Select(d => d.DepartmentId)
                    .Contains(d.DepartmentId))
                .Select(d => d.DepartmentId)
                .Contains(df.DepartmentId))
            .Select(df => df.Faculty) // Get Faculty from bridge table
            .Where(u => u.Type != "CampusAdmin" && u.Type != "UniversityAdmin") // ✅ Type filtering
            .Distinct()
            .Select(u => new UsersResponseDto
            {
                Id = u.Id,
                Email = u.Email ?? "No Email", // Null-safe
                FirstName = u.FirstName,
                LastName = u.LastName,

                // Fetch Roles by joining UserRoles and Roles
                Roles = _context.UserRoles
                    .Where(ur => ur.UserId == u.Id)
                    .Join(_context.Roles,
                          ur => ur.RoleId,
                          r => r.Id,
                          (ur, r) => r.Name ?? "Unknown Role") // Null-safe
                    .ToList(),

                // Fetch Departments via Faculty-Department bridge table
                Departments = _context.DepartmentFaculties
                    .Where(df => df.FacultyId == u.Id)
                    .Select(df => df.Department.DepartmentName)
                    .ToList(),

                Type = u.Type,
                Designation = u.Designation,
                EmploymentType = u.EmploymentType,
                EmploymentStatus = u.EmploymentStatus,
                Qualification = u.Qualification,

                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            })
            .ToListAsync();

        return users;
    }


    public async Task<ApiResponseDto> DeleteUserAsync(string userId)
    {
        // ✅ Fetch the user from the database
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ApiResponseDto(404, "User not found");
        }

        // ✅ Check if the user is a Faculty and find their associated campuses
        var faculty = await _context.Faculties
            .Include(f => f.FacultyCampuses)
            .Include(f => f.DepartmentFaculties)
            .FirstOrDefaultAsync(f => f.Id == userId);

        if (faculty == null)
        {
            return new ApiResponseDto(400, "User is not a faculty member");
        }

        // ✅ Retrieve the Campus ID from UserContext
        int? campusId = _userContextService.GetCampusId();
        if (campusId == null) {
            return new ApiResponseDto(403, "Unauthorized", new List<string> { "Campus information is missing. Ensure you have the correct permissions." });
        }

        // ✅ Ensure the user belongs to the requesting CampusAdmin's campus
        var facultyCampusAssociation = faculty.FacultyCampuses.FirstOrDefault(fc => fc.CampusId == campusId.Value);
        if (facultyCampusAssociation == null)
        {
            return new ApiResponseDto(403, "Unauthorized", new List<string> { "You can only delete users associated with your campus." });
        }

        // ✅ Check if the faculty is associated with multiple campuses
        if (faculty.FacultyCampuses.Count > 1)
        {
            // The user is assigned to multiple campuses → Remove association with the current campus
            _context.FacultiesCampuses.Remove(facultyCampusAssociation);
            await _context.SaveChangesAsync();

            return new ApiResponseDto(200, "User removed from this campus but not deleted.");
        }

        // ✅ Remove related Faculty-Department associations (if they exist)
        if (faculty.DepartmentFaculties.Any())
        {
            _context.DepartmentFaculties.RemoveRange(faculty.DepartmentFaculties);
        }

        // ✅ Remove user from UserRoles table before deleting
        var userRoles = await _context.UserRoles.Where(ur => ur.UserId == userId).ToListAsync();
        if (userRoles.Any())
        {
            _context.UserRoles.RemoveRange(userRoles);
        }

        // ✅ Delete the user
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return new ApiResponseDto(400, "User deletion failed", result.Errors.Select(e => e.Description).ToList());
        }

        // ✅ Save changes to DB
        await _context.SaveChangesAsync();

        return new ApiResponseDto(200, "User deleted successfully");
    }

    public async Task<UsersResponseDto?> GetUserByIdAsync(string  userId)
    {
        var user = await _context.Faculties
            .Include(f => f.FacultyCampuses)
            .Include(f => f.DepartmentFaculties)
            .FirstOrDefaultAsync(f => f.Id == userId);
        if (user == null)
        {
            return null;
        }
        var userResponse = new UsersResponseDto
        {
            Id = user.Id,
            Email = user.Email ?? "No Email", // Null-safe
            FirstName = user.FirstName,
            LastName = user.LastName,
            // Fetch Roles by joining UserRoles and Roles
            Roles = _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_context.Roles,
                      ur => ur.RoleId,
                      r => r.Id,
                      (ur, r) => r.Name ?? "Unknown Role") // Null-safe
                .ToList(),
            // Fetch Departments via Faculty-Department bridge table
            Departments = _context.DepartmentFaculties
                .Where(df => df.FacultyId == user.Id)
                .Select(df => df.Department.DepartmentName)
                .ToList(),
            Type = user.Type,
            Designation = user.Designation,
            EmploymentType = user.EmploymentType,
            EmploymentStatus = user.EmploymentStatus,
            Qualification = user.Qualification,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
        return userResponse;
    }
}
