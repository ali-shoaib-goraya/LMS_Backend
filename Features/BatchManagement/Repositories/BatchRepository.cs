using LMS.Data;
using LMS.Features.BatchManagement;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Features.BatchManagement.Repositories
{
    public class BatchRepository : IBatchRepository
    {
        private readonly ApplicationDbContext _context;

        public BatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProgramBatch>> GetBatchesByCampusAsync(int campusId)
        {
            return await _context.ProgramBatches
                .Include(b => b.Program)
                .ThenInclude(p => p.Department)
                .ThenInclude(d => d.School)
                .Where(b => b.Program.Department.School.CampusId == campusId && !b.IsDeleted)
                .ToListAsync();
        }

        public async Task<ProgramBatch?> GetBatchByIdAsync(int batchId)
        {
            return await _context.ProgramBatches
                .Include(b => b.Program)
                .FirstOrDefaultAsync(b => b.ProgramBatchId == batchId && !b.IsDeleted);
        }

        public async Task<bool> IsBatchNameExistsAsync(int campusId, string batchName, int? excludingBatchId = null)
        {
            if (string.IsNullOrEmpty(batchName))
                return false;
            return await _context.ProgramBatches
                .Include(b => b.Program)
                .ThenInclude(p => p.Department)
                .ThenInclude(d => d.School)
                .Where(b => b.Program != null &&
                            b.Program.Department != null &&
                            b.Program.Department.School != null &&
                            b.Program.Department.School.CampusId == campusId &&
                               b.BatchName.ToLower() == batchName.ToLower() &&
                               !b.IsDeleted &&
                               (excludingBatchId == null || b.ProgramBatchId != excludingBatchId))
                .AnyAsync();
        }

        public async Task<bool> IsProgramNameExistsAsync(int campusId, string programName, int? excludingProgramId = null)
        {
            // Guard against null programName parameter
            if (string.IsNullOrEmpty(programName))
                return false;

            return await _context.Programs
                .Include(p => p.Department)
                .ThenInclude(d => d.School)
                .Where(p => p.Department != null &&
                            p.Department.School != null &&
                            p.Department.School.CampusId == campusId &&
                            p.ProgramName.ToLower() == programName.ToLower() &&
                            !p.IsDeleted &&
                            (excludingProgramId == null || p.ProgramId != excludingProgramId))
                .AnyAsync();
        }


        public async Task<ProgramBatch> CreateBatchAsync(ProgramBatch batch)
        {
            // Load related entities to avoid null reference issues
            var program = await _context.Programs
                .Include(p => p.Department)
                .ThenInclude(d => d.School)
                .FirstOrDefaultAsync(p => p.ProgramId == batch.ProgramId);

            if (program == null)
                throw new Exception("Invalid ProgramId. Program not found.");

            batch.Program = program;

            var campusId = batch.Program.Department.School.CampusId;

            if (await IsBatchNameExistsAsync(campusId, batch.BatchName))
                throw new Exception("A batch with this name already exists in the campus.");

            _context.ProgramBatches.Add(batch);
            await _context.SaveChangesAsync();
            return batch;
        }

        public async Task<ProgramBatch?> UpdateBatchAsync(ProgramBatch batch)
        {
            var existingBatch = await _context.ProgramBatches.AsNoTracking().FirstOrDefaultAsync(b => b.ProgramBatchId == batch.ProgramBatchId);
            if (existingBatch == null)
                return null;

            // Ensure the ProgramId is valid
            var programExists = await _context.Programs.AnyAsync(p => p.ProgramId == batch.ProgramId);
            if (!programExists)
                throw new Exception("Invalid ProgramId provided");

            if (await IsBatchNameExistsAsync(batch.Program.Department.School.CampusId, batch.BatchName, batch.ProgramBatchId))
                throw new Exception("A batch with this name already exists in the campus.");

            _context.ProgramBatches.Update(batch);
            await _context.SaveChangesAsync();
            return batch;
        }

        public async Task<bool> DeleteBatchAsync(int batchId)
        {
            var batch = await _context.ProgramBatches.FindAsync(batchId);
            if (batch == null)
                return false;

            _context.ProgramBatches.Remove(batch);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteBatchAsync(int batchId)
        {
            var batch = await _context.ProgramBatches.FindAsync(batchId);
            if (batch == null || batch.IsDeleted)
                return false;

            batch.IsDeleted = true;
            batch.UpdatedAt = DateTime.UtcNow;

            _context.ProgramBatches.Update(batch);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
