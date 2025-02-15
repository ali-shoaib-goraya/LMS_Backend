using Dynamic_RBAMS.DTOs.AuthDtos;
using Dynamic_RBAMS.DTOs;
using Dynamic_RBAMS.Interfaces;
using Dynamic_RBAMS.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class UserService : IUserService
{   private readonly ICampusRepository _campusRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IUniversityRepository _universityRepository; 

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IUniversityRepository universityRepository, ICampusRepository campusRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _universityRepository = universityRepository;
        _campusRepository = campusRepository;

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
        // Check if role is valid.
        if (!await _roleManager.RoleExistsAsync(dto.Role))
        {
            return new ApiResponseDto(400, "Invalid role", new List<string> { "The specified role does not exist." });
        }
        //var deptId = dto.DepartmentID;

        var user = new Faculty
        {
            Email = dto.Email,
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address,
            Gender = dto.Gender,
            EmergencyContact = dto.EmergencyContact,
            DepartmentID = dto.DepartmentID, // Required for teachers.
            Designation = dto.Designation,
            ProfilePicture = dto.ProfilePicture,
            EmploymentType = dto.EmploymentType,
            Qualification = dto.Qualification,
            EmploymentStatus = dto.EmploymentStatus,
            Type = dto.Type  // e.g., "Teacher"
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return new ApiResponseDto(400, "Registration failed", result.Errors.Select(e => e.Description).ToList());
        }

        // Assign role to user.
        await _userManager.AddToRoleAsync(user, dto.Role);

        // Retrieve the created user and map to RegisterResponseDto.
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
}
