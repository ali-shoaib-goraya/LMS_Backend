using LMS.Features.EnrollmentManagement.Services;
using LMS.Features.EnrollmentManagement.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LMS.Features.Common.Dtos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;

namespace LMS.Features.EnrollmentManagement.Controllers
{
    [ApiController]
    [Authorize(Roles = "CampusAdmin, Teacher, Student")]
    [Route("api/enrollments")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        /// <summary>
        /// Enrolls a student in a Course Section
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> EnrollStudent([FromBody] SingleEnrollmentDto dto)
        {
            Log.Information("Received request to enroll student: {@Dto}", dto);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                Log.Warning("Validation failed for student enrollment: {@Errors}", errors);
                return BadRequest(new ApiResponseDto(400, "Invalid data", errors));
            }

            try
            {
                var response = await _enrollmentService.EnrollStudentAsync(dto);
                Log.Information("Student enrolled successfully: {@Response}", response);
                return Ok(new ApiResponseDto(200, "Student enrolled successfully", response));
            }
            catch (InvalidOperationException ex)
            {
                Log.Warning("Business rule violation: {Message}", ex.Message);
                return BadRequest(new ApiResponseDto(400, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during student enrollment");
                return StatusCode(500, new ApiResponseDto(500, "An unexpected error occurred", ex.Message));
            }
        }

        /// <summary>
        /// Bulk enrolls students in a Course Section
        /// </summary>
        [HttpPost("bulk-enroll")]
        public async Task<IActionResult> BulkEnrollStudents([FromBody] BulkEnrollmentDto dto)
        {
            Log.Information("Received request for bulk student enrollment: {@Dto}", dto);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                Log.Warning("Validation failed for bulk enrollment: {@Errors}", errors);
                return BadRequest(new ApiResponseDto(400, "Invalid data", errors));
            }

            try
            {
                var response = await _enrollmentService.BulkEnrollStudentsAsync(dto);
                Log.Information("Bulk students enrolled successfully: {@Response}", response);
                return Ok(new ApiResponseDto(200, "Students enrolled successfully", response));
            }
            catch (InvalidOperationException ex)
            {
                Log.Warning("Business rule violation: {Message}", ex.Message);
                return BadRequest(new ApiResponseDto(400, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during bulk student enrollment");
                return StatusCode(500, new ApiResponseDto(500, "An unexpected error occurred", ex.Message));
            }
        }

        /// <summary>
        /// Removes a student from a Course Section
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveStudentEnrollment(int id)
        {
            Log.Information("Received request to remove student enrollment ID: {Id}", id);

            try
            {
                var result = await _enrollmentService.RemoveEnrollmentAsync(id);
                if (!result)
                {
                    Log.Warning("Enrollment not found for removal, ID: {Id}", id);
                    return NotFound(new ApiResponseDto(404, "Enrollment not found"));
                }

                Log.Information("Student enrollment removed successfully, ID: {Id}", id);
                return Ok(new ApiResponseDto(200, "Student enrollment removed successfully"));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while removing enrollment ID: {Id}", id);
                return StatusCode(500, new ApiResponseDto(500, "An unexpected error occurred", ex.Message));
            }
        }

        /// <summary>
        /// Retrieves the Course Sections a student is enrolled in
        /// </summary>
        [HttpGet("student-course-sections")]
        public async Task<IActionResult> GetStudentCourseSections()
        {
            Log.Information("Received request to retrieve student's enrolled course sections");

            try
            {
                var response = await _enrollmentService.GetStudentCourseSectionsAsync();
                Log.Information("Retrieved student's enrolled course sections successfully: {@Response}", response);
                return Ok(new ApiResponseDto(200, "Student course sections retrieved successfully", response));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while retrieving student course sections");
                return StatusCode(500, new ApiResponseDto(500, "An unexpected error occurred", ex.Message));
            }
        }

        /// <summary>
        /// Retrieves the Course Sections a teacher is assigned to
        /// </summary>
        [HttpGet("teacher-course-sections")]
        public async Task<IActionResult> GetTeacherCourseSections()
        {
            Log.Information("Received request to retrieve teacher's assigned course sections");

            try
            {
                var response = await _enrollmentService.GetTeacherCourseSectionsAsync();
                Log.Information("Retrieved teacher's assigned course sections successfully: {@Response}", response);
                return Ok(new ApiResponseDto(200, "Teacher course sections retrieved successfully", response));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while retrieving teacher course sections");
                return StatusCode(500, new ApiResponseDto(500, "An unexpected error occurred", ex.Message));
            }
        }
    }
}
