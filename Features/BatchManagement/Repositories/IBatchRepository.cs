using LMS.Features.BatchManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Features.BatchManagement.Repositories
{
    public interface IBatchRepository
    {
        Task<IEnumerable<ProgramBatch>> GetBatchesByCampusAsync(int campusId);
        Task<ProgramBatch?> GetBatchByIdAsync(int batchId);
        Task<ProgramBatch> CreateBatchAsync(ProgramBatch batch);
        Task<ProgramBatch?> UpdateBatchAsync(ProgramBatch batch);
        Task<bool> DeleteBatchAsync(int batchId);
        Task<bool> SoftDeleteBatchAsync(int batchId);
        Task<bool> IsBatchNameExistsAsync(int campusId, string batchName, int? excludingBatchId = null);
    }
}
