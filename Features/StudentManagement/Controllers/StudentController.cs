using LMS.Features.Common.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LMS.Features.StudentManagement.Services;
using LMS.Features.StudentManagement.Dtos;

namespace LMS.Features.StudentManagement.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }


        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudent([FromBody] StudentEnrollmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDto(400, "Invalid data.", ModelState));
            }

            var response = await _studentService.EnrollStudentAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentById(string studentId)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(studentId);
                return Ok(new ApiResponseDto(200, "Student retrieved successfully.", student));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDto(400, ex.Message));
            }
        }

        [HttpGet("mine")]
        public async Task<IActionResult> GetAllStudentsByCampus()
        {
            var students = await _studentService.GetAllStudentsByCampusAsync();
            return Ok(new ApiResponseDto(200, "Students retrieved successfully.", students));
        }

        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(string studentId)
        {
            var response = await _studentService.DeleteStudentAsync(studentId);
            return StatusCode(response.StatusCode, response);
        }


        [HttpPost("bulk-enroll/{batchSectionId}")]
        [Authorize(Roles = "CampusAdmin")]
        public async Task<IActionResult> BulkEnrollStudents(int batchSectionId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new ApiResponseDto(400, "CSV file is required."));

            var result = await _studentService.BulkEnrollStudentsAsync(file, batchSectionId);
            return StatusCode(result.StatusCode, result);
        }


    }
}
