using LMS.Features.SemesterManagement.Dtos;
using LMS.Features.SemesterManagement.Services;
using LMS.Features.Common.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LMS.Features.SemesterManagement.Controllers
{
    [Route("api/semesters")]
    [ApiController]
    [Authorize] // Requires authentication for all endpoints
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _semesterService;

        public SemesterController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        // 1️⃣ Get All Semesters by Campus
        [HttpGet("mine")]
        [Authorize(Roles = "CampusAdmin")]
        public async Task<IActionResult> GetAllSemestersByCampus()
        {
            int userCampusId = int.Parse(User.FindFirst("CampusId")?.Value ?? "0");
            var semesters = await _semesterService.GetAllSemestersByCampusAsync(userCampusId);

            return Ok(new ApiResponseDto(200, "Semesters retrieved successfully.", semesters));
        }

        // 2️⃣ Get Semester by ID (with CampusAdmin restriction)
        [HttpGet("{semesterId}")]
        [Authorize(Roles = "CampusAdmin")]
        public async Task<IActionResult> GetSemesterById(int semesterId)
        {
            var semester = await _semesterService.GetSemesterByIdAsync(semesterId);
            if (semester == null)
                return NotFound(new ApiResponseDto(404, "Semester not found."));

            return Ok(new ApiResponseDto(200, "Semester retrieved successfully.", semester));
        }

        // 3️⃣ Create a Semester (CampusAdmin Restriction)
        [HttpPost]
        [Authorize(Roles = "CampusAdmin")]
        public async Task<IActionResult> CreateSemester([FromBody] CreateSemesterDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid request data.", ModelState));

            int userCampusId = int.Parse(User.FindFirst("CampusId")?.Value ?? "0");

            var createdSemester = await _semesterService.CreateSemesterAsync(createDto);
            return CreatedAtAction(nameof(GetSemesterById), new { semesterId = createdSemester.SemesterId },
                new ApiResponseDto(201, "Semester created successfully.", createdSemester));
        }

        // 4️⃣ Update Semester
        [HttpPatch("{semesterId}")]
        [Authorize(Roles = "CampusAdmin")]
        public async Task<IActionResult> UpdateSemester(int semesterId, [FromBody] UpdateSemesterDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid request data.", ModelState));

            var semester = await _semesterService.GetSemesterByIdAsync(semesterId);
            if (semester == null)
                return NotFound(new ApiResponseDto(404, "Semester not found."));

            var updatedSemester = await _semesterService.UpdateSemesterAsync(semesterId, updateDto);
            return Ok(new ApiResponseDto(200, "Semester updated successfully.", updatedSemester));
        }

        // 5️⃣ Hard Delete Semester (SuperAdmin Only)
        [HttpDelete("{semesterId}")]
        [Authorize(Roles = "CampusAdmin")]
        public async Task<IActionResult> DeleteSemester(int semesterId)
        {
            var semester = await _semesterService.GetSemesterByIdAsync(semesterId);
            if (semester == null)
                return NotFound(new ApiResponseDto(404, "Semester not found."));

            var success = await _semesterService.DeleteSemesterAsync(semesterId);
            if (!success)
                return BadRequest(new ApiResponseDto(400, "Semester deletion failed."));

            return Ok(new ApiResponseDto(200, "Semester deleted successfully."));
        }

        // 6️⃣ Soft Delete Semester (CampusAdmin Only)
        [HttpPatch("soft-delete/{semesterId}")]
        [Authorize(Roles = "CampusAdmin")]
        public async Task<IActionResult> SoftDeleteSemester(int semesterId)
        {
            var semester = await _semesterService.GetSemesterByIdAsync(semesterId);
            if (semester == null)
                return NotFound(new ApiResponseDto(404, "Semester not found."));

            var success = await _semesterService.SoftDeleteSemesterAsync(semesterId);
            if (!success)
                return BadRequest(new ApiResponseDto(400, "Semester soft delete failed."));

            return Ok(new ApiResponseDto(200, "Semester soft deleted successfully."));
        }
    }
}
