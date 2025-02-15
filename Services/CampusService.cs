using Dynamic_RBAMS.Interfaces;
using Dynamic_RBAMS.Models;
using Dynamic_RBAMS.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Dynamic_RBAMS.Services
{
    public class CampusService : ICampusService
    {
        private readonly ICampusRepository _campusRepository;
        private readonly IUniversityRepository _universityRepository;

        public CampusService(ICampusRepository campusRepository, IUniversityRepository universityRepository)
        {
            _campusRepository = campusRepository;
            _universityRepository = universityRepository;
        }

        public async Task<ApiResponseDto> CreateCampusAsync(CreateCampusDto dto)
        {
            // Validate University Existence First
            var universityExists = await _universityRepository.GetByIdAsync(dto.UniversityId);
            if (universityExists == null)
            {
                return new ApiResponseDto(400, "Invalid University Id");
            }

            // Validate Unique Campus Name
            var existingCampus = await _campusRepository.GetByNameAsync(dto.Name);
            if (existingCampus != null)
            {
                return new ApiResponseDto(400, "Campus name already exists");
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
            var responseDto = new CampusResponseDto(campus);

            return new ApiResponseDto(201, "Campus created successfully", responseDto);
        }

        public async Task<ApiResponseDto> GetCampusByIdAsync(int id)
        {
            var campusDto = await _campusRepository.GetByIdAsync(id);
            if (campusDto == null)
            {
                return new ApiResponseDto(404, "Campus not found");
            }
            return new ApiResponseDto(200, "Campus retrieved successfully", campusDto);
        }

        public async Task<ApiResponseDto> GetAllCampusesByUniversityIdAsync(int universityId)
        {
            var campuses = await _campusRepository.GetAllByUniversityIdAsync(universityId);
            return new ApiResponseDto(200, "Campuses retrieved successfully", campuses);
        }

        public async Task<ApiResponseDto> DeleteCampusAsync(int id)
        {
            var campus = await _campusRepository.GetEntityByIdAsync(id);
            if (campus == null)
            {
                return new ApiResponseDto(404, "Campus not found");
            }

            await _campusRepository.DeleteAsync(campus);
            return new ApiResponseDto(200, "Campus deleted successfully");
        }

        public async Task<ApiResponseDto> UpdateCampusAsync(int id, UpdateCampusDto dto)
        {
            var campus = await _campusRepository.GetEntityByIdAsync(id);
            if (campus == null)
            {
                return new ApiResponseDto(404, "Campus not found");
            }

            if (campus.CampusName != dto.Name)
            {
                var existingCampus = await _campusRepository.GetByNameAsync(dto.Name);
                if (existingCampus != null)
                {
                    return new ApiResponseDto(400, "Campus name already exists");
                }
            }

            campus.CampusName = dto.Name;
            campus.ShortName = dto.ShortName;
            campus.Address = dto.Address;
            campus.City = dto.City;
            campus.Notes = dto.Notes;
            campus.Type = dto.Type;
            campus.UpdatedAt = System.DateTime.Now;

            await _campusRepository.UpdateAsync(campus);
            var responseDto = new CampusResponseDto(campus);

            return new ApiResponseDto(200, "Campus updated successfully", responseDto);
        }

        public async Task<ApiResponseDto> SoftDeleteCampusAsync(int id)
        {
            var campus = await _campusRepository.GetEntityByIdAsync(id);
            if (campus == null)
            {
                return new ApiResponseDto(404, "Campus not found");
            }
            if (campus.IsDeleted)
            {
                return new ApiResponseDto(400, "Campus is already soft deleted. Perform hard delete manually if needed.");
            }

            await _campusRepository.SoftDeleteAsync(campus);
            return new ApiResponseDto(200, "Campus soft deleted successfully");
        }
    }
}
