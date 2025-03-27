using LMS.Data;
using LMS.Features.CourseSectionManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LMS.Features.CourseSectionManagement.Repositories
{
    public class CourseSectionRepository: ICourseSectionRepository
    {   
        private readonly ApplicationDbContext _context;
        public CourseSectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsFacultyFromSameCampus(string facultyId, int campusId)
        {
            return await _context.FacultiesCampuses.AnyAsync(fc => fc.FacultyId == facultyId && fc.CampusId == campusId);
        }
         
        public async Task<bool> CourseSectionExistsAsync(string courseSectionName, int semesterId, int campusId, string section, int? excludeId = null)
        {
            var courseSections = await _context.CourseSections
                .Where(cs => cs.SemesterId == semesterId &&
                 cs.School != null && cs.School.CampusId == campusId &&
                 (excludeId == null || cs.CourseSectionId != excludeId))
                .ToListAsync(); // Fetch data first

            return courseSections.Any(cs =>
                cs.CourseSectionName.Equals(courseSectionName, StringComparison.OrdinalIgnoreCase) &&
                (cs.Section == section ||
                 (cs.Section != null && cs.Section.Equals(section, StringComparison.OrdinalIgnoreCase))));
        }


        public async Task<CourseSection?> AddCourseSectionAsync(CourseSection courseSection)
        {
            _context.CourseSections.Add(courseSection);
            await _context.SaveChangesAsync();
            return courseSection;
        } 

        public async Task<CourseSection?> GetCourseSectionByIdAsync(int id)
        {
            return await _context.CourseSections.Include(cs => cs.School).Include(cs => cs.Semester).Include(cs => cs.Course).Include(cs => cs.Faculty).FirstOrDefaultAsync(cs => cs.CourseSectionId == id);
        }
        public async Task<IEnumerable<CourseSection>> GetAllCourseSectionsByCampusAsync(int campusId)
        {
            return await _context.CourseSections.Include(cs => cs.School).Include(cs => cs.Semester).Include(cs => cs.Course).Include(cs => cs.Faculty).Where(cs => cs.School.CampusId == campusId).ToListAsync();
          
        }

        public async Task<CourseSection?> UpdateCourseSectionAsync(int id, CourseSection courseSection)
        {
            var existingCourseSection = await _context.CourseSections.FirstOrDefaultAsync(cs => cs.CourseSectionId == id);
            if (existingCourseSection == null)
                return null;
            existingCourseSection.CourseSectionName = courseSection.CourseSectionName;
            existingCourseSection.Status = courseSection.Status;
            existingCourseSection.Section = courseSection.Section;
            existingCourseSection.Prefix = courseSection.Prefix;
            existingCourseSection.Notes = courseSection.Notes;
            existingCourseSection.UpdatedAt = DateTime.UtcNow;
            existingCourseSection.FacultyId = courseSection.FacultyId;
            await _context.SaveChangesAsync();
            return existingCourseSection;
        }

        public async Task<bool> DeleteCourseSectionAsync(int id)
        {
            var courseSection = await _context.CourseSections.FirstOrDefaultAsync(cs => cs.CourseSectionId == id);
            if (courseSection == null)
                return false;
            _context.CourseSections.Remove(courseSection);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteCourseSectionAsync(int id)
        {
            var courseSection = await _context.CourseSections.FirstOrDefaultAsync(cs => cs.CourseSectionId == id);
            if (courseSection == null)
                return false;
            courseSection.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task BulkAddCourseSectionsAsync(List<CourseSection> courseSections)
        {
            _context.CourseSections.AddRange(courseSections);
            await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    } 
}

