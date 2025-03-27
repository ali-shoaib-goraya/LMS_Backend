using AutoMapper;
using LMS.Features.CampusManagement.Repositories;
using LMS.Features.Common.Services;
using LMS.Features.DepartmentManagement.Dtos;
using LMS.Features.DepartmentManagement.Repositories;
using LMS.Features.SchoolManagement;
using LMS.Features.SchoolManagement.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Features.DepartmentManagement.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly ICampusRepository _campusRepository;
        private readonly IMapper _mapper;
        private readonly ICampusEntityAuthorizationService _campusAuthorizationService;

        public DepartmentService(IDepartmentRepository departmentRepository, ISchoolRepository schoolRepository,
            IMapper mapper, ICampusRepository campusRepository,
            ICampusEntityAuthorizationService campusAuthorizationService)
        {
            _departmentRepository = departmentRepository;
            _schoolRepository = schoolRepository;
            _mapper = mapper;
            _campusRepository = campusRepository;
            _campusAuthorizationService = campusAuthorizationService;
        }

        public async Task<IEnumerable<DepartmentResponseDto>?> GetDepartmentsByCampusAsync(int campusId)
        {
            var campus = await _campusRepository.GetByIdAsync(campusId);
            if (campus == null) return null;

            var departments = await _departmentRepository.GetDepartmentsByCampusAsync(campusId);
            return _mapper.Map<IEnumerable<DepartmentResponseDto>>(departments);
        }

        public async Task<IEnumerable<DepartmentResponseDto>?> GetAllDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllDepartmentsAsync();
            return _mapper.Map<IEnumerable<DepartmentResponseDto>>(departments);
        }

        public async Task<DepartmentResponseDto?> GetDepartmentByIdAsync(int departmentId)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(departmentId);
            if (department == null) return null;

            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Department>(departmentId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to access this department");

            return _mapper.Map<DepartmentResponseDto>(department);
        }

        public async Task<DepartmentResponseDto> CreateDepartmentAsync(CreateDepartmentDto dto)
        {
            var school = await _schoolRepository.GetByIdAsync(dto.SchoolId);
            if (school == null) throw new KeyNotFoundException("School not found");

            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<School>(dto.SchoolId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to create a department in this school");

            // 🔒 Ensure department name is unique within the campus
            bool nameExists = await _departmentRepository.IsDepartmentNameExistsAsync(school.CampusId, dto.DepartmentName);
            if (nameExists)
                throw new Exception("A department with this name already exists within the campus");

            var department = _mapper.Map<Department>(dto);
            department.CreatedAt = DateTime.UtcNow;

            var createdDepartment = await _departmentRepository.CreateDepartmentAsync(department);
            return _mapper.Map<DepartmentResponseDto>(createdDepartment);
        }

        public async Task<DepartmentResponseDto?> UpdateDepartmentAsync(int departmentId, UpdateDepartmentDto dto)
        {
            var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(departmentId);
            if (existingDepartment == null) return null;

            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Department>(departmentId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to update this department");

            // 🔒 Ensure updated department name is unique within the campus
            bool nameExists = await _departmentRepository.IsDepartmentNameExistsAsync(existingDepartment.School.CampusId, dto.DepartmentName, departmentId);
            if (nameExists)
                throw new Exception("A department with this name already exists within the campus");

            _mapper.Map(dto, existingDepartment);
            existingDepartment.UpdatedAt = DateTime.UtcNow;

            var updatedDepartment = await _departmentRepository.UpdateDepartmentAsync(existingDepartment);
            return _mapper.Map<DepartmentResponseDto>(updatedDepartment);
        }

        public async Task<bool> SoftDeleteDepartmentAsync(int departmentId)
        {
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Department>(departmentId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to delete this department");

            return await _departmentRepository.SoftDeleteDepartmentAsync(departmentId);
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Department>(departmentId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to delete this department");

            return await _departmentRepository.DeleteDepartmentAsync(departmentId);
        }
    }
}
