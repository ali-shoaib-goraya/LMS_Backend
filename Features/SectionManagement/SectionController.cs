using LMS.Features.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using LMS.Features.Common.Services;
using LMS.Features.SectionManagement.Dtos;
using LMS.Features.SectionManagement.Services;

namespace LMS.Features.SectionManagement
{
    [Authorize] // Ensures all endpoints require authentication
    [Route("api/sections")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionService _sectionService;
        private readonly IUserContextService _userContextService;

        public SectionController(ISectionService sectionService, IUserContextService userContextService)
        {
            _sectionService = sectionService;
            _userContextService = userContextService;
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyCampusSections()
        {
            var sections = await _sectionService.GetSectionsByCampusAsync();
            if (!sections.Any())
                return Ok(new ApiResponseDto(200, "No sections found yet", sections));

            return Ok(new ApiResponseDto(200, "Sections retrieved successfully", sections));
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpGet("{sectionId}")]
        public async Task<IActionResult> GetSectionById(int sectionId)
        {
            if (sectionId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid section ID"));

            var section = await _sectionService.GetSectionByIdAsync(sectionId);
            if (section == null)
                return NotFound(new ApiResponseDto(404, "Section not found"));

            return Ok(new ApiResponseDto(200, "Section retrieved successfully", section));
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateSection([FromBody] CreateSectionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid input", ModelState));

            try
            {
                var createdSection = await _sectionService.CreateSectionAsync(dto);
                return CreatedAtAction(nameof(GetSectionById), new { sectionId = createdSection.ProgramBatchSectionId },
                    new ApiResponseDto(201, "Section created successfully", createdSection));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDto(404, ex.Message));
            }
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpPatch("{sectionId}")]
        public async Task<IActionResult> UpdateSection(int sectionId, [FromBody] UpdateSectionDto dto)
        {
            if (sectionId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid section ID"));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid input", ModelState));

            var updatedSection = await _sectionService.UpdateSectionAsync(sectionId, dto);
            if (updatedSection == null)
                return NotFound(new ApiResponseDto(404, "Section not found"));

            return Ok(new ApiResponseDto(200, "Section updated successfully", updatedSection));
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpPatch("{sectionId}/soft-delete")]
        public async Task<IActionResult> SoftDeleteSection(int sectionId)
        {
            if (sectionId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid section ID"));

            var success = await _sectionService.SoftDeleteSectionAsync(sectionId);
            if (!success)
                return BadRequest(new ApiResponseDto(400, "Section not found or already soft deleted"));

            return Ok(new ApiResponseDto(200, "Section soft deleted successfully"));
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpDelete("{sectionId}")]
        public async Task<IActionResult> DeleteSection(int sectionId)
        {
            if (sectionId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid section ID"));

            var success = await _sectionService.DeleteSectionAsync(sectionId);
            if (!success)
                return NotFound(new ApiResponseDto(404, "Section not found"));

            return Ok(new ApiResponseDto(200, "Section deleted successfully"));
        }
    }
}
