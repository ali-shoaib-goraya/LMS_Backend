using LMS.Data;
using LMS.Features.ActivityManagement.Helper;
using LMS.Features.ActivityManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Features.ActivityManagement.Services
{
    public class ActivityService
    {
        private readonly ApplicationDbContext _context;

        public ActivityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CalculateAndStoreFinalGrade(string studentId, int courseSectionId)
        {
            var activities = await _context.Activities
                .Where(a => a.CourseSectionId == courseSectionId)
                .ToListAsync();

            var studentActivities = await _context.StudentActivities
                .Where(sa => sa.StudentId == studentId && activities.Select(a => a.ActivityId).Contains(sa.ActivityId))
                .ToListAsync();

            decimal totalWeightedScore = 0;
            decimal totalWeight = 0;
             
            foreach (var activity in activities)
            {
                var studentActivity = studentActivities.FirstOrDefault(sa => sa.ActivityId == activity.ActivityId);
                if (studentActivity != null)
                {
                    decimal activityPercentage = (studentActivity.ObtainedMarks / activity.TotalMarks) * 100;
                    decimal weightedScore = (activityPercentage * activity.Weightage) / 100;
                    totalWeightedScore += weightedScore;
                    totalWeight += activity.Weightage;
                }
            }

            if (totalWeight == 0) return; // Avoid division by zero

            decimal finalPercentage = (totalWeightedScore / totalWeight) * 100;
            var (letterGrade, gpa) = GradingHelper.CalculateGPA(finalPercentage);

            var studentGrade = await _context.StudentGrades
                .FirstOrDefaultAsync(sg => sg.StudentId == studentId && sg.CourseSectionId == courseSectionId);

            if (studentGrade == null)
            {
                studentGrade = new StudentGrade
                {
                    StudentId = studentId,
                    CourseSectionId = courseSectionId,
                    FinalPercentage = finalPercentage,
                    LetterGrade = letterGrade,
                    GPA = gpa
                };
                _context.StudentGrades.Add(studentGrade);
            }
            else
            {
                studentGrade.FinalPercentage = finalPercentage;
                studentGrade.LetterGrade = letterGrade;
                studentGrade.GPA = gpa;
                _context.StudentGrades.Update(studentGrade);
            }

            await _context.SaveChangesAsync();
        }









    }
}
