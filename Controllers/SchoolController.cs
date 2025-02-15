using Dynamic_RBAMS.Interfaces;
using Dynamic_RBAMS.DTOs;
using Dynamic_RBAMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Dynamic_RBAMS.Controllers
{
    [ApiController]
    [Route("api/Schools")]
    public class SchoolController: ControllerBase
    {
        private readonly ISchoolService _schoolService;
        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchool([FromBody] CreateSchoolDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _schoolService.CreateSchoolAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchool(int id)
        {
            var result = await _schoolService.GetSchoolByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("campuses/{CampusId}")] // More RESTful 
        public async Task<IActionResult> GetAllSchoolsByCampusId(int CampusId)
        {
            var result = await _schoolService.GetAllSchoolsByCampusIdAsync(CampusId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]    
        public async Task<IActionResult> DeleteSchool(int id)
        {
            var result = await _schoolService.DeleteSchoolAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchool(int id, [FromBody] UpdateSchoolDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _schoolService.UpdateSchoolAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/softdelete")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var result = await _schoolService.SoftDeleteSchoolAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
    }
