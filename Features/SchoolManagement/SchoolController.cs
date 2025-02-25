using Dynamic_RBAMS.Features.SchoolManagement.Dtos;
using Dynamic_RBAMS.Features.SchoolManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Dynamic_RBAMS.Features.SchoolManagement
{
    [Authorize]
    [ApiController] 
    [Route("api/schools")]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;
        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpGet("mine")]
        public async Task<IActionResult> GetAllSchoolsByCampusId()
        {
            var result = await _schoolService.GetAllSchoolsByCampusAsync();
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchool(int id)
        {
            var result = await _schoolService.GetSchoolByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }


        [Authorize(Roles = "CampusAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateSchool([FromBody] CreateSchoolDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _schoolService.CreateSchoolAsync(dto);
            return StatusCode(result.StatusCode, result);
        }


        [Authorize(Roles = "CampusAdmin")]
        [HttpPut("{schoolId}")]
        public async Task<IActionResult> UpdateSchool(int schoolId, [FromBody] UpdateSchoolDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _schoolService.UpdateSchoolAsync(schoolId, dto);
            return StatusCode(result.StatusCode, result);
        }


        [Authorize(Roles = "CampusAdmin")]
        [HttpDelete("{schoolId}")]
        public async Task<IActionResult> DeleteSchool(int schoolId)
        {
            var result = await _schoolService.DeleteSchoolAsync(schoolId);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpPatch("{schoolId}/softdelete")]
        public async Task<IActionResult> SoftDelete(int schoolId)
        {
            var result = await _schoolService.SoftDeleteSchoolAsync(schoolId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
