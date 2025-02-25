// UniversityController.cs
using Dynamic_RBAMS.Features.UniveristyManagement.Dtos;
using Dynamic_RBAMS.Features.UniveristyManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Dynamic_RBAMS.Features.UniveristyManagement
{
    //[Authorize]
    [ApiController]
    [Route("api/universities")]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityService _universityService;

        public UniversityController(IUniversityService universityService)
        {
            _universityService = universityService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUniversity([FromBody] CreateUniversityDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Handles DTO validation errors

            var result = await _universityService.CreateUniversityAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{universityId}")]
        public async Task<IActionResult> GetUniversity(int universityId)
        {
            var result = await _universityService.GetUniversityByIdAsync(universityId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUniversities()
        {
            var result = await _universityService.GetAllUniversitiesAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{universityId}")]
        public async Task<IActionResult> DeleteUniversity(int universityId)
        {
            var result = await _universityService.DeleteUniversityAsync(universityId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("soft-delete/{universityId}")]
        public async Task<IActionResult> SoftDeleteUniversity(int universityId)
        {
            var result = await _universityService.SoftDeleteUniversityAsync(universityId);
            return StatusCode(result.StatusCode, result);
        }

    }
}
