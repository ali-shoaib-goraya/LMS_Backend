using AutoMapper;
using LMS.Features.BatchManagement.Repositories;
using LMS.Features.ProgramManagement.Repositories;
using LMS.Features.Common.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using LMS.Features.BatchManagement;
using LMS.Features.ProgramManagement;
using LMS.Features.BatchManagement.Dtos;

namespace LMS.Features.BatchManagement.Services
{
    public class BatchService : IBatchService
    {
        private readonly IMapper _mapper;
        private readonly IBatchRepository _batchRepository;
        private readonly IProgramRepository _programRepository;
        private readonly ICampusEntityAuthorizationService _campusAuthorizationService;
        private readonly IUserContextService _userContextService;

        public BatchService(
            IMapper mapper,
            IBatchRepository batchRepository,
            IProgramRepository programRepository,
            ICampusEntityAuthorizationService campusAuthorizationService,
            IUserContextService userContextService)
        {
            _mapper = mapper;
            _batchRepository = batchRepository;
            _programRepository = programRepository;
            _campusAuthorizationService = campusAuthorizationService;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<BatchResponseDto>?> GetBatchesByCampusAsync()
        {
            // 🔒 Ensure CampusAdmin is fetching only their own campus data
            var userCampusId = _userContextService.GetCampusId();
            if ( userCampusId == null)
                throw new UnauthorizedAccessException("You do not have permission to access batches from this campus.");

            var batches = await _batchRepository.GetBatchesByCampusAsync(userCampusId.Value);
            return _mapper.Map<IEnumerable<BatchResponseDto>>(batches);
        }

        public async Task<BatchResponseDto?> GetBatchByIdAsync(int batchId)
        {
            var batch = await _batchRepository.GetBatchByIdAsync(batchId);
            if (batch == null) return null;

            // 🔒 Authorization Check
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<ProgramBatch>(batchId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to access this batch.");

            return _mapper.Map<BatchResponseDto>(batch);
        }

        public async Task<BatchResponseDto> CreateBatchAsync(CreateBatchDto createBatchDto)
        {
            var program = await _programRepository.GetProgramByIdAsync(createBatchDto.ProgramId);
            if (program == null) throw new KeyNotFoundException("Program not found");

            // 🔒 Authorization Check
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Programs>(createBatchDto.ProgramId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to create a batch in this program.");

            // ✅ Ensure batch name is unique within the campus
            bool nameExists = await _batchRepository.IsBatchNameExistsAsync(program.Department.School.CampusId, createBatchDto.BatchName);
            if (nameExists)
                throw new InvalidOperationException("A batch with the same name already exists in this campus.");

            var batch = _mapper.Map<ProgramBatch>(createBatchDto);
            batch.CreatedAt = DateTime.UtcNow;

            var createdBatch = await _batchRepository.CreateBatchAsync(batch);
            return _mapper.Map<BatchResponseDto>(createdBatch);
        }

        public async Task<BatchResponseDto?> UpdateBatchAsync(int batchId, UpdateBatchDto dto)
        {
            var existingBatch = await _batchRepository.GetBatchByIdAsync(batchId);
            if (existingBatch == null) return null;

            // 🔒 Authorization Check
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<ProgramBatch>(batchId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to update this batch.");

            // ✅ Ensure batch name is unique within the campus (excluding itself)
            bool nameExists = await _batchRepository.IsBatchNameExistsAsync(existingBatch.Program.Department.School.CampusId, dto.BatchName, batchId);
            if (nameExists)
                throw new InvalidOperationException("A batch with the same name already exists in this campus.");

            _mapper.Map(dto, existingBatch);

            existingBatch.UpdatedAt = DateTime.UtcNow;

            var updatedBatch = await _batchRepository.UpdateBatchAsync(existingBatch);
            return _mapper.Map<BatchResponseDto>(updatedBatch);
        }

        public async Task<bool> DeleteBatchAsync(int batchId)
        {
            // 🔒 Authorization Check
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<ProgramBatch>(batchId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to delete this batch.");

            return await _batchRepository.DeleteBatchAsync(batchId);
        }

        public async Task<bool> SoftDeleteBatchAsync(int batchId)
        {
            // 🔒 Authorization Check
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<ProgramBatch>(batchId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to soft delete this batch.");

            return await _batchRepository.SoftDeleteBatchAsync(batchId);
        }
    }
}
