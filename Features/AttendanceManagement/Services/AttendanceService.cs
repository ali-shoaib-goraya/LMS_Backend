using AutoMapper;
using LMS.Data;
using LMS.Features.AttendanceManagement.Dtos;
using LMS.Features.AttendanceManagement.Models;
using LMS.Features.Common.Services;
using Microsoft.EntityFrameworkCore;
namespace LMS.Features.AttendanceManagement.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public AttendanceService(ApplicationDbContext context, IMapper mapper, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        // 1. Mark Attendance
        public async Task<int> MarkAttendanceAsync(MarkAttendanceDto markAttendanceDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create new ClassSession
                var classSession = new ClassSession
                {
                    CourseSectionId = markAttendanceDto.CourseSectionId,
                    SessionDate = markAttendanceDto.SessionDate,
                    Duration = markAttendanceDto.Duration,
                    LectureName = markAttendanceDto.LectureName,
                    Venue = markAttendanceDto.Venue,
                    Type = markAttendanceDto.Type,
                    Topic = markAttendanceDto.Topic,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.ClassSessions.AddAsync(classSession);
                await _context.SaveChangesAsync();

                var teacherId = _userContextService.GetUserId();
                if (teacherId == null)
                    throw new UnauthorizedAccessException("Teacher not found.");
                // Create Attendance Records
                var attendanceRecords = markAttendanceDto.Students.Select(student => new AttendanceRecord
                {
                    ClassSessionId = classSession.ClassSessionId,
                    StudentId = student.StudentId,
                    IsPresent = student.IsPresent,
                    MarkedByTeacherId = teacherId
                }).ToList();

                await _context.AttendanceRecords.AddRangeAsync(attendanceRecords);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return classSession.ClassSessionId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // 2. Get Attendance by ClassSessionId
        public async Task<AttendanceResponseDto> GetAttendanceBySessionAsync(int classSessionId)
        {
            var classSession = await _context.ClassSessions
                .Include(cs => cs.CourseSection)
                .FirstOrDefaultAsync(cs => cs.ClassSessionId == classSessionId);

            if (classSession == null)
                throw new KeyNotFoundException("Class session not found.");

            var attendanceRecords = await _context.AttendanceRecords
                .Where(a => a.ClassSessionId == classSessionId)
                .Include(a => a.Student)
                .ToListAsync();

            var response = new AttendanceResponseDto
            {
                ClassSessionId = classSession.ClassSessionId,
                SessionDate = classSession.SessionDate,
                Duration = classSession.Duration,
                LectureName = classSession.LectureName,
                Venue = classSession.Venue,
                Type = classSession.Type,
                Topic = classSession.Topic,
                AttendanceRecords = attendanceRecords.Select(a => new StudentAttendanceDto
                {
                    StudentId = a.StudentId,
                    IsPresent = a.IsPresent
                }).ToList()
            };

            return response; 
        }

        // 3. Update Attendance for a ClassSession
        public async Task<bool> UpdateAttendanceAsync(UpdateAttendanceDto updateAttendanceDto)
        {
            var classSession = await _context.ClassSessions
                .FirstOrDefaultAsync(cs => cs.ClassSessionId == updateAttendanceDto.ClassSessionId);

            if (classSession == null)
                throw new KeyNotFoundException("Class session not found.");

            var attendanceRecords = await _context.AttendanceRecords
                .Where(a => a.ClassSessionId == updateAttendanceDto.ClassSessionId)
                .ToListAsync();

            foreach (var record in updateAttendanceDto.UpdatedAttendance)
            {
                var existingRecord = attendanceRecords.FirstOrDefault(a => a.StudentId == record.StudentId);
                if (existingRecord != null)
                    existingRecord.IsPresent = record.IsPresent;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // 4. Delete Attendance Records for a ClassSession
        public async Task<bool> DeleteAttendanceBySessionAsync(int classSessionId)
        {
            var classSession = await _context.ClassSessions
                .FirstOrDefaultAsync(cs => cs.ClassSessionId == classSessionId);

            if (classSession == null)
                throw new KeyNotFoundException("Class session not found.");

            var attendanceRecords = await _context.AttendanceRecords
                .Where(a => a.ClassSessionId == classSessionId)
                .ToListAsync();

            _context.AttendanceRecords.RemoveRange(attendanceRecords);
            _context.ClassSessions.Remove(classSession);

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
