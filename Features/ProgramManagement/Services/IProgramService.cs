using LMS.Features.ProgramManagement.Dtos;

namespace LMS.Features.ProgramManagement.Services
{
    public interface IProgramService
    {
        Task<IEnumerable<ProgramResponseDto>?> GetProgramsByCampusAsync(int campusId);
        Task<ProgramResponseDto?> GetProgramByIdAsync(int programId);
        Task<ProgramResponseDto> CreateProgramAsync(CreateProgramDto createProgramDto);
        Task<ProgramResponseDto?> UpdateProgramAsync(int programId, UpdateProgramDto updateProgramDto); 
        Task<bool> DeleteProgramAsync(int programId);
        Task<bool> SoftDeleteProgramAsync(int programId);
    }
}
