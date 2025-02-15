using Dynamic_RBAMS.Interfaces;
using Dynamic_RBAMS.Models;
using Dynamic_RBAMS.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dynamic_RBAMS.DTOs.Dynamic_RBAMS.DTOs;
using Microsoft.EntityFrameworkCore;
using Dynamic_RBAMS.Repos;

namespace Dynamic_RBAMS.Services
{
    public class UniversityService : IUniversityService
    {
        private readonly IUniversityRepository _repository;

        public UniversityService(IUniversityRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponseDto> CreateUniversityAsync(CreateUniversityDto dto)
        {
            var existingUniversity = await _repository.GetByNameAsync(dto.Name);
            if (existingUniversity != null)
            {
                return new ApiResponseDto(400, "University name already exists.");
            }

            var university = new University
            {
                UniversityName = dto.Name,
                Address = dto.Address
            };

            await _repository.AddAsync(university);
            return new ApiResponseDto(201, "University created successfully.", new UniversityResponseDto(university));
        }

        public async Task<ApiResponseDto> GetUniversityByIdAsync(int id)
        {
            var university = await _repository.GetByIdAsync(id);
            if (university == null)
                return new ApiResponseDto(404, "University not found.");

            return new ApiResponseDto(200, "University retrieved.", new UniversityResponseDto(university));
        }

        public async Task<ApiResponseDto> GetAllUniversitiesAsync()
        {
            var universities = await _repository.GetAllAsync();
            var universityDtos = universities.Select(u => new UniversityResponseDto(u)).ToList();
            return new ApiResponseDto(200, "Universities retrieved.", universityDtos);
        }

        public async Task<ApiResponseDto> DeleteUniversityAsync(int id)
        {
            var university = await _repository.GetByIdAsync(id);
            if (university == null)
                return new ApiResponseDto(404, "University not found.");

            if (university.Campuses.Any())
            {
                return new ApiResponseDto(400, "Cannot delete university. Delete associated campuses first.");
            }

            await _repository.DeleteAsync(university); // Hard delete

            return new ApiResponseDto(200, "University permanently deleted.");
        }

        public async Task<ApiResponseDto> SoftDeleteUniversityAsync(int id)
        {
            var university = await _repository.GetByIdAsync(id);
            if (university == null)
                return new ApiResponseDto(404, "University not found.");

            if (university.Campuses.Any()) 
            {
                return new ApiResponseDto(400, "Cannot delete university. Delete associated campuses first.");
            }

            await _repository.SoftDeleteAsync(university); // Soft delete

            return new ApiResponseDto(200, "University soft deleted successfully.");
        }

        public async Task<ApiResponseDto> UpdateUniversityAsync(int id, CreateUniversityDto dto)
        {
            var university = await _repository.GetByIdAsync(id);
            if (university == null)
            {
                return new ApiResponseDto(404, "university not found");
            }
            university.UniversityName = dto.Name;
            university.Address = dto.Address;

            await _repository.UpdateAsync(university);
            return new ApiResponseDto(200, "Campus updated successfully", new UniversityResponseDto(university));
        }
    }
}
