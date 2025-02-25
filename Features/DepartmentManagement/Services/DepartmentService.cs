using AutoMapper;
using Dynamic_RBAMS.Features.CampusManagement.Repositories;
using Dynamic_RBAMS.Features.Common.Services;
using Dynamic_RBAMS.Features.DepartmentManagement.Dtos;
using Dynamic_RBAMS.Features.DepartmentManagement.Repositories;
using Dynamic_RBAMS.Features.SchoolManagement;
using Dynamic_RBAMS.Features.SchoolManagement.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dynamic_RBAMS.Features.DepartmentManagement.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly ICampusRepository _campusRepository;
        private readonly IMapper _mapper;
        private readonly ICampusEntityAuthorizationService _campusAuthorizationService; // ✅ Injected

        public DepartmentService(IDepartmentRepository departmentRepository, ISchoolRepository schoolRepository,
            IMapper mapper, ICampusRepository campusRepository,
            ICampusEntityAuthorizationService campusAuthorizationService) // ✅ Injected
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
            if (campus == null) return null; // Indicate campus does not exist

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

            // 🔒 Authorization: Ensure user has access to the department's campus
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Department>(departmentId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to access this department");

            return _mapper.Map<DepartmentResponseDto>(department);
        }

        public async Task<DepartmentResponseDto> CreateDepartmentAsync(CreateDepartmentDto dto)
        {
            var school = await _schoolRepository.GetByIdAsync(dto.SchoolId);
            if (school == null) throw new KeyNotFoundException("School not found");

            // 🔒 Authorization: Ensure user has access to the school's campus before creating a department
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<School>(dto.SchoolId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to create a department in this school");

            var department = _mapper.Map<Department>(dto);
            department.CreatedAt = DateTime.UtcNow;

            var createdDepartment = await _departmentRepository.CreateDepartmentAsync(department);
            return _mapper.Map<DepartmentResponseDto>(createdDepartment);
        }

        public async Task<DepartmentResponseDto?> UpdateDepartmentAsync(int departmentId, UpdateDepartmentDto dto)
        {
            var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(departmentId);
            if (existingDepartment == null) return null;

            // 🔒 Authorization: Ensure user has access to update this department
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Department>(departmentId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to update this department");

            _mapper.Map(dto, existingDepartment);
            existingDepartment.UpdatedAt = DateTime.UtcNow;

            var updatedDepartment = await _departmentRepository.UpdateDepartmentAsync(existingDepartment);
            return _mapper.Map<DepartmentResponseDto>(updatedDepartment);
        }

        public async Task<bool> SoftDeleteDepartmentAsync(int departmentId)
        {
            // 🔒 Authorization: Ensure user has access before soft deleting
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Department>(departmentId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to delete this department");

            return await _departmentRepository.SoftDeleteDepartmentAsync(departmentId);
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            // 🔒 Authorization: Ensure user has access before hard deleting
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Department>(departmentId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to delete this department");

            return await _departmentRepository.DeleteDepartmentAsync(departmentId);
        }
    }
}
