using LMS.Features.BatchManagement.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Features.BatchManagement.Services
{
    public interface IBatchService
    {
        Task<IEnumerable<BatchResponseDto>?> GetBatchesByCampusAsync();
        Task<BatchResponseDto?> GetBatchByIdAsync(int batchId);
        Task<BatchResponseDto> CreateBatchAsync(CreateBatchDto createBatchDto);
        Task<BatchResponseDto?> UpdateBatchAsync(int batchId, UpdateBatchDto updateBatchDto);
        Task<bool> DeleteBatchAsync(int batchId);
        Task<bool> SoftDeleteBatchAsync(int batchId);
    }
}
