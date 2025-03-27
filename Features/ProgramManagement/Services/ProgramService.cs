using AutoMapper;
using LMS.Features.DepartmentManagement.Repositories;
using LMS.Features.ProgramManagement.Dtos;
using LMS.Features.ProgramManagement.Repositories;
using LMS.Features.CampusManagement.Repositories;
using LMS.Features.Common.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using LMS.Features.DepartmentManagement;

namespace LMS.Features.ProgramManagement.Services
{
    public class ProgramService : IProgramService
    {
        private readonly IMapper _mapper;
        private readonly IProgramRepository _programRepository;
        private readonly ICampusRepository _campusRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ICampusEntityAuthorizationService _campusAuthorizationService;

        public ProgramService(
            IMapper mapper,
            IProgramRepository programRepository,
            ICampusRepository campusRepository,
            IDepartmentRepository departmentRepository,
            ICampusEntityAuthorizationService campusAuthorizationService)
        {
            _mapper = mapper;
            _programRepository = programRepository;
            _campusRepository = campusRepository;
            _departmentRepository = departmentRepository;
            _campusAuthorizationService = campusAuthorizationService;
        }

        public async Task<IEnumerable<ProgramResponseDto>?> GetProgramsByCampusAsync(int campusId)
        {
            var campus = await _campusRepository.GetByIdAsync(campusId);
            if (campus == null) return null;

            var programs = await _programRepository.GetProgramsByCampusAsync(campusId);
            return _mapper.Map<IEnumerable<ProgramResponseDto>>(programs);
        }

        public async Task<ProgramResponseDto?> GetProgramByIdAsync(int programId)
        {
            var program = await _programRepository.GetProgramByIdAsync(programId);
            if (program == null) return null;

            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Programs>(programId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to access this program.");

            return _mapper.Map<ProgramResponseDto>(program);
        }

        public async Task<ProgramResponseDto> CreateProgramAsync(CreateProgramDto createProgramDto)
        {
            if (createProgramDto.DepartmentId <= 0)
                throw new ArgumentException("Invalid DepartmentId provided.");

            var department = await _departmentRepository.GetDepartmentByIdAsync(createProgramDto.DepartmentId);
            if (department == null)
                throw new KeyNotFoundException("Department not found");

            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Department>(createProgramDto.DepartmentId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to create a program in this department.");

            bool nameExists = await _programRepository.IsProgramNameExistsAsync(department.School.CampusId, createProgramDto.ProgramName);
            if (nameExists)
                throw new InvalidOperationException("A program with the same name already exists in this campus.");

            var program = _mapper.Map<Programs>(createProgramDto);
            program.CreatedAt = DateTime.UtcNow;

            var createdProgram = await _programRepository.CreateProgramAsync(program);
            return _mapper.Map<ProgramResponseDto>(createdProgram);
        }

        public async Task<ProgramResponseDto?> UpdateProgramAsync(int programId, UpdateProgramDto dto)
        {
            var existingProgram = await _programRepository.GetProgramByIdAsync(programId);
            if (existingProgram == null) return null;

            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Programs>(programId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to update this program.");


            bool nameExists = await _programRepository.IsProgramNameExistsAsync(existingProgram.Department.School.CampusId, dto.ProgramName, programId);
            if (nameExists)
                throw new InvalidOperationException("A program with the same name already exists in this campus.");

            _mapper.Map(dto, existingProgram);
            existingProgram.UpdatedAt = DateTime.UtcNow;

            var updatedProgram = await _programRepository.UpdateProgramAsync(existingProgram);
            return _mapper.Map<ProgramResponseDto>(updatedProgram);
        }

        public async Task<bool> DeleteProgramAsync(int programId)
        {
            // 🔒 Authorization Check
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Programs>(programId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to delete this program.");

            return await _programRepository.DeleteProgramAsync(programId);
        }

        public async Task<bool> SoftDeleteProgramAsync(int programId)
        {
            // 🔒 Authorization Check
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Programs>(programId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to soft delete this program.");

            return await _programRepository.SoftDeleteProgramAsync(programId);
        }
    }
}
