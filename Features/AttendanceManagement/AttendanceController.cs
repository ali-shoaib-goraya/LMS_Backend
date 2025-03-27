using global::LMS.Features.AttendanceManagement.Dtos;
using global::LMS.Features.AttendanceManagement.Services;
using global::LMS.Features.Common.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LMS.Features.AttendanceManagement
{
   
[Route("api/attendance")]
    [ApiController]
    [Authorize(Roles = "CampusAdmin, Teacher")] // Fixed the syntax and added Roles property
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
         
        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        // 1. Mark Attendance
        [HttpPost("mark")]
        public async Task<IActionResult> MarkAttendance([FromBody] MarkAttendanceDto markAttendanceDto)
        {
            try
            {
                int classSessionId = await _attendanceService.MarkAttendanceAsync(markAttendanceDto);
                return Ok(new ApiResponseDto(200, "Attendance marked successfully.", classSessionId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto(500, "An error occurred while marking attendance.", ex.Message));
            }
        }

        // 2. Get Attendance for a Session
        [HttpGet("{classSessionId}")]
        public async Task<IActionResult> GetAttendance(int classSessionId)
        {
            try
            {
                var attendance = await _attendanceService.GetAttendanceBySessionAsync(classSessionId);
                return Ok(new ApiResponseDto(200, "Attendance retrieved successfully.", attendance));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDto(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto(500, "An error occurred while fetching attendance.", ex.Message));
            }
        }

        // 3. Update Attendance
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAttendance([FromBody] UpdateAttendanceDto updateAttendanceDto)
        {
            try
            {
                bool updated = await _attendanceService.UpdateAttendanceAsync(updateAttendanceDto);
                return Ok(new ApiResponseDto(200, "Attendance updated successfully.", updated));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDto(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto(500, "An error occurred while updating attendance.", ex.Message));
            }
        }

        // 4. Delete Attendance Records for a Session
        [HttpDelete("{classSessionId}")]
        public async Task<IActionResult> DeleteAttendance(int classSessionId)
        {
            try
            {
                bool deleted = await _attendanceService.DeleteAttendanceBySessionAsync(classSessionId);
                return Ok(new ApiResponseDto(200, "Attendance records deleted successfully.", deleted));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDto(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto(500, "An error occurred while deleting attendance records.", ex.Message));
            }
        }
    }
    }

