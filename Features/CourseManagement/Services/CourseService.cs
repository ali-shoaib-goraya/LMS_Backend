using AutoMapper;
using LMS.Features.CampusManagement.Repositories;
using LMS.Features.Common.Services;
using LMS.Features.CourseManagement.Dtos;
using LMS.Features.CourseManagement.Repositories;
using LMS.Features.DepartmentManagement.Repositories;
using LMS.Features.DepartmentManagement;

namespace LMS.Features.CourseManagement.Services
{
    public class CourseService: ICourseService
    {
        private readonly IMapper _mapper;
        private readonly ICourseRepository _courseRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ICampusRepository _campusRepository;
        private readonly ICampusEntityAuthorizationService _campusAuthorizationService;

        public CourseService(
            IMapper mapper,
            ICourseRepository courseRepository,
            IDepartmentRepository departmentRepository,
            ICampusRepository campusRepository,
            ICampusEntityAuthorizationService campusAuthorizationService)
        {
            _mapper = mapper;
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
            _campusRepository = campusRepository;
            _campusAuthorizationService = campusAuthorizationService;
        }

        public async Task<CourseResponseDto?> GetCourseByIdAsync(int courseId)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null) return null;

            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Course>(courseId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to access this course.");

            return _mapper.Map<CourseResponseDto>(course);
        }

        public async Task<IEnumerable<CourseResponseDto>> GetCoursesByDepartmentAsync(int departmentId)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(departmentId);
            if (department == null) return new List<CourseResponseDto>();

            var courses = await _courseRepository.GetCoursesByDepartmentAsync(departmentId);
            return _mapper.Map<IEnumerable<CourseResponseDto>>(courses);
        }

        public async Task<IEnumerable<CourseResponseDto>> GetCoursesByCampusAsync(int campusId)
        {
            var campus = await _campusRepository.GetByIdAsync(campusId);
            if (campus == null) return new List<CourseResponseDto>();

            var courses = await _courseRepository.GetCoursesByCampusAsync(campusId);
            return _mapper.Map<IEnumerable<CourseResponseDto>>(courses);
        }

        public async Task<CourseResponseDto> CreateCourseAsync(AddCourseDto createCourseDto)
        {
            if (createCourseDto.DepartmentId <= 0)
                throw new ArgumentException("Invalid DepartmentId provided.");

            var department = await _departmentRepository.GetDepartmentByIdAsync(createCourseDto.DepartmentId);
            if (department == null)
                throw new KeyNotFoundException("Department not found.");

            Course? connectedCourse = null;
            if (createCourseDto.ConnectedCourseId.HasValue)
            {
                if (createCourseDto.ConnectedCourseId.Value <= 0)
                {
                    throw new ArgumentException("Invalid Connected Course ID provided.");
                }

                connectedCourse = await _courseRepository.GetCourseByIdAsync(createCourseDto.ConnectedCourseId.Value);
                if (connectedCourse == null)
                {
                    throw new KeyNotFoundException("Connected Course not found.");
                }
            }

            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Department>(createCourseDto.DepartmentId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to add a course to this department.");

            bool exists = await _courseRepository.CourseExistsAsync(department.School.CampusId, createCourseDto.CourseName, createCourseDto.CourseCode);
            if (exists)
                throw new InvalidOperationException("A course with the same name or code already exists in this campus.");

            var course = _mapper.Map<Course>(createCourseDto);

            // If a connected course is provided, set the relationship
            if (connectedCourse != null)
            {
                course.ConnectedCourseId = connectedCourse.CourseId;
            }

            var createdCourse = await _courseRepository.AddCourseAsync(course);
            return _mapper.Map<CourseResponseDto>(createdCourse);
        }


        public async Task<CourseResponseDto?> UpdateCourseAsync(int courseId, UpdateCourseDto updateCourseDto)
        {
            var existingCourse = await _courseRepository.GetCourseByIdAsync(courseId);
            if (existingCourse == null) return null;

            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Course>(courseId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to update this course.");

            // Check for duplicate CourseName or CourseCode only if they are being updated
            if ((!string.IsNullOrEmpty(updateCourseDto.CourseName) && updateCourseDto.CourseName != existingCourse.CourseName) ||
                (!string.IsNullOrEmpty(updateCourseDto.CourseCode) && updateCourseDto.CourseCode != existingCourse.CourseCode))
            {
                bool exists = await _courseRepository.CourseExistsAsync(
                    existingCourse.Department.School.CampusId,
                    updateCourseDto.CourseName ?? existingCourse.CourseName, // Keep existing if null
                    updateCourseDto.CourseCode ?? existingCourse.CourseCode  // Keep existing if null
                );

                if (exists)
                    throw new InvalidOperationException("A course with the same name or code already exists in this campus.");
            }

            // If a new ConnectedCourseId is provided, validate it
            if (updateCourseDto.ConnectedCourseId.HasValue)
            {
                if (updateCourseDto.ConnectedCourseId.Value <= 0)
                    throw new ArgumentException("Invalid Connected Course ID provided.");

                var connectedCourse = await _courseRepository.GetCourseByIdAsync(updateCourseDto.ConnectedCourseId.Value);
                if (connectedCourse == null)
                    throw new KeyNotFoundException("Connected Course not found.");
            }

            // Update only non-null fields
            existingCourse.CourseName = updateCourseDto.CourseName ?? existingCourse.CourseName;
            existingCourse.CourseCode = updateCourseDto.CourseCode ?? existingCourse.CourseCode;
            existingCourse.CreditHours = updateCourseDto.CreditHours ?? existingCourse.CreditHours;
            existingCourse.IsLab = updateCourseDto.IsLab ?? existingCourse.IsLab;
            existingCourse.IsCompulsory = updateCourseDto.IsCompulsory ?? existingCourse.IsCompulsory;
            existingCourse.IsTheory = updateCourseDto.IsTheory ?? existingCourse.IsTheory;
            existingCourse.ConnectedCourseId = updateCourseDto.ConnectedCourseId ?? existingCourse.ConnectedCourseId;
            existingCourse.Objective = updateCourseDto.Objective ?? existingCourse.Objective;
            existingCourse.Notes = updateCourseDto.Notes ?? existingCourse.Notes;

            var updatedCourse = await _courseRepository.UpdateCourseAsync(existingCourse);
            return _mapper.Map<CourseResponseDto>(updatedCourse);
        }


        public async Task<bool> SoftDeleteCourseAsync(int courseId)
        {
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Course>(courseId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to soft delete this course.");

            return await _courseRepository.SoftDeleteCourseAsync(courseId);
        }

        public async Task<bool> HardDeleteCourseAsync(int courseId)
        {
            bool hasAccess = await _campusAuthorizationService.HasAccessToEntityAsync<Course>(courseId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have permission to delete this course.");

            return await _courseRepository.HardDeleteCourseAsync(courseId);
        }
    }
}
