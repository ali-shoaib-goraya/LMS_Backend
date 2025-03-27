using LMS.Features.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using LMS.Features.ProgramManagement.Dtos;
using LMS.Features.ProgramManagement.Services;
using Microsoft.AspNetCore.Authorization;
using LMS.Features.Common.Services;

namespace LMS.Features.ProgramManagement
{
    [Authorize]  // Ensures all endpoints require authentication
    [Route("api/programs")]
    [ApiController]
    public class ProgramController : ControllerBase
    {
        private readonly IProgramService _programService;
        private readonly IUserContextService _userContextService; // Service to get logged-in user info

        public ProgramController(IProgramService programService, IUserContextService userContextService)
        {
            _programService = programService;
            _userContextService = userContextService;
        }


        [Authorize(Roles = "CampusAdmin")]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyCampusPrograms()
        {
            var userCampusId = _userContextService.GetCampusId();

            if (userCampusId == null)
                return BadRequest(new ApiResponseDto(403, "Forbidden"));

            var programs = await _programService.GetProgramsByCampusAsync(userCampusId.Value);

            if (!programs.Any())
                return Ok(new ApiResponseDto(200, "No programs found yet", programs));

            return Ok(new ApiResponseDto(200, "Programs retrieved successfully", programs));
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpGet("{programId}")]
        public async Task<IActionResult> GetProgramById(int programId)
        {
            if (programId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid program ID"));

            var program = await _programService.GetProgramByIdAsync(programId);
            if (program == null)
                return NotFound(new ApiResponseDto(404, "Program not found"));

            return Ok(new ApiResponseDto(200, "Program retrieved successfully", program));
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateProgram([FromBody] CreateProgramDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid input", ModelState));

            try
            {
                var createdProgram = await _programService.CreateProgramAsync(dto);
                return CreatedAtAction(nameof(GetProgramById), new { programId = createdProgram.ProgramId },
                    new ApiResponseDto(201, "Program created successfully", createdProgram));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDto(404, ex.Message));
            }
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpPut("{programId}")]
        public async Task<IActionResult> UpdateProgram(int programId, [FromBody] UpdateProgramDto dto)
        {
            if (programId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid program ID"));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid input", ModelState));

            var updatedProgram = await _programService.UpdateProgramAsync(programId, dto);
            if (updatedProgram == null)
                return NotFound(new ApiResponseDto(404, "Program not found"));

            return Ok(new ApiResponseDto(200, "Program updated successfully", updatedProgram));
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpPatch("{programId}/soft-delete")]
        public async Task<IActionResult> SoftDeleteProgram(int programId)
        {
            if (programId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid program ID"));

            var success = await _programService.SoftDeleteProgramAsync(programId);
            if (!success)
                return BadRequest(new ApiResponseDto(400, "Program not found or already soft deleted"));

            return Ok(new ApiResponseDto(200, "Program soft deleted successfully"));
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpDelete("{programId}")]
        public async Task<IActionResult> DeleteProgram(int programId)
        {
            if (programId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid program ID"));

            var success = await _programService.DeleteProgramAsync(programId);
            if (!success)
                return NotFound(new ApiResponseDto(404, "Program not found"));

            return Ok(new ApiResponseDto(200, "Program deleted successfully"));
        }
    }
}
