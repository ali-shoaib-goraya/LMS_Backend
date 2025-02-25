using Dynamic_RBAMS.Features.CampusManagement.Dtos;
using Dynamic_RBAMS.Features.CampusManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dynamic_RBAMS.Features.CampusManagement
{
    //[Authorize]
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
        /// Retrieves a campus by its ID.
        /// </summary>
        [HttpGet("{campusId}")]
        public async Task<IActionResult> GetCampusById(int campusId)
        {
            var result = await _campusService.GetCampusByIdAsync(campusId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves all campuses for a given university.
        /// </summary>
        [HttpGet("mine")]
        public async Task<IActionResult> GetAllCampusesByUniversity()
        {
            var result = await _campusService.GetAllCampusesByUniversityAsync();
            return StatusCode(result.StatusCode, result);
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
        /// Updates an existing campus.
        /// </summary>
        [HttpPut("{campusId}")]
        public async Task<IActionResult> UpdateCampus(int campusId, [FromBody] UpdateCampusDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _campusService.UpdateCampusAsync(campusId, dto);
            return StatusCode(result.StatusCode, result);
        }


        /// <summary>
        /// Permanently deletes a campus.
        /// </summary>
        [HttpDelete("{campusId}")]
        public async Task<IActionResult> HardDeleteCampus(int campusId)
        {
            var result = await _campusService.DeleteCampusAsync(campusId);
            return StatusCode(result.StatusCode, result);
        }


        /// <summary>
        /// Soft deletes a campus.
        /// </summary>
        [HttpPatch("{campusId}/soft-delete")]
        public async Task<IActionResult> SoftDeleteCampus(int campusId)
        {
            var result = await _campusService.SoftDeleteCampusAsync(campusId);
            return StatusCode(result.StatusCode, result);
        }

    }
}

