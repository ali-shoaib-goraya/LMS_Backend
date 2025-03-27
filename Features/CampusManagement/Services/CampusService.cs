using System.Threading.Tasks;
using System.Collections.Generic;
using LMS.Features.Common.Dtos;
using LMS.Features.CampusManagement.Repositories;
using LMS.Features.CampusManagement.Dtos;
using LMS.Features.Common.Services;
using LMS.Features.UniveristyManagement.Repositories;

namespace LMS.Features.CampusManagement.Services
{
    public class CampusService : ICampusService
    {
        private readonly ICampusRepository _campusRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IUserContextService _userContextService;

        public CampusService(ICampusRepository campusRepository, IUniversityRepository universityRepository, IUserContextService userContextService)
        {
            _campusRepository = campusRepository;
            _universityRepository = universityRepository;
            _userContextService = userContextService;
        }

        // ✅ Create a new campus
        public async Task<ApiResponseDto> CreateCampusAsync(CreateCampusDto dto)
        {
            
            var universityExists = await _universityRepository.GetByIdAsync(dto.UniversityId);
            if (universityExists == null)
            {
                return new ApiResponseDto(400, "Invalid University Id");
            }
            var universityId = _userContextService.GetUniversityId();
            if (universityId == null || universityId != dto.UniversityId)
            {
                return new ApiResponseDto(403, "Access Forbidden");
            }

            var existingCampus = await _campusRepository.GetByNameAndUniversityAsync(dto.UniversityId, dto.Name);
            if (existingCampus != null)
            {
                return new ApiResponseDto(400, "Campus name already exists for this university");
            }

            var campus = new Campus
            {
                CampusName = dto.Name,
                UniversityId = dto.UniversityId,
                ShortName = dto.ShortName,
                Address = dto.Address,
                City = dto.City,
                Notes = dto.Notes,
                Type = dto.Type
            };

            await _campusRepository.AddAsync(campus);
            return new ApiResponseDto(201, "Campus created successfully", new CampusResponseDto(campus));
        }

        // ✅ Get campus by ID
        public async Task<ApiResponseDto> GetCampusByIdAsync(int id)
        {
            var universityId = _userContextService.GetUniversityId();
            var campus = await _campusRepository.GetByIdAsync(id);

            if (campus == null)
                return new ApiResponseDto(404, "Campus not found");

            if (universityId == null || campus.UniversityId != universityId)
                return new ApiResponseDto(403, "Access Forbidden");

            return new ApiResponseDto(200, "Campus retrieved successfully", new CampusResponseDto(campus));
        }

        // ✅ Get all campuses for the current university
        public async Task<ApiResponseDto> GetAllCampusesByUniversityAsync()
        {
            var universityId = _userContextService.GetUniversityId();
            if (universityId == null)
                return new ApiResponseDto(403, "Access Forbidden");

            var campuses = await _campusRepository.GetAllByUniversityIdAsync(universityId.Value);
            return new ApiResponseDto(200, "Campuses retrieved successfully", campuses);
        }

        // ✅ Update campus details
        public async Task<ApiResponseDto> UpdateCampusAsync(int id, UpdateCampusDto dto)
        {
            var universityId = _userContextService.GetUniversityId();
            var campus = await _campusRepository.GetByIdAsync(id);

            if (campus == null)
                return new ApiResponseDto(404, "Campus not found");

            if (universityId == null || campus.UniversityId != universityId)
                return new ApiResponseDto(403, "Access Forbidden");

            if (campus.CampusName != dto.Name)
            {
                var existingCampus = await _campusRepository.GetByNameAndUniversityAsync(universityId.Value, dto.Name);
                if (existingCampus != null)
                {
                    return new ApiResponseDto(400, "Campus name already exists for this university");
                }
            }

            campus.CampusName = dto.Name;
            campus.ShortName = dto.ShortName;
            campus.Address = dto.Address;
            campus.City = dto.City;
            campus.Notes = dto.Notes;
            campus.Type = dto.Type;
            campus.UpdatedAt = DateTime.UtcNow;

            await _campusRepository.UpdateAsync(campus);
            return new ApiResponseDto(200, "Campus updated successfully", new CampusResponseDto(campus));
        }

        // ✅ Soft delete campus
        public async Task<ApiResponseDto> SoftDeleteCampusAsync(int id)
        {
            var universityId = _userContextService.GetUniversityId();
            var campus = await _campusRepository.GetByIdAsync(id);

            if (campus == null)
                return new ApiResponseDto(404, "Campus not found");

            if (universityId == null || campus.UniversityId != universityId)
                return new ApiResponseDto(403, "Access Forbidden");

            if (campus.IsDeleted)
                return new ApiResponseDto(400, "Campus is already soft deleted. Perform hard delete manually if needed.");

            await _campusRepository.SoftDeleteAsync(id);
            return new ApiResponseDto(200, "Campus soft deleted successfully");
        }

        // ✅ Hard delete campus
        public async Task<ApiResponseDto> DeleteCampusAsync(int id)
        {
            var universityId = _userContextService.GetUniversityId();
            var campus = await _campusRepository.GetByIdAsync(id);

            if (campus == null)
                return new ApiResponseDto(404, "Campus not found");

            if (universityId == null || campus.UniversityId != universityId)
                return new ApiResponseDto(403, "Access Forbidden");

            var deleted = await _campusRepository.DeleteAsync(id);
            if (!deleted)
                return new ApiResponseDto(400, "Failed to delete campus. Ensure it has no dependencies.");

            return new ApiResponseDto(200, "Campus deleted successfully");
        }
    }
}


