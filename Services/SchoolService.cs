using Dynamic_RBAMS.Interfaces;
using Dynamic_RBAMS.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dynamic_RBAMS.Models;

namespace Dynamic_RBAMS.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly ICampusRepository _campusRepository;

        public SchoolService(ISchoolRepository schoolRepository, ICampusRepository campusRepository)
        {
            _schoolRepository = schoolRepository;
            _campusRepository = campusRepository;
        }

        public async Task<ApiResponseDto> CreateSchoolAsync(CreateSchoolDto dto)
        {
            var campusExists = await _campusRepository.GetByIdAsync(dto.CampusId);
            if (campusExists == null) return new ApiResponseDto(400, "Invalid Campus Id.");

            var existingSchool = await _schoolRepository.GetByNameAsync(dto.Name);
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
            return school == null
                ? new ApiResponseDto(404, "School not found.")
                : new ApiResponseDto(200, "School retrieved successfully.", MapToDto(school));
        }

        public async Task<ApiResponseDto> GetAllSchoolsByCampusIdAsync(int campusId)
        {
            var campusExists = await _campusRepository.GetByIdAsync(campusId);
            if (campusExists == null) return new ApiResponseDto(400, "Invalid Campus Id.");

            var schools = await _schoolRepository.GetAllByCampusIdAsync(campusId);
            var schoolDtos = schools.Select(MapToDto).ToList();

            return new ApiResponseDto(200, "Schools retrieved successfully.", schoolDtos);
        }

        public async Task<ApiResponseDto> UpdateSchoolAsync(int id, UpdateSchoolDto dto)
        {
            var school = await _schoolRepository.GetByIdAsync(id);
            if (school == null) return new ApiResponseDto(404, "School not found.");

            var existingSchool = await _schoolRepository.GetByNameAsync(dto.Name);
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
            var success = await _schoolRepository.DeleteAsync(id);
            return success
                ? new ApiResponseDto(200, "School deleted successfully.")
                : new ApiResponseDto(404, "School not found.");
        }

        public async Task<ApiResponseDto> SoftDeleteSchoolAsync(int id)
        {
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
