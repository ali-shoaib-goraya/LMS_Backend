using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Features.CampusManagement;
using LMS.Features.CampusManagement.Repositories;
using LMS.Features.Common.Dtos;
using LMS.Features.Common.Services;
using LMS.Features.SchoolManagement.Dtos;
using LMS.Features.SchoolManagement.Repositories;

namespace LMS.Features.SchoolManagement.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly ICampusRepository _campusRepository;
        private readonly IUserContextService _userContextService;
        private readonly ICampusEntityAuthorizationService _campusAuthorizationService; 

        public SchoolService(ISchoolRepository schoolRepository, ICampusRepository campusRepository, IUserContextService userContextService, ICampusEntityAuthorizationService campusAuthorizationService)
        {
            _schoolRepository = schoolRepository;
            _campusRepository = campusRepository;
            _userContextService = userContextService;
            _campusAuthorizationService = campusAuthorizationService;
        }

        public async Task<ApiResponseDto> CreateSchoolAsync(CreateSchoolDto dto)
        {
            var campusExists = await _campusRepository.GetByIdAsync(dto.CampusId);
            if (campusExists == null) return new ApiResponseDto(400, "Invalid Campus Id.");

            // 🔒 Authorization: Ensure user has access to the campus before creating a School
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Campus>(dto.CampusId);
            if (!hasAccess)
                return new ApiResponseDto(403, "Access Forbidden");

            var existingSchool = await _schoolRepository.GetByNameAndCampusAsync(dto.CampusId, dto.Name);
            if (existingSchool != null)
            {
                return new ApiResponseDto(400, "A school with this name already exists in this campus.");
            }

            var school = new School
            {
                SchoolName = dto.Name,
                Address = dto.Address,
                ShortName = dto.ShortName,
                Academic = dto.Academic,
                City = dto.City,
                Notes = dto.Notes,
                CampusId = dto.CampusId
            };

            var createdSchool = await _schoolRepository.AddAsync(school);
            return new ApiResponseDto(201, "School created successfully.", MapToDto(createdSchool));
        }

        public async Task<ApiResponseDto> GetSchoolByIdAsync(int id)
        {
           
            var school = await _schoolRepository.GetByIdAsync(id);
            if (school == null)
            {
                return new ApiResponseDto(404, "School not found.");
            }

            // 🔒 Authorization: Ensure user has access to the school's campus before creating a department
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<School>(id);
            if (!hasAccess)
                return new ApiResponseDto(403, "Access Forbidden");

            return new ApiResponseDto(200, "School retrieved successfully.", MapToDto(school));
        }

        public async Task<ApiResponseDto> GetAllSchoolsByCampusAsync()
        {
            var userCampusId = _userContextService.GetCampusId(); // Get campusId from logged-in user
            if (userCampusId == null)
                return new ApiResponseDto(403, "Access Forbidden"); ; // Ensures the user has an assigned campus
              

            var campusExists = await _campusRepository.GetByIdAsync(userCampusId.Value);
            if (campusExists == null) return new ApiResponseDto(400, "Invalid Campus Id.");

            var schools = await _schoolRepository.GetAllByCampusIdAsync(userCampusId.Value);
            var schoolDtos = schools.Select(MapToDto).ToList();

            return new ApiResponseDto(200, "Schools retrieved successfully.", schoolDtos);
        }

        public async Task<ApiResponseDto> UpdateSchoolAsync(int id, UpdateSchoolDto dto)
        {
            var school = await _schoolRepository.GetByIdAsync(id);
            if (school == null) return new ApiResponseDto(404, "School not found.");

            // 🔒 Authorization: Ensure user has access to the school
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<School>(id);
            if (!hasAccess)
                return new ApiResponseDto(403, "Access Forbidden");

            var CampusId = school.CampusId;
            var existingSchool = await _schoolRepository.GetByNameAndCampusAsync(CampusId, dto.Name);
            if (existingSchool != null && existingSchool.SchoolId != id)
                return new ApiResponseDto(400, "A school with this name already exists in this campus.");

            school.SchoolName = dto.Name;
            school.Address = dto.Address;
            school.ShortName = dto.ShortName;
            school.Academic = dto.Academic;
            school.City = dto.City;
            school.Notes = dto.Notes;

            var updatedSchool = await _schoolRepository.UpdateAsync(school);
            return new ApiResponseDto(200, "School updated successfully.", MapToDto(updatedSchool));
        }

        public async Task<ApiResponseDto> DeleteSchoolAsync(int id)
        {
            // 🔒 Authorization: Ensure user has access to the school
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<School>(id);
            if (!hasAccess)
                return new ApiResponseDto(403, "Access Forbidden");

            var school = await _schoolRepository.GetByIdAsync(id);
            if (school == null)
            {
                return new ApiResponseDto(404, "School NoT Found");
            }
            var schoolDto = MapToDto(school);
            var success = await _schoolRepository.DeleteAsync(id);
            return success
                ? new ApiResponseDto(200, "School deleted successfully.", schoolDto)
                : new ApiResponseDto(400, "Cannot delete school. Delete all departments first.");
        }

        public async Task<ApiResponseDto> SoftDeleteSchoolAsync(int id)
        {
            // 🔒 Authorization: Ensure user has access to the school
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<School>(id);
            if (!hasAccess)
                return new ApiResponseDto(403, "Access Forbidden");

            var success = await _schoolRepository.SoftDeleteAsync(id);
            return success
                ? new ApiResponseDto(200, "School soft deleted successfully.")
                : new ApiResponseDto(400, "School is already soft deleted or not found.");
        }

        // ✅ Private helper method to map model to DTO
        private static SchoolResponseDto MapToDto(School school)
        {
            return new SchoolResponseDto
            {
                SchoolId = school.SchoolId,
                SchoolName = school.SchoolName,
                ShortName = school.ShortName,
                Address = school.Address,
                Academic = school.Academic,
                City = school.City,
                Notes = school.Notes
            };
        }
    }
}

