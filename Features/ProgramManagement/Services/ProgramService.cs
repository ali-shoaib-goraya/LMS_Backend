using AutoMapper;
using Dynamic_RBAMS.Features.DepartmentManagement.Dtos;
using Dynamic_RBAMS.Features.DepartmentManagement;
using Dynamic_RBAMS.Features.ProgramManagement.Dtos;
using Dynamic_RBAMS.Features.ProgramManagement.Repositories;
using Dynamic_RBAMS.Features.DepartmentManagement.Repositories;
using Dynamic_RBAMS.Features.CampusManagement.Repositories;

namespace Dynamic_RBAMS.Features.ProgramManagement.Services
{
    public class ProgramService : IProgramService
    {
        private readonly IMapper _mapper;
        private readonly IProgramRepository _programRepository;
        private readonly ICampusRepository _campusRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public ProgramService(IMapper mapper, IProgramRepository programRepository, ICampusRepository campusRepository, IDepartmentRepository departmentRepository)
        {
            _mapper = mapper;
            _programRepository = programRepository;
            _campusRepository = campusRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<ProgramResponseDto>?> GetProgramsByCampusAsync(int campusId)
        {
            var campus = await _campusRepository.GetByIdAsync(campusId);
            if (campus == null) return null; // Indicate campus does not exist

            var programs = await _programRepository.GetProgramsByCampusAsync(campusId);
            return _mapper.Map<IEnumerable<ProgramResponseDto>>(programs);
        }

        public async Task<ProgramResponseDto?> GetProgramByIdAsync(int programId)
        {
            var program = await _programRepository.GetProgramByIdAsync(programId);
            return program == null ? null : _mapper.Map<ProgramResponseDto>(program);
        }

        public async Task<ProgramResponseDto> CreateProgramAsync(CreateProgramDto createProgramDto)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(createProgramDto.DepartmentId);
            if (department == null) throw new KeyNotFoundException("Department not found");

            var program = _mapper.Map<Programs>(createProgramDto);
            program.CreatedAt = DateTime.UtcNow;
            var createdProgram = await _programRepository.CreateProgramAsync(program);
            return _mapper.Map<ProgramResponseDto>(createdProgram);
        }

        public async Task<ProgramResponseDto?> UpdateProgramAsync(int programId, UpdateProgramDto dto)
        {
            var existingProgram = await _programRepository.GetProgramByIdAsync(programId);
            if (existingProgram == null) return null;

            _mapper.Map(dto, existingProgram);
            existingProgram.UpdatedAt = DateTime.UtcNow;

            var updatedProgram = await _programRepository.UpdateProgramAsync(existingProgram);
            return _mapper.Map<ProgramResponseDto>(updatedProgram);
        }

        public async Task<bool> DeleteProgramAsync(int programId)
        {
            return await _programRepository.DeleteProgramAsync(programId);
        }

        public async Task<bool> SoftDeleteProgramAsync(int programId)
        {
            return await _programRepository.SoftDeleteProgramAsync(programId);
        }

    }
}
