// UniversityController.cs
using Microsoft.AspNetCore.Mvc;
using Dynamic_RBAMS.Services;
using Dynamic_RBAMS.DTOs;
using Dynamic_RBAMS.Interfaces;


namespace Dynamic_RBAMS.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUniversity(int id)
        {
            var result = await _universityService.GetUniversityByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUniversities()
        {
            var result = await _universityService.GetAllUniversitiesAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUniversity(int id)
        {
            var result = await _universityService.DeleteUniversityAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDeleteUniversity(int id)
        {
            var result = await _universityService.SoftDeleteUniversityAsync(id);
            return StatusCode(result.StatusCode, result);
        }

    }
}
