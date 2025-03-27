using LMS.Data;
using Microsoft.EntityFrameworkCore;


namespace LMS.Features.CourseManagement.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Course?> AddCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course?> UpdateCourseAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course?> GetCourseByIdAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.ConnectedCourse)
                .Include(c => c.Department)
                .ThenInclude(d => d.School)
                .ThenInclude(s => s.Campus)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public async Task<IEnumerable<Course>> GetCoursesByDepartmentAsync(int departmentId)
        {
            return await _context.Courses
                .Where(c => c.DepartmentId == departmentId)
                .Include(c => c.ConnectedCourse)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByCampusAsync(int campusId)
        {
            return await _context.Courses
                .Include(c => c.Department)
                .ThenInclude(d => d.School)
                .ThenInclude(s => s.Campus)
                .Where(c => c.Department.School.CampusId == campusId && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> CourseExistsAsync(int campusId, string courseName, string courseCode)
        {
            return await _context.Courses
                .Include(c => c.Department)
                .ThenInclude(d => d.School)
                .ThenInclude(s => s.Campus)
                .AnyAsync(c => c.Department.School.CampusId == campusId &&
                               (c.CourseName == courseName || c.CourseCode == courseCode));
        }

        public async Task<bool> SoftDeleteCourseAsync(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) return false;

            course.IsDeleted = true;  // Assuming IsDeleted is a property for soft deletion
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HardDeleteCourseAsync(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
