using Dynamic_RBAMS.Features.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dynamic_RBAMS.Features.DepartmentManagement.Dtos;
using Dynamic_RBAMS.Features.DepartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Dynamic_RBAMS.Features.Common.Services;

namespace Dynamic_RBAMS.Features.DepartmentManagement
{
    [Authorize]  //Ensures all endpoints require authentication
    [Route("api/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IUserContextService _userContextService; // Service to get logged-in user info

        public DepartmentController(IDepartmentService departmentService, IUserContextService userContextService)
        {
            _departmentService = departmentService;
            _userContextService = userContextService;
        }

   
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();

            if (!departments.Any())
                return Ok(new ApiResponseDto(200, "No departments found yet", departments));

            return Ok(new ApiResponseDto(200, "Departments retrieved successfully", departments));
        }



        [Authorize(Roles = "CampusAdmin")]
        [HttpGet("mine")] 
        public async Task<IActionResult> GetMyCampusDepartments()
        {
            var userCampusId = _userContextService.GetCampusId(); // Get campusId from logged-in user

            if (userCampusId == null)
                return Forbid(); // Ensures the user has an assigned campus

            var departments = await _departmentService.GetDepartmentsByCampusAsync(userCampusId.Value);

            if (!departments.Any())
                return Ok(new ApiResponseDto(200, "No departments found yet", departments));

            return Ok(new ApiResponseDto(200, "Departments retrieved successfully", departments));
        }



        [Authorize(Roles = "CampusAdmin")]
        [HttpGet("{departmentId}")]
        public async Task<IActionResult> GetDepartmentById(int departmentId)
        {
            if (departmentId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid department ID"));

            var department = await _departmentService.GetDepartmentByIdAsync(departmentId);
            if (department == null)
                return NotFound(new ApiResponseDto(404, "Department not found"));

            return Ok(new ApiResponseDto(200, "Department retrieved successfully", department));
        }



        [Authorize(Roles = "CampusAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid input", ModelState));

            try
            {
                var createdDepartment = await _departmentService.CreateDepartmentAsync(dto);
                return CreatedAtAction(nameof(GetDepartmentById), new { departmentId = createdDepartment.DepartmentId },
                    new ApiResponseDto(201, "Department created successfully", createdDepartment));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDto(404, ex.Message));
            }
        }




        [Authorize(Roles = "CampusAdmin")]
        [HttpPut("{departmentId}")]
        public async Task<IActionResult> UpdateDepartment(int departmentId, [FromBody] UpdateDepartmentDto dto)
        {
            if (departmentId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid department ID"));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid input", ModelState));

            var updatedDepartment = await _departmentService.UpdateDepartmentAsync(departmentId, dto);
            if (updatedDepartment == null)
                return NotFound(new ApiResponseDto(404, "Department not found"));

            return Ok(new ApiResponseDto(200, "Department updated successfully", updatedDepartment));
        }

        


        [Authorize(Roles = "CampusAdmin")]
        [HttpPatch("{departmentId}/soft-delete")]
        public async Task<IActionResult> SoftDeleteDepartment(int departmentId)
        {
            if (departmentId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid department ID"));

            var success = await _departmentService.SoftDeleteDepartmentAsync(departmentId);
            if (!success)
                return BadRequest(new ApiResponseDto(400, "Department not found or already soft deleted"));

            return Ok(new ApiResponseDto(200, "Department soft deleted successfully"));
        }




        [Authorize(Roles = "CampusAdmin")]
        [HttpDelete("{departmentId}")]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            if (departmentId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid department ID"));

            var success = await _departmentService.DeleteDepartmentAsync(departmentId);
            if (!success)
                return NotFound(new ApiResponseDto(404, "Department not found"));

            return Ok(new ApiResponseDto(200, "Department deleted successfully")); 
        }
    }
}

