using AutoMapper;
using LMS.Data;
using LMS.Features.Common.Services;
using LMS.Features.CourseSectionManagement.Dtos;
using LMS.Features.CourseSectionManagement.Models;
using LMS.Features.EnrollmentManagement.Dtos;
using LMS.Features.StudentManagement.Models;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

namespace LMS.Features.EnrollmentManagement.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public EnrollmentService(ApplicationDbContext context, IUserContextService userContextService, IMapper mapper)
        {
            _context = context;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<EnrollmentResponseDto> EnrollStudentAsync(SingleEnrollmentDto enrollmentDto)
        {
            var student = await _context.Students.FindAsync(enrollmentDto.StudentId);
            var courseSection = await _context.CourseSections
                .Include(cs => cs.School)
                .FirstOrDefaultAsync(cs => cs.CourseSectionId == enrollmentDto.CourseSectionId);

            if (student == null) throw new NotFoundException("Student not found");
            if (courseSection == null) throw new NotFoundException("CourseSection not found");

            var adminCampusId = _userContextService.GetCampusId();
            if (student.CampusId != courseSection.School.CampusId)
                throw new BadRequestException("Student and CourseSection must be from the same campus");

            if (student.CampusId != adminCampusId)
                throw new ForbiddenException("You are not allowed to enroll students from other campuses");

            var enrollment = new Enrollment
            {
                StudentId = student.Id,
                CourseSectionId = courseSection.CourseSectionId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return new EnrollmentResponseDto
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseSectionId = enrollment.CourseSectionId,
                Status = enrollment.Status,
                IsApproved = enrollment.IsApproved
            };
        }

        public async Task<bool> RemoveEnrollmentAsync(int enrollmentId)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.CourseSection)
                .ThenInclude(cs => cs.School)
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

            if (enrollment == null)
                return false;

            var adminCampusId = _userContextService.GetCampusId();
            if (enrollment.Student.CampusId != enrollment.CourseSection.School.CampusId)
                throw new BadRequestException("Student and CourseSection must be from the same campus");

            if (enrollment.Student.CampusId != adminCampusId)
                throw new ForbiddenException("You are not allowed to remove enrollments from other campuses");

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<EnrollmentResponseDto>> BulkEnrollStudentsAsync(BulkEnrollmentDto enrollmentDto)
        {
            var adminCampusId = _userContextService.GetCampusId();

            var students = await _context.Students
                .Where(s => enrollmentDto.StudentIds.Contains(s.Id))
                .ToListAsync();

            var courseSection = await _context.CourseSections
                .Include(cs => cs.School)
                .FirstOrDefaultAsync(cs => cs.CourseSectionId == enrollmentDto.CourseSectionId);

            if (courseSection == null)
                throw new NotFoundException("CourseSection not found");

            if (students.Count != enrollmentDto.StudentIds.Count)
                throw new BadRequestException("Some students were not found");

            if (students.Any(s => s.CampusId != adminCampusId))
                throw new ForbiddenException("You are not allowed to enroll students from other campuses");

            if (students.Any(s => s.CampusId != courseSection.School.CampusId))
                throw new BadRequestException("All students must be from the same campus as the CourseSection");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var enrollments = students.Select(student => new Enrollment
                {
                    StudentId = student.Id,
                    CourseSectionId = enrollmentDto.CourseSectionId,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                await _context.Enrollments.AddRangeAsync(enrollments);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return enrollments.Select(e => new EnrollmentResponseDto
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = e.StudentId,
                    CourseSectionId = e.CourseSectionId,
                    Status = e.Status,
                    IsApproved = e.IsApproved
                }).ToList();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new Exception("Bulk enrollment failed, transaction rolled back.");
            }
        }

        public async Task<IEnumerable<CourseSectionResponseDto>> GetStudentCourseSectionsAsync()
        {
            var studentId = _userContextService.GetUserId();

            var courseSectionIds = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Select(e => e.CourseSectionId) 
                .ToListAsync();

     
            var courseSections = await _context.CourseSections
                .Where(cs => courseSectionIds.Contains(cs.CourseSectionId))
                .Include(cs => cs.Course)
                .Include(cs => cs.Semester)
                .Include(cs => cs.Faculty)
                .Include(cs => cs.School)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CourseSectionResponseDto>>(courseSections);
        }


        public async Task<IEnumerable<CourseSectionResponseDto>> GetTeacherCourseSectionsAsync()
        {
            var teacherId = _userContextService.GetUserId();

            var courseSections = await _context.CourseSections
                .Where(cs => cs.FacultyId == teacherId)
                .Include(cs => cs.Course)
                .Include(cs => cs.Semester)
                .Include(cs => cs.Faculty)
                .Include(cs => cs.School)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CourseSectionResponseDto>>(courseSections);
        }

    } 
}
