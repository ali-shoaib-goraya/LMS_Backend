using LMS.Features.CourseSectionManagement.Services;
using LMS.Features.CourseSectionManagement.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LMS.Features.Common.Dtos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;

namespace LMS.Features.CourseSectionManagement.Controllers
{
    [ApiController]
    [Authorize(Roles = "CampusAdmin")]
    [Route("api/coursesections")]
    public class CourseSectionController : ControllerBase
    {
        private readonly ICourseSectionService _courseSectionService;

        public CourseSectionController(ICourseSectionService courseSectionService)
        {
            _courseSectionService = courseSectionService;
        }

        /// <summary>
        /// Creates a new Course Section
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCourseSection([FromBody] CreateCourseSectionDto dto)
        {
            Log.Information("Received request to create Course Section: {@Dto}", dto);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                Log.Warning("Validation failed for Course Section creation: {@Errors}", errors);
                return BadRequest(new ApiResponseDto(400, "Invalid data", errors));
            }

            try
            {
                var response = await _courseSectionService.CreateCourseSectionAsync(dto);
                Log.Information("Course Section created successfully: {@Response}", response);
                return Ok(new ApiResponseDto(200, "Course Section created successfully", response));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating Course Section");
                return StatusCode(500, new ApiResponseDto(500, "An unexpected error occurred", ex.Message));
            }
        }

        /// <summary>
        /// Bulk Creates Course Sections
        /// </summary>
        [HttpPost("bulk-create")]
        public async Task<IActionResult> BulkCreateCourseSections([FromBody] BulkCreateCourseSectionDto dto)
        {
            Log.Information("Received request to bulk create Course Sections: {@Dto}", dto);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                Log.Warning("Validation failed for bulk Course Section creation: {@Errors}", errors);
                return BadRequest(new ApiResponseDto(400, "Invalid data", errors));
            }

            try
            {
                var response = await _courseSectionService.BulkCreateCourseSectionsAsync(dto);
                Log.Information("Bulk Course Sections created successfully: {@Response}", response);
                return Ok(new ApiResponseDto(200, "Course Sections created successfully", response));
            }
            catch (InvalidOperationException ex)
            {
                Log.Warning("Business rule violation in bulk Course Section creation: {Message}", ex.Message);
                return BadRequest(new ApiResponseDto(400, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred during bulk Course Section creation");
                return StatusCode(500, new ApiResponseDto(500, "An unexpected error occurred", ex.Message));
            }
        }

        /// <summary>
        /// Gets all Course Sections
        /// </summary>
        [HttpGet("mine")]
        public async Task<IActionResult> GetAllCourseSectionsByCampus()
        {
            Log.Information("Received request to get all Course Sections for Campus Admin");

            try
            {
                var response = await _courseSectionService.GetAllCourseSectionsByCampusAsync();
                Log.Information("Retrieved Course Sections successfully: {@Response}", response);
                return Ok(new ApiResponseDto(200, "Course Sections retrieved successfully", response));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving Course Sections");
                return StatusCode(500, new ApiResponseDto(500, "An unexpected error occurred", ex.Message));
            }
        }

        /// <summary>
        /// Gets a Course Section by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseSectionById(int id)
        {
            Log.Information("Received request to get Course Section by ID: {Id}", id);

            try
            {
                var response = await _courseSectionService.GetCourseSectionByIdAsync(id);
                Log.Information("Retrieved Course Section successfully: {@Response}", response);
                return Ok(new ApiResponseDto(200, "Course Section retrieved successfully", response));
            }
            catch (KeyNotFoundException)
            {
                Log.Warning("Course Section not found with ID: {Id}", id);
                return NotFound(new ApiResponseDto(404, "Course Section not found"));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving Course Section with ID: {Id}", id);
                return StatusCode(500, new ApiResponseDto(500, "An unexpected error occurred", ex.Message));
            }
        }

        /// <summary>
        /// Updates a Course Section
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourseSection(int id, [FromBody] UpdateCourseSectionDto dto)
        {
            Log.Information("Received request to update Course Section ID: {Id} with data: {@Dto}", id, dto);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                Log.Warning("Validation failed for Course Section update: {@Errors}", errors);
                return BadRequest(new ApiResponseDto(400, "Invalid data", errors));
            }

            try
            {
                var response = await _courseSectionService.UpdateCourseSectionAsync(id, dto);
                Log.Information("Course Section updated successfully: {@Response}", response);
                return Ok(new ApiResponseDto(200, "Course Section updated successfully", response));
            }
            catch (KeyNotFoundException ex)
            {
                Log.Warning("Course Section not found for update, ID: {Id}", id);
                return NotFound(new ApiResponseDto(404, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Warning("Unauthorized access while updating Course Section ID: {Id}", id);
                return StatusCode(403, new ApiResponseDto(403, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while updating Course Section ID: {Id}", id);
                return StatusCode(500, new ApiResponseDto(500, "An unexpected error occurred", ex.Message));
            }
        }

        /// <summary>
        /// Deletes a Course Section
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseSection(int id)
        {
            Log.Information("Received request to delete Course Section ID: {Id}", id);

            try
            {
                var result = await _courseSectionService.DeleteCourseSectionAsync(id);
                if (!result)
                {
                    Log.Warning("Course Section not found for deletion, ID: {Id}", id);
                    return NotFound(new ApiResponseDto(404, "Course Section not found"));
                }

                Log.Information("Course Section deleted successfully, ID: {Id}", id);
                return Ok(new ApiResponseDto(200, "Course Section deleted successfully"));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while deleting Course Section ID: {Id}", id);
                return StatusCode(500, new ApiResponseDto(500, "An unexpected error occurred", ex.Message));
            }
        }

        /// <summary>
        /// Soft deletes a Course Section (marks it as inactive)
        /// </summary>
        [HttpPatch("{id}/soft-delete")]
        public async Task<IActionResult> SoftDeleteCourseSection(int id)
        {
            Log.Information("Received request to soft delete Course Section ID: {Id}", id);
            try
            {
                var result = await _courseSectionService.SoftDeleteCourseSectionAsync(id);
                if (!result)
                {
                    Log.Warning("Course Section not found for soft deletion, ID: {Id}", id);
                    return NotFound(new ApiResponseDto(404, "Course Section not found"));
                }

                Log.Information("Course Section soft deleted successfully, ID: {Id}", id);
                return Ok(new ApiResponseDto(200, "Course Section soft deleted successfully"));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while soft deleting Course Section ID: {Id}", id);
                return StatusCode(500, new ApiResponseDto(500, "An unexpected error occurred", ex.Message));
            }
        }
    }
}
