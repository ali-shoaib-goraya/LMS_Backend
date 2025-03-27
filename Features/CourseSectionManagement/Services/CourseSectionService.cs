using System.Threading.Tasks;
using AutoMapper;
using LMS.Features.Common.Dtos;
using LMS.Features.Common.Services;
using LMS.Features.CourseManagement.Repositories;
using LMS.Features.CourseSectionManagement.Dtos;
using LMS.Features.CourseSectionManagement.Models;
using LMS.Features.CourseSectionManagement.Repositories;
using LMS.Features.SchoolManagement.Repositories;
using LMS.Features.SectionManagement.Repositories;
using LMS.Features.SemesterManagement.Repositories;
using LMS.Features.UserManagement.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LMS.Features.CourseSectionManagement.Services
{
    public class CourseSectionService : ICourseSectionService
    {
        private readonly ICourseSectionRepository _courseSectionRepository;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IUserService _userService;
        private readonly ICourseRepository _courseRepository;
        private readonly ISchoolRepository _schoolRepository;

        public CourseSectionService(ICourseSectionRepository courseSectionRepository, IUserContextService userContextService, IMapper mapper, ISemesterRepository semesterRepository, IUserService userService, ICourseRepository courseRepository, ISchoolRepository schoolRepository)
        {
            _courseSectionRepository = courseSectionRepository;
            _userContextService = userContextService;
            _semesterRepository = semesterRepository;
            _mapper = mapper;
            _userService = userService;
            _courseRepository = courseRepository;
            _schoolRepository = schoolRepository;
        }

        public async Task<CourseSectionResponseDto> CreateCourseSectionAsync(CreateCourseSectionDto dto)
        {
            var campusId = _userContextService.GetCampusId();

            // Check if School exists
            var school = await _schoolRepository.GetByIdAsync(dto.SchoolId);
            if (school == null)
                throw new KeyNotFoundException("School not found");

            if (campusId == null || campusId != school.CampusId)
                throw new UnauthorizedAccessException("Forbidden");

            // Check if Semester exists
            var semester = await _semesterRepository.GetSemesterByIdAsync(dto.SemesterId);
            if (semester == null)
                throw new KeyNotFoundException("Semester not found");

            // Check if Course exists
            var course = await _courseRepository.GetCourseByIdAsync(dto.CourseId);
            if (course == null)
                throw new KeyNotFoundException("Course not found");
            // Check if Faculty exists
            var faculty = await _userService.GetUserByIdAsync(dto.FacultyId);
            if (faculty == null)
                throw new KeyNotFoundException("Faculty not found");

            var isFacultyFromSameCampus = await _courseSectionRepository.IsFacultyFromSameCampus(dto.FacultyId, campusId.Value);
            if (!isFacultyFromSameCampus)
                throw new InvalidOperationException("Faculty does not belong to the same campus.");

            // Check if CourseSection with same name exists in the same Semester 
            bool exists = await _courseSectionRepository.CourseSectionExistsAsync(dto.CourseSectionName, dto.SemesterId, campusId.Value, dto.Section);
            if (exists)
                throw new InvalidOperationException("CourseSection with the same name already exists in this Semester and Section.");

            var courseSection = new CourseSection
            {
                CourseSectionName = dto.CourseSectionName,
                Status = dto.Status,
                Section = dto.Section,
                Prefix = dto.Prefix,
                CreatedAt = DateTime.UtcNow,
                Notes = dto.Notes,
                FacultyId = dto.FacultyId,
                SemesterId = dto.SemesterId,
                CourseId = dto.CourseId,
                SchoolId = dto.SchoolId
            };

            var createdCourseSection = await _courseSectionRepository.AddCourseSectionAsync(courseSection);
            var courseSectionResponseDto = _mapper.Map<CourseSectionResponseDto>(courseSection);
            return courseSectionResponseDto;
        }

        public async Task<CourseSectionResponseDto> GetCourseSectionByIdAsync(int id)
        {
            var courseSection = await _courseSectionRepository.GetCourseSectionByIdAsync(id);
            if (courseSection == null)
                throw new KeyNotFoundException("CourseSection not found");
            var courseSectionResponseDto = _mapper.Map<CourseSectionResponseDto>(courseSection);
            return courseSectionResponseDto;
        }

        public async Task<IEnumerable<CourseSectionResponseDto>> GetAllCourseSectionsByCampusAsync()
        {
            var campusId = _userContextService.GetCampusId();
            if (campusId == null)
                throw new UnauthorizedAccessException("Forbidden");

            var courseSections = await _courseSectionRepository.GetAllCourseSectionsByCampusAsync(campusId.Value);

            // Map the courseSections list to a list of response DTOs
            var courseSectionResponse = _mapper.Map<IEnumerable<CourseSectionResponseDto>>(courseSections);

            return courseSectionResponse;
        }


        public async Task<CourseSectionResponseDto> UpdateCourseSectionAsync(int id, UpdateCourseSectionDto dto)
        {
            var campusId = _userContextService.GetCampusId();
            if (campusId == null)
                throw new UnauthorizedAccessException("Forbidden");

            // Fetch existing CourseSection
            var existingCourseSection = await _courseSectionRepository.GetCourseSectionByIdAsync(id);
            if (existingCourseSection == null)
                throw new KeyNotFoundException("CourseSection not found");

            // Ensure the CourseSection belongs to the same campus
            var school = await _schoolRepository.GetByIdAsync(existingCourseSection.SchoolId);
            if (school == null || school.CampusId != campusId)
                throw new UnauthorizedAccessException("Forbidden");
            
            var faculty = await _userService.GetUserByIdAsync(dto.FacultyId);
            if (faculty == null)
                throw new KeyNotFoundException("Faculty not found");

            var isFacultyFromSameCampus = await _courseSectionRepository.IsFacultyFromSameCampus(dto.FacultyId, campusId.Value);
            if (!isFacultyFromSameCampus)
                throw new InvalidOperationException("Faculty does not belong to the same campus.");

            bool exists = await _courseSectionRepository.CourseSectionExistsAsync(dto.CourseSectionName, existingCourseSection.SemesterId, campusId.Value, existingCourseSection.Section, existingCourseSection.CourseSectionId);
            if (exists)
                throw new InvalidOperationException("CourseSection with the same name already exists in this Semester and Section.");

            existingCourseSection.FacultyId = dto.FacultyId;
            existingCourseSection.Prefix = dto.Prefix;
            existingCourseSection.Status = dto.Status;
            existingCourseSection.Notes = dto.Notes;
            existingCourseSection.Section = dto.Section;
            existingCourseSection.CourseSectionName = dto.CourseSectionName;
            existingCourseSection.UpdatedAt = DateTime.UtcNow;

            // Save changes
            var updatedCourseSection = await _courseSectionRepository.UpdateCourseSectionAsync(id, existingCourseSection);

            // Map response DTO
            var courseSectionResponseDto = _mapper.Map<CourseSectionResponseDto>(updatedCourseSection);
            return courseSectionResponseDto;
        }
        
        public async Task<bool> DeleteCourseSectionAsync(int id)
        {
            var campusId = _userContextService.GetCampusId();
            if (campusId == null)
                throw new UnauthorizedAccessException("Forbidden");
            var courseSection = await _courseSectionRepository.GetCourseSectionByIdAsync(id);
            if (courseSection == null)
                throw new KeyNotFoundException("CourseSection not found");
            if (courseSection.School.CampusId != campusId)
                throw new UnauthorizedAccessException("Forbidden");
            return await _courseSectionRepository.DeleteCourseSectionAsync(id);
        }

        public async Task<bool> SoftDeleteCourseSectionAsync(int id)
        {
            var campusId = _userContextService.GetCampusId();
            if (campusId == null)
                throw new UnauthorizedAccessException("Forbidden");
            var courseSection = await _courseSectionRepository.GetCourseSectionByIdAsync(id);
            if (courseSection == null)
                throw new KeyNotFoundException("CourseSection not found");
            if (courseSection.School.CampusId != campusId)
                throw new UnauthorizedAccessException("Forbidden");
            return await _courseSectionRepository.SoftDeleteCourseSectionAsync(id);
        }

        public async Task<IEnumerable<CourseSectionResponseDto>> BulkCreateCourseSectionsAsync(BulkCreateCourseSectionDto dto)
        {
            var campusId = _userContextService.GetCampusId();
            if (campusId == null)
                throw new UnauthorizedAccessException("Forbidden");

            // Validate School
            var school = await _schoolRepository.GetByIdAsync(dto.SchoolId);
            if (school == null || school.CampusId != campusId)
                throw new UnauthorizedAccessException("Invalid School or Unauthorized");

            // Validate Semester
            var semester = await _semesterRepository.GetSemesterByIdAsync(dto.SemesterId);
            if (semester == null)
                throw new KeyNotFoundException("Semester not found");

            var createdCourseSections = new List<CourseSection>();

            using var transaction = await _courseSectionRepository.BeginTransactionAsync();
            try
            {
                foreach (var item in dto.CourseSections)
                {
                    // Validate Course
                    var course = await _courseRepository.GetCourseByIdAsync(item.CourseId);
                    if (course == null)
                        throw new KeyNotFoundException($"Course with ID {item.CourseId} not found");

                    // Validate Faculty
                    var faculty = await _userService.GetUserByIdAsync(item.FacultyId);
                    if (faculty == null)
                        throw new KeyNotFoundException($"Faculty with ID {item.FacultyId} not found");

                    var isFacultyFromSameCampus = await _courseSectionRepository.IsFacultyFromSameCampus(item.FacultyId, campusId.Value);
                    if (!isFacultyFromSameCampus)
                        throw new InvalidOperationException($"Faculty {item.FacultyId} does not belong to the same campus.");

                    // Check for duplicates
                    bool exists = await _courseSectionRepository.CourseSectionExistsAsync(item.CourseSectionName, dto.SemesterId, campusId.Value, item.Section);
                    if (exists)
                        throw new InvalidOperationException($"CourseSection '{item.CourseSectionName}' already exists in this Semester and Section.");

                    var courseSection = new CourseSection
                    {
                        CourseSectionName = item.CourseSectionName,
                        Status = item.Status,
                        Section = item.Section,
                        Prefix = dto.Prefix,
                        Notes = item.Notes,
                        CreatedAt = DateTime.UtcNow,
                        FacultyId = item.FacultyId,
                        SemesterId = dto.SemesterId,
                        CourseId = item.CourseId,
                        SchoolId = dto.SchoolId
                    };

                    createdCourseSections.Add(courseSection);
                }

                await _courseSectionRepository.BulkAddCourseSectionsAsync(createdCourseSections);
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            return _mapper.Map<IEnumerable<CourseSectionResponseDto>>(createdCourseSections);
        }



    }

}
