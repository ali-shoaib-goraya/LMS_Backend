using LMS.Features.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using LMS.Features.Common.Services;
using LMS.Features.BatchManagement.Dtos;
using LMS.Features.BatchManagement.Services;

namespace LMS.Features.BatchManagement
{
    [Authorize] // Ensures all endpoints require authentication
    [Route("api/batches")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly IBatchService _batchService;
        private readonly IUserContextService _userContextService;

        public BatchController(IBatchService batchService, IUserContextService userContextService)
        {
            _batchService = batchService;
            _userContextService = userContextService;
        }


        [Authorize(Roles = "CampusAdmin")]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyCampusBatches()
        {
            var batches = await _batchService.GetBatchesByCampusAsync();
            if (!batches.Any())
                return Ok(new ApiResponseDto(200, "No batches found yet", batches));

            return Ok(new ApiResponseDto(200, "Batches retrieved successfully", batches));
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpGet("{batchId}")]
        public async Task<IActionResult> GetBatchById(int batchId)
        {
            if (batchId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid batch ID"));

            var batch = await _batchService.GetBatchByIdAsync(batchId);
            if (batch == null)
                return NotFound(new ApiResponseDto(404, "Batch not found"));

            return Ok(new ApiResponseDto(200, "Batch retrieved successfully", batch));
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateBatch([FromBody] CreateBatchDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid input", ModelState));

            try
            {
                var createdBatch = await _batchService.CreateBatchAsync(dto);
                return CreatedAtAction(nameof(GetBatchById), new { batchId = createdBatch.ProgramBatchId },
                    new ApiResponseDto(201, "Batch created successfully", createdBatch));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDto(404, ex.Message));
            }
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpPut("{batchId}")]
        public async Task<IActionResult> UpdateBatch(int batchId, [FromBody] UpdateBatchDto dto)
        {
            if (batchId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid batch ID"));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto(400, "Invalid input", ModelState));

            var updatedBatch = await _batchService.UpdateBatchAsync(batchId, dto);
            if (updatedBatch == null)
                return NotFound(new ApiResponseDto(404, "Batch not found"));

            return Ok(new ApiResponseDto(200, "Batch updated successfully", updatedBatch));
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpPatch("{batchId}/soft-delete")]
        public async Task<IActionResult> SoftDeleteBatch(int batchId)
        {
            if (batchId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid batch ID"));

            var success = await _batchService.SoftDeleteBatchAsync(batchId);
            if (!success)
                return BadRequest(new ApiResponseDto(400, "Batch not found or already soft deleted"));

            return Ok(new ApiResponseDto(200, "Batch soft deleted successfully"));
        }

        [Authorize(Roles = "CampusAdmin")]
        [HttpDelete("{batchId}")]
        public async Task<IActionResult> DeleteBatch(int batchId)
        {
            if (batchId <= 0)
                return BadRequest(new ApiResponseDto(400, "Invalid batch ID"));

            var success = await _batchService.DeleteBatchAsync(batchId);
            if (!success)
                return NotFound(new ApiResponseDto(404, "Batch not found"));

            return Ok(new ApiResponseDto(200, "Batch deleted successfully"));
        }
    }
}
