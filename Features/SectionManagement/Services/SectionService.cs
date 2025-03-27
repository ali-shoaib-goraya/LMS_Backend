using AutoMapper;
using LMS.Features.BatchManagement.Repositories;
using LMS.Features.ProgramManagement.Repositories;
using LMS.Features.Common.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using LMS.Features.BatchManagement.Dtos;
using LMS.Features.BatchManagement;
using LMS.Features.ProgramManagement;
using LMS.Features.SectionManagement.Repositories;
using LMS.Features.SectionManagement.Dtos;

namespace LMS.Features.SectionManagement.Services
{
    public class SectionService : ISectionService
    { 
        private readonly IMapper _mapper;
        private readonly ISectionRepository _sectionRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly ICampusEntityAuthorizationService _campusAuthorizationService;
        private readonly IUserContextService _userContextService;

        public SectionService(
            IMapper mapper,
            ISectionRepository sectionRepository,
            IBatchRepository batchRepository,
            ICampusEntityAuthorizationService campusAuthorizationService,
            IUserContextService userContextService) 
        {
            _mapper = mapper;
            _sectionRepository = sectionRepository;
            _batchRepository = batchRepository;
            _campusAuthorizationService = campusAuthorizationService;
            _userContextService = userContextService;
        }
         
        public async Task<IEnumerable<SectionResponseDto>?> GetSectionsByCampusAsync()
        {
            // 🔒 Ensure CampusAdmin is fetching only their own campus data
            var userCampusId = _userContextService.GetCampusId();
            if (userCampusId == null)
                throw new UnauthorizedAccessException("You do not have permission to access batch sections from this campus.");

            var batchSections = await _sectionRepository.GetSectionsByCampusAsync(userCampusId.Value);
            return _mapper.Map<IEnumerable<SectionResponseDto>>(batchSections);
        }

        public async Task<SectionResponseDto?> GetSectionByIdAsync(int batchSectionId)
        {
            var batchSection = await _sectionRepository.GetSectionByIdAsync(batchSectionId);
            if (batchSection == null) return null;

            // 🔒 Authorization Check
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<ProgramBatchSection>(batchSectionId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to access this batch section.");

            return _mapper.Map<SectionResponseDto>(batchSection);
        }

        public async Task<SectionResponseDto> CreateSectionAsync(CreateSectionDto createBatchSectionDto)
        {
            var batch = await _batchRepository.GetBatchByIdAsync(createBatchSectionDto.ProgramBatchId);
            if (batch == null) throw new KeyNotFoundException("Batch not found");

            // 🔒 Authorization Check
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<ProgramBatch>(createBatchSectionDto.ProgramBatchId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to create a batch section in this batch.");

            // ✅ Ensure batch section name is unique within the campus
            bool nameExists = await _sectionRepository.IsSectionNameExistsAsync(createBatchSectionDto.ProgramBatchId, createBatchSectionDto.SectionName);
            if (nameExists)
                throw new InvalidOperationException("A batch section with the same name already exists in this campus.");

            var batchSection = _mapper.Map<ProgramBatchSection>(createBatchSectionDto); 
            batchSection.CreatedAt = DateTime.UtcNow;

            var createdBatchSection = await _sectionRepository.CreateSectionAsync(batchSection);
            return _mapper.Map<SectionResponseDto>(createdBatchSection);
        }

        public async Task<SectionResponseDto?> UpdateSectionAsync(int batchSectionId, UpdateSectionDto dto)
        {
            var existingBatchSection = await _sectionRepository.GetSectionByIdAsync(batchSectionId);
            if (existingBatchSection == null) return null;

            // 🔒 Authorization Check
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<ProgramBatchSection>(batchSectionId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to update this batch section.");

            // ✅ Ensure section name is unique within the batch (excluding itself)
            bool nameExists = await _sectionRepository.IsSectionNameExistsAsync(existingBatchSection.ProgramBatchId, dto.SectionName, batchSectionId);
            if (nameExists)
                throw new InvalidOperationException("A batch section with the same name already exists in this campus.");

            // ✅ Map DTO to entity
            existingBatchSection.SectionName = dto.SectionName;
            existingBatchSection.Capacity = dto.Capacity;
            existingBatchSection.UpdatedAt = DateTime.UtcNow;

            var updatedBatchSection = await _sectionRepository.UpdateSectionAsync(existingBatchSection);
            return _mapper.Map<SectionResponseDto>(updatedBatchSection);
        }


        public async Task<bool> DeleteSectionAsync(int batchSectionId)
        {
            // 🔒 Authorization Check
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<ProgramBatchSection>(batchSectionId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to delete this batch section.");

            return await _sectionRepository.DeleteSectionAsync(batchSectionId);
        }

        public async Task<bool> SoftDeleteSectionAsync(int batchSectionId)
        {
            // 🔒 Authorization Check
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<ProgramBatchSection>(batchSectionId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to soft delete this batch section.");

            return await _sectionRepository.SoftDeleteSectionAsync(batchSectionId);
        }
    }
}
