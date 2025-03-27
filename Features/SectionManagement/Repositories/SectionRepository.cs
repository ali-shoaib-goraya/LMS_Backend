using LMS.Data;
using LMS.Features.SectionManagement;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Features.SectionManagement.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly ApplicationDbContext _context;

        public SectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProgramBatchSection>> GetSectionsByCampusAsync(int campusId)
        {
            return await _context.ProgramBatchSections
                .Where(s =>
                    s.ProgramBatch.Program.Department.School.CampusId == campusId &&
                    !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<ProgramBatchSection?> GetSectionByIdAsync(int sectionId)
        {
            return await _context.ProgramBatchSections
                    .Include(s => s.ProgramBatch)
                    .ThenInclude(pb => pb.Program)
                    .ThenInclude(p => p.Department)
                    .ThenInclude(d => d.School)
                    .FirstOrDefaultAsync(s => s.ProgramBatchSectionId == sectionId && !s.IsDeleted);
        }

        public async Task<bool> IsSectionNameExistsAsync(int batchId, string sectionName, int? excludingSectionId = null)
        {
            if (string.IsNullOrEmpty(sectionName))
                return false;

            return await _context.ProgramBatchSections
                .Where(s => s.ProgramBatchId == batchId &&
                            s.SectionName.ToLower() == sectionName.ToLower() &&
                            !s.IsDeleted &&
                            (excludingSectionId == null || s.ProgramBatchSectionId != excludingSectionId))
                .AnyAsync();
        }

        public async Task<ProgramBatchSection> CreateSectionAsync(ProgramBatchSection section)
        {
            // Validate Batch existence
            var batch = await _context.ProgramBatches
                .FirstOrDefaultAsync(b => b.ProgramBatchId == section.ProgramBatchId);

            if (batch == null)
                throw new Exception("Invalid BatchId. Batch not found.");
            section.ProgramBatch = batch;
            // Ensure unique section name within batch
            if (await IsSectionNameExistsAsync(section.ProgramBatchId, section.SectionName))
                throw new Exception("A section with this name already exists in the batch.");

            _context.ProgramBatchSections.Add(section);
            await _context.SaveChangesAsync();
            return section;
        }

        public async Task<ProgramBatchSection?> UpdateSectionAsync(ProgramBatchSection section)
        {
            var existingSection = await _context.ProgramBatchSections
                .FirstOrDefaultAsync(s => s.ProgramBatchSectionId == section.ProgramBatchSectionId);

            if (existingSection == null)
                return null;

            var batchExists = await _context.ProgramBatches.AnyAsync(b => b.ProgramBatchId == section.ProgramBatchId);
            if (!batchExists)
                throw new InvalidOperationException("Invalid BatchId provided.");

            if (await IsSectionNameExistsAsync(section.ProgramBatchId, section.SectionName, section.ProgramBatchSectionId))
                throw new InvalidOperationException("A section with this name already exists in the batch.");

            // ✅ Update required fields only
            existingSection.SectionName = section.SectionName;
            existingSection.Capacity = section.Capacity;
            existingSection.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingSection;
        }


        public async Task<bool> DeleteSectionAsync(int sectionId)
        {
            var section = await _context.ProgramBatchSections.FindAsync(sectionId);
            if (section == null)
                return false;

            _context.ProgramBatchSections.Remove(section);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteSectionAsync(int sectionId)
        {
            var section = await _context.ProgramBatchSections.FindAsync(sectionId);
            if (section == null || section.IsDeleted)
                return false;

            section.IsDeleted = true;
            section.UpdatedAt = DateTime.UtcNow;

            _context.ProgramBatchSections.Update(section);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
