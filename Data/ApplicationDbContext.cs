using Dynamic_RBAMS.Features.AuthenticationManagment;
using Dynamic_RBAMS.Features.BatchManagment;
using Dynamic_RBAMS.Features.CampusManagement;
using Dynamic_RBAMS.Features.Common.Models;
using Dynamic_RBAMS.Features.CourseManagement;
using Dynamic_RBAMS.Features.DepartmentManagement;
using Dynamic_RBAMS.Features.EnrollmentManagement;
using Dynamic_RBAMS.Features.PermissionsManagement;
using Dynamic_RBAMS.Features.ProgramManagement;
using Dynamic_RBAMS.Features.RoleManagement;
using Dynamic_RBAMS.Features.SchoolManagement;
using Dynamic_RBAMS.Features.SectionManagement;
using Dynamic_RBAMS.Features.SemesterManagement;
using Dynamic_RBAMS.Features.UniveristyManagement;
using Dynamic_RBAMS.Features.UserManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Dynamic_RBAMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, String>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermissionClaim> RolePermissionClaims { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Campus> Campuses { get; set; } 
        public DbSet<DepartmentFaculty> DepartmentFaculties { get; set; }
        public DbSet<School> Schools { get; set; }  
        public DbSet<Department> Departments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<FacultyCampus> FacultiesCampuses { get; set; } 
        public DbSet<Course> Courses { get; set; }
        public DbSet<Programs> Programs { get; set; }  
        public DbSet<ProgramBatch> ProgramsBatches { get; set; } 
        public DbSet<ProgramBatchSection> ProgramsBatchSections { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<CourseSection> CourseSections { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; } 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Override OnModelCreating to define relationships
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Optional: Delete tokens when user is deleted

        // ✅ Role Permissions
    builder.Entity<RolePermissionClaim>()
        .HasOne(rpc => rpc.Permission)
        .WithMany(p => p.RoleClaims)
        .HasForeignKey(rpc => rpc.PermissionId);

    // ✅ TPT Inheritance for Students & Faculty
    builder.Entity<Student>().ToTable("Students");
    builder.Entity<Faculty>().ToTable("Faculties");

    // ✅ Faculty ↔ Department (Many-to-Many, Faculty must NOT be deleted if linked elsewhere)
    builder.Entity<DepartmentFaculty>()
        .HasOne(df => df.Faculty)
        .WithMany(f => f.DepartmentFaculties)
        .HasForeignKey(df => df.FacultyId)
        .OnDelete(DeleteBehavior.Restrict);  // ⛔ Prevent Faculty deletion if referenced elsewhere

    builder.Entity<DepartmentFaculty>()
        .HasOne(df => df.Department)
        .WithMany(d => d.DepartmentFaculties)
        .HasForeignKey(df => df.DepartmentId)
        .OnDelete(DeleteBehavior.Cascade);  // ✅ Delete relation when department is deleted

    // ✅ Faculty ↔ Campus (Many-to-Many)
    builder.Entity<FacultyCampus>()
        .HasKey(df => df.Id);

    builder.Entity<FacultyCampus>()
        .HasIndex(df => new { df.FacultyId, df.CampusId })
        .IsUnique();

    builder.Entity<FacultyCampus>()
        .HasOne(fc => fc.Campus)
        .WithMany(c => c.FacultyCampuses)
        .HasForeignKey(fc => fc.CampusId)
        .OnDelete(DeleteBehavior.Restrict);  // ⛔ Prevent campus deletion if faculty exists

    builder.Entity<FacultyCampus>()
        .HasOne(fc => fc.Faculty)
        .WithMany(f => f.FacultyCampuses)
        .HasForeignKey(fc => fc.FacultyId)
        .OnDelete(DeleteBehavior.Cascade);  // ✅ Remove faculty-campus relation when faculty is deleted

    // ✅ Course Relationships (Restrict deletions)
    builder.Entity<Course>()
        .HasOne(c => c.ConnectedCourse)
        .WithOne()
        .HasForeignKey<Course>(c => c.ConnectedCourseId)
        .OnDelete(DeleteBehavior.Restrict);

    // ✅ CourseSection Constraints
    builder.Entity<CourseSection>()
        .HasOne(cs => cs.ProgramBatchSection)
        .WithMany()
        .HasForeignKey(cs => cs.SectionId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<CourseSection>()
        .HasOne(cs => cs.Course)
        .WithMany()
        .HasForeignKey(cs => cs.CourseId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<CourseSection>()
        .HasOne(cs => cs.Faculty)
        .WithMany()
        .HasForeignKey(cs => cs.FacultyId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<CourseSection>()
        .HasOne(cs => cs.Semester)
        .WithMany()
        .HasForeignKey(cs => cs.SemesterId)
        .OnDelete(DeleteBehavior.Restrict);

    // ✅ University → Campus (Restrict deletion if campuses exist)
    builder.Entity<Campus>()
        .HasOne(c => c.University)
        .WithMany(u => u.Campuses)
        .HasForeignKey(c => c.UniversityId)
        .OnDelete(DeleteBehavior.Restrict);

    // ✅ Campus → School (Restrict deletion if schools exist)
    builder.Entity<School>()
        .HasOne(s => s.Campus)
        .WithMany(c => c.Schools)
        .HasForeignKey(s => s.CampusId)
        .OnDelete(DeleteBehavior.Restrict);

    // ✅ School → Departments (Restrict deletion if departments exist)
    builder.Entity<Department>()
        .HasOne(d => d.School)
        .WithMany(s => s.Departments)
        .HasForeignKey(d => d.SchoolId)
        .OnDelete(DeleteBehavior.Restrict);

    // ✅ Department → Programs (Cascade delete programs when department is deleted)
    builder.Entity<Programs>()
        .HasOne(p => p.Department)
        .WithMany(d => d.Programs)
        .HasForeignKey(p => p.DepartmentId)
        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
    

