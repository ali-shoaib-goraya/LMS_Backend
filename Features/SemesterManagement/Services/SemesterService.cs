using LMS.Features.SemesterManagement.Dtos;
using LMS.Features.SemesterManagement.Repositories;
using AutoMapper;
using LMS.Features.Common.Services;
using SendGrid.Helpers.Errors.Model;

namespace LMS.Features.SemesterManagement.Services
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public SemesterService(ISemesterRepository semesterRepository, IMapper mapper, IUserContextService userContextService)
        {
            _semesterRepository = semesterRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<SemesterResponseDto>> GetAllSemestersByCampusAsync(int campusId)
        {
            var semesters = await _semesterRepository.GetAllSemestersByCampusAsync(campusId);
            return _mapper.Map<IEnumerable<SemesterResponseDto>>(semesters);
        }

        public async Task<SemesterResponseDto?> GetSemesterByIdAsync(int semesterId)
        {
            var semester = await _semesterRepository.GetSemesterByIdAsync(semesterId);
            if (semester == null) return null;

            var userCampusId = _userContextService.GetCampusId();
            if (semester.CampusId != userCampusId)
                throw new ForbiddenException("Unauthorized access to this semester.");

            return _mapper.Map<SemesterResponseDto>(semester);
        }

        public async Task<SemesterResponseDto> CreateSemesterAsync(CreateSemesterDto createDto)
        {
            var semester = _mapper.Map<Semester>(createDto);
            semester.CreatedAt = DateTime.UtcNow;
            semester.UpdatedAt = DateTime.UtcNow;

            var campusId = _userContextService.GetCampusId();
            if (!campusId.HasValue)
            {
                throw new ForbiddenException("Unauthorized access to create semester.");
            } 
            semester.CampusId = campusId.Value;

            var createdSemester = await _semesterRepository.CreateSemesterAsync(semester);
            return _mapper.Map<SemesterResponseDto>(createdSemester);
        }

        public async Task<SemesterResponseDto> UpdateSemesterAsync(int semesterId, UpdateSemesterDto updateDto)
        {
            var semester = await _semesterRepository.GetSemesterByIdAsync(semesterId);
            if (semester == null) throw new NotFoundException("Semester not found.");

            var userCampusId = _userContextService.GetCampusId();
            if (semester.CampusId != userCampusId)
                throw new ForbiddenException("Unauthorized access to this semester.");

            if (!string.IsNullOrEmpty(updateDto.SemesterName)) semester.SemesterName = updateDto.SemesterName;
            if (!string.IsNullOrEmpty(updateDto.Status)) semester.Status = updateDto.Status;
            if (updateDto.StartDate.HasValue) semester.StartDate = updateDto.StartDate.Value;
            if (updateDto.EndDate.HasValue) semester.EndDate = updateDto.EndDate.Value;
            if (!string.IsNullOrEmpty(updateDto.Notes)) semester.Notes = updateDto.Notes;

            semester.UpdatedAt = DateTime.UtcNow;

            await _semesterRepository.UpdateSemesterAsync(semester);
            return _mapper.Map<SemesterResponseDto>(semester);
        }

        public async Task<bool> DeleteSemesterAsync(int semesterId)
        {
            var semester = await _semesterRepository.GetSemesterByIdAsync(semesterId);
            if (semester == null) throw new NotFoundException("Semester not found.");

            var userCampusId = _userContextService.GetCampusId();
            if (semester.CampusId != userCampusId)
                throw new ForbiddenException("Unauthorized access to this semester.");

            return await _semesterRepository.DeleteSemesterAsync(semesterId);
        }

        public async Task<bool> SoftDeleteSemesterAsync(int semesterId)
        {
            var semester = await _semesterRepository.GetSemesterByIdAsync(semesterId);
            if (semester == null) throw new NotFoundException("Semester not found.");

            var userCampusId = _userContextService.GetCampusId();
            if (semester.CampusId != userCampusId)
                throw new ForbiddenException("Unauthorized access to this semester.");

            return await _semesterRepository.SoftDeleteSemesterAsync(semesterId);
        }
    }
}
