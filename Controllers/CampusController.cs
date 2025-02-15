using Dynamic_RBAMS.DTOs;
using Dynamic_RBAMS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dynamic_RBAMS.Controllers
{
    [ApiController]
    [Route("api/campuses")] // More RESTful
    public class CampusController : ControllerBase
    {
        private readonly ICampusService _campusService;

        public CampusController(ICampusService campusService)
        {
            _campusService = campusService;
        }

        /// <summary>
        /// Creates a new campus.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCampus([FromBody] CreateCampusDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _campusService.CreateCampusAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves a campus by its ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampusById(int id)
        {
            var result = await _campusService.GetCampusByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves all campuses for a given university.
        /// </summary>
        [HttpGet("university/{universityId}")]
        public async Task<IActionResult> GetAllCampusesByUniversity(int universityId)
        {
            var result = await _campusService.GetAllCampusesByUniversityIdAsync(universityId);
            // log the result here
            Console.WriteLine(result);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Updates an existing campus.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampus(int id, [FromBody] UpdateCampusDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _campusService.UpdateCampusAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Soft deletes a campus.
        /// </summary>
        [HttpPatch("{id}/soft-delete")]
        public async Task<IActionResult> SoftDeleteCampus(int id)
        {
            var result = await _campusService.SoftDeleteCampusAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Permanently deletes a campus.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> HardDeleteCampus(int id)
        {
            var result = await _campusService.DeleteCampusAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
