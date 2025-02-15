using AutoMapper;
using Dynamic_RBAMS.DTOs;
using Dynamic_RBAMS.DTOs.Dynamic_RBAMS.DTOs;
using Dynamic_RBAMS.Interfaces;
using Dynamic_RBAMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dynamic_RBAMS.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly ICampusRepository _campusRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, ISchoolRepository schoolRepository,
            IMapper mapper, ICampusRepository campusRepository)
        {
            _departmentRepository = departmentRepository;
            _schoolRepository = schoolRepository;
            _mapper = mapper;
            _campusRepository = campusRepository;
        }

        public async Task<IEnumerable<DepartmentResponseDto>?> GetDepartmentsByCampusAsync(int campusId)
        {
            
            var campus = await _campusRepository.GetByIdAsync(campusId);
            if (campus == null) return null; // Indicate campus does not exist

            var departments = await _departmentRepository.GetDepartmentsByCampusAsync(campusId);
            return _mapper.Map<IEnumerable<DepartmentResponseDto>>(departments);
        }

        public async Task<DepartmentResponseDto?> GetDepartmentByIdAsync(int departmentId)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(departmentId);
            return department == null ? null : _mapper.Map<DepartmentResponseDto>(department);
        }

        public async Task<DepartmentResponseDto> CreateDepartmentAsync(CreateDepartmentDto dto)
        {
            var school = await _schoolRepository.GetByIdAsync(dto.SchoolId);
            if (school == null) throw new KeyNotFoundException("School not found");

            var department = _mapper.Map<Department>(dto);
            department.CreatedAt = DateTime.UtcNow;

            var createdDepartment = await _departmentRepository.CreateDepartmentAsync(department);
            return _mapper.Map<DepartmentResponseDto>(createdDepartment);
        }

        public async Task<DepartmentResponseDto?> UpdateDepartmentAsync(int departmentId, UpdateDepartmentDto dto)
        {
            var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(departmentId);
            if (existingDepartment == null) return null;
             
            _mapper.Map(dto, existingDepartment);
            existingDepartment.UpdatedAt = DateTime.UtcNow;

            var updatedDepartment = await _departmentRepository.UpdateDepartmentAsync(existingDepartment);
            return _mapper.Map<DepartmentResponseDto>(updatedDepartment);
        }

        public async Task<bool> SoftDeleteDepartmentAsync(int departmentId)
        {
            return await _departmentRepository.SoftDeleteDepartmentAsync(departmentId);
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            return await _departmentRepository.DeleteDepartmentAsync(departmentId);
        }
    }
}
