using LMS.Features.Common.Dtos;
using LMS.Features.CourseManagement.Dtos;
using LMS.Features.CourseManagement.Services;
using LMS.Features.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Features.CourseManagement
{
    [Authorize] // Ensures all endpoints require authentication
    [Route("api/courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IUserContextService _userContextService; // Service to get logged-in user info

        public CourseController(ICourseService courseService, IUserContextService userContextService)
        {
            _courseService = courseService;
            _userContextService = userContextService;
        }

        [Authorize(Roles = "CampusAdmin,HeadOfDepartment")]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyCampusCourses()
        {
            var userCampusId = _userContextService.GetCampusId();
            if (userCampusId == null)
                return BadRequest(new ApiResponseDto(403, "Forbidden"));

            var courses = await _courseService.GetCoursesByCampusAsync(userCampusId.Value);

            if (!courses.Any())
                return Ok(new ApiResponseDto(200, "No courses found yet", courses));

            return Ok(new ApiResponseDto(200, "Courses retrieved successfully", courses));
        }

        [Authorize(Roles = "CampusAdmin,HeadOfDepartment")]
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseById(int courseId)
        {
            if (courseId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid course ID"));

            var course = await _courseService.GetCourseByIdAsync(courseId);
            if (course == null)
                return NotFound(new ApiResponseDto(404, "Course not found"));

            return Ok(new ApiResponseDto(200, "Course retrieved successfully", course));
        }

        [Authorize(Roles = "CampusAdmin,HeadOfDepartment")]
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] AddCourseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid input", ModelState));

            try
            {
                var createdCourse = await _courseService.CreateCourseAsync(dto);
                return CreatedAtAction(nameof(GetCourseById), new { courseId = createdCourse.CourseId },
                    new ApiResponseDto(201, "Course created successfully", createdCourse));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDto(404, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponseDto(400, ex.Message));
            }
        }

        [Authorize(Roles = "CampusAdmin,HeadOfDepartment")]
        [HttpPatch("{courseId}")]
        public async Task<IActionResult> UpdateCourse(int courseId, [FromBody] UpdateCourseDto dto)
        {
            if (courseId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid course ID"));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid input", ModelState));

            var updatedCourse = await _courseService.UpdateCourseAsync(courseId, dto);
            if (updatedCourse == null)
                return NotFound(new ApiResponseDto(404, "Course not found"));

            return Ok(new ApiResponseDto(200, "Course updated successfully", updatedCourse));
        }


        [Authorize(Roles = "CampusAdmin,HeadOfDepartment")]
        [HttpPatch("{courseId}/soft-delete")]
        public async Task<IActionResult> SoftDeleteCourse(int courseId)
        {
            if (courseId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid course ID"));

            var success = await _courseService.SoftDeleteCourseAsync(courseId);
            if (!success)
                return BadRequest(new ApiResponseDto(400, "Course not found or already soft deleted"));

            return Ok(new ApiResponseDto(200, "Course soft deleted successfully"));
        }

        [Authorize(Roles = "CampusAdmin,HeadOfDepartment")]
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            if (courseId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid course ID"));

            var success = await _courseService.HardDeleteCourseAsync(courseId);
            if (!success)
                return NotFound(new ApiResponseDto(404, "Course not found"));

            return Ok(new ApiResponseDto(200, "Course deleted successfully"));
        }
    }
}
