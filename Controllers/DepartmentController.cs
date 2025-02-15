using Dynamic_RBAMS.DTOs;
using Dynamic_RBAMS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Dynamic_RBAMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Get all departments for a specific campus
        /// </summary>
        [HttpGet("campus/{campusId}")]
        public async Task<IActionResult> GetDepartmentsByCampus(int campusId)
        {
            if (campusId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid Campus Id"));

            var departments = await _departmentService.GetDepartmentsByCampusAsync(campusId);

            if (departments == null)
                return NotFound(new ApiResponseDto(404, "Campus not found"));

            if (!departments.Any())
                return Ok(new ApiResponseDto(200, "No departments found yet", departments));

            return Ok(new ApiResponseDto(200, "Departments retrieved successfully", departments));
        }


        /// <summary>
        /// Get a single department by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid department ID"));

            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
                return NotFound(new ApiResponseDto(404, "Department not found"));

            return Ok(new ApiResponseDto(200, "Department retrieved successfully", department));
        }

        /// <summary>
        /// Create a new department
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid input", ModelState));

            try
            {
                var createdDepartment = await _departmentService.CreateDepartmentAsync(dto);
                return CreatedAtAction(nameof(GetDepartmentById), new { id = createdDepartment.DepartmentId },
                    new ApiResponseDto(201, "Department created successfully", createdDepartment));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDto(404, ex.Message));
            }
        }

        /// <summary>
        /// Update an existing department
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentDto dto)
        {
            if (id <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid department ID"));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid input", ModelState));

            var updatedDepartment = await _departmentService.UpdateDepartmentAsync(id, dto);
            if (updatedDepartment == null)
                return NotFound(new ApiResponseDto(404, "Department not found"));

            return Ok(new ApiResponseDto(200, "Department updated successfully", updatedDepartment));
        }

        /// <summary>
        /// Soft delete a department (marks it as deleted)
        /// </summary>
        [HttpPut("soft-delete/{id}")]
        public async Task<IActionResult> SoftDeleteDepartment(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid department ID"));

            var success = await _departmentService.SoftDeleteDepartmentAsync(id);
            if (!success)
                return BadRequest(new ApiResponseDto(400, "Department not found or already soft deleted"));

            return Ok(new ApiResponseDto(200, "Department soft deleted successfully"));
        }

        /// <summary>
        /// Hard delete a department (permanently removes it)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid department ID"));

            var success = await _departmentService.DeleteDepartmentAsync(id);
            if (!success)
                return NotFound(new ApiResponseDto(404, "Department not found"));

            return Ok(new ApiResponseDto(200, "Department deleted successfully"));
        }
    }
}
