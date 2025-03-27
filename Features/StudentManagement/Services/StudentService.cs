using LMS.Features.Common.Dtos;
using LMS.Features.StudentManagement.Models;
using LMS.Features.StudentManagement.Dtos;
using LMS.Features.StudentManagement.Repositories;
using LMS.Features.UserManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using LMS.Data;
using System.Formats.Asn1;
using LMS.Features.Common.Services;

namespace LMS.Features.StudentManagement.Services
{
    public class StudentService : IStudentService
    {
        //private readonly IStudentRepository _studentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserContextService _userContextService;

        public StudentService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IUserContextService userContextService)
        {
            //_studentRepository = studentRepository;
            _userManager = userManager;
            _dbContext = dbContext;
            _userContextService = userContextService;
        }


        public async Task<ApiResponseDto> EnrollStudentAsync(StudentEnrollmentDto dto)
        {
            var usercampusId = _userContextService.GetCampusId();
            if (!usercampusId.HasValue)
            {
                return new ApiResponseDto(400, "Forbidden");
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var existingStudent = await _userManager.Users
                    .OfType<Student>()
                    .AnyAsync(s => s.Email == dto.Email || s.EnrollmentNo == dto.EnrollmentNo);

                if (existingStudent)
                {
                    return new ApiResponseDto(400, "A student with this email or enrollment number already exists.");
                }


                var student = new Student
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    EnrollmentNo = dto.EnrollmentNo,
                    GuardianName = dto.GuardianName,
                    GuardianContact = dto.GuardianContact,
                    Type = UserType.Student.ToString(),
                    CreatedAt = DateTime.UtcNow,
                    ProgramBatchSectionId = dto.BatchSectionId,
                    CampusId = usercampusId.Value
                };

                var result = await _userManager.CreateAsync(student, dto.Password);
                if (!result.Succeeded)
                {
                    return new ApiResponseDto(400, "Failed to create student: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                await transaction.CommitAsync();

                // ✅ Convert Student to StudentResponseDto before returning
                var studentResponse = new StudentResponseDto
                {
                    Id = student.Id,
                    EnrollmentNo = student.EnrollmentNo,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    GuardianName = student.GuardianName,
                    GuardianContact = student.GuardianContact,
                    CreatedAt = student.CreatedAt,
                    UpdatedAt = student.UpdatedAt, 
                    SectionId = student.ProgramBatchSectionId,
                    Email = student.Email
                };

                return new ApiResponseDto(201, "Student enrolled successfully.", studentResponse);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiResponseDto(500, "An error occurred: " + ex.Message);
            }
        }

        public async Task<StudentResponseDto> GetStudentByIdAsync(string studentId)
        {
            var userCampusId = _userContextService.GetCampusId();
            var student = await _dbContext.Students
                .Where(s => s.Id == studentId && s.CampusId == userCampusId)
                .Select(s => new StudentResponseDto
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName ?? string.Empty,
                    Email = s.Email ?? string.Empty,
                    EnrollmentNo = s.EnrollmentNo,
                    GuardianName = s.GuardianName,
                    GuardianContact = s.GuardianContact,
                    SectionId = s.ProgramBatchSectionId
                })
                .FirstOrDefaultAsync();

            if (student == null)
                throw new Exception("Student not found.");

            return student;
        }

        public async Task<List<StudentResponseDto>> GetAllStudentsByCampusAsync()
        {
            var campusId = _userContextService.GetCampusId();
            return await _dbContext.Students
                .Where(s => s.CampusId == campusId)
                .Select(s => new StudentResponseDto
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName ?? string.Empty,
                    Email = s.Email ?? string.Empty,
                    EnrollmentNo = s.EnrollmentNo,
                    GuardianName = s.GuardianName,
                    GuardianContact = s.GuardianContact,
                    SectionId = s.ProgramBatchSectionId
                })
                .ToListAsync();
        }

        public async Task<ApiResponseDto> DeleteStudentAsync(string studentId)
        {
            var student = await _userManager.Users.OfType<Student>().FirstOrDefaultAsync(s => s.Id == studentId);
            if (student == null)
                return new ApiResponseDto(404, "Student not found.");

            var userCampusId = _userContextService.GetCampusId();
            if (student.CampusId != userCampusId)
            {
                return new ApiResponseDto(403, "Forbidden");
            }

            // Delete using UserManager to remove from AspNetUsers and Students table
            var result = await _userManager.DeleteAsync(student);
            if (!result.Succeeded)
            {
                return new ApiResponseDto(400, "Failed to delete student.", result.Errors);
            }

            return new ApiResponseDto(200, "Student deleted successfully.");
        }


        public async Task<ApiResponseDto> BulkEnrollStudentsAsync(IFormFile file, int batchSectionId)
        {
            if (file == null || file.Length == 0)
                return new ApiResponseDto(400, "CSV file is required.");

            var studentsToEnroll = new List<Student>();
            var validationErrors = new List<ValidationResult>();
            var failedRecords = new List<object>();

            var usercampusId = _userContextService.GetCampusId();
            if (!usercampusId.HasValue)
            {
                return new ApiResponseDto(400, "Forbidden");
            }

            try
            {
                using (var stream = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
                using (var csv = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture) { HeaderValidated = null, MissingFieldFound = null }))
                {
                    var records = csv.GetRecords<BulkStudentUploadDto>().ToList();
                    int lineNumber = 1; // Track line number for error reporting

                    foreach (var record in records)
                    {
                        lineNumber++;
                        var recordErrors = ValidateRecord(record);

                        // If validation errors exist, store them
                        if (recordErrors.Any())
                        {
                            validationErrors.Add(new ValidationResult(lineNumber, record, recordErrors));
                            continue;
                        }

                        // Check if student already exists
                        var existingStudent = await _userManager.Users
                            .OfType<Student>() // 👈 Ensures we're working only with students
                            .AnyAsync(s => s.Email == record.Email || s.EnrollmentNo == record.EnrollmentNo);

                        if (existingStudent)
                        {
                            validationErrors.Add(new ValidationResult(lineNumber, record, new List<string> { "Student already exists." }));
                            continue;
                        }

                        

                        // Prepare student entity
                        var newStudent = new Student
                        {
                            UserName = record.Email,
                            Email = record.Email,
                            FirstName = record.FirstName,
                            LastName = record.LastName,
                            EnrollmentNo = record.EnrollmentNo,
                            GuardianName = record.GuardianName,
                            GuardianContact = record.GuardianContact,
                            Type = UserType.Student.ToString(),
                            ProgramBatchSectionId = batchSectionId,
                            CampusId = usercampusId.Value,
                            CreatedAt = DateTime.UtcNow
                        }; 

                        studentsToEnroll.Add(newStudent);
                    }
                }

                // If any validation errors exist, return them
                if (validationErrors.Any())
                {
                    return new ApiResponseDto(400, "Bulk enrollment failed due to validation errors.", new
                    {
                        TotalRecords = studentsToEnroll.Count + validationErrors.Count,
                        SuccessfulRecords = studentsToEnroll.Count,
                        FailedRecords = validationErrors
                    });
                }

                // Start Transaction
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        foreach (var student in studentsToEnroll)
                        {
                            var result = await _userManager.CreateAsync(student, "Default@123"); // Assign a default password
                            if (!result.Succeeded)
                            {
                                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                                throw new Exception($"Failed to create student: {student.Email}. Errors: {errors}");
                            }
                            // Ensure the student role is assigned
                            await _userManager.AddToRoleAsync(student, UserType.Student.ToString());
                        }

                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return new ApiResponseDto(400, $"Bulk enrollment failed: {ex.Message}");
                    }
                }

                return new ApiResponseDto(200, "Bulk enrollment completed successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponseDto(400, $"Error processing CSV file: {ex.Message}");
            }
        }

        private List<string> ValidateRecord(BulkStudentUploadDto record)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(record.FirstName))
                errors.Add("First Name is required.");

            if (string.IsNullOrWhiteSpace(record.Email))
                errors.Add("Email is required.");
            else if (!record.Email.Contains("@")) // Basic email validation
                errors.Add("Invalid Email format.");

            if (string.IsNullOrWhiteSpace(record.EnrollmentNo))
                errors.Add("Enrollment Number is required.");

            if (string.IsNullOrWhiteSpace(record.GuardianName))
                errors.Add("Guardian Name is required.");

            if (string.IsNullOrWhiteSpace(record.GuardianContact))
                errors.Add("Guardian Contact is required.");
            else if (!record.GuardianContact.All(char.IsDigit) || record.GuardianContact.Length < 10)
                errors.Add("Invalid Guardian Contact format.");

            return errors;
        }
    }

    public class ValidationResult
    {
        public int Line { get; set; }
        public object Record { get; set; }
        public List<string> Errors { get; set; }

        public ValidationResult(int line, object record, List<string> errors)
        {
            Line = line;
            Record = record;
            Errors = errors;
        }
    }
}


