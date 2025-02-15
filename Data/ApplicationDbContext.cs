using Dynamic_RBAMS.Models;
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


// Configure RolePermissionClaim relationship with Permission
            builder.Entity<RolePermissionClaim>()
                .HasOne(rpc => rpc.Permission)
                .WithMany(p => p.RoleClaims)
                .HasForeignKey(rpc => rpc.PermissionId);
            

 // Configure TPT Inheritance
            builder.Entity<Student>().ToTable("Students");
            builder.Entity<Faculty>().ToTable("Faculties");

        // Many to Many Relationship for Faculty and Department
            builder.Entity<DepartmentFaculty>()
                .HasKey(df => df.Id);  // Primary Key is now the surrogate key
            builder.Entity<DepartmentFaculty>()
            .HasIndex(df => new { df.DepartmentId, df.FacultyId })
            .IsUnique();
            builder.Entity<DepartmentFaculty>() 
                .HasOne(df => df.Faculty)
                .WithMany(f => f.DepartmentFaculties)
                .HasForeignKey(df => df.FacultyId)
                .OnDelete(DeleteBehavior.Cascade);  // Keep cascade here if needed
            builder.Entity<DepartmentFaculty>()
                .HasOne(df => df.Department)
                .WithMany(d => d.DepartmentFaculties) 
                .HasForeignKey(df => df.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        // Many to Many relationship for Faculty and Campus 
            builder.Entity<FacultyCampus>()
                    .HasKey(df => df.Id);
            builder.Entity<FacultyCampus>()
                .HasIndex(df => new { df.FacultyId, df.CampusId })
                .IsUnique();
            builder.Entity<FacultyCampus>()
                .HasOne(df => df.Campus)
                .WithMany(f => f.FacultyCampuses)
                .HasForeignKey(df => df.CampusId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<FacultyCampus>()
                .HasOne(df => df.Faculty)
                .WithMany(d => d.FacultyCampuses)
                .HasForeignKey(df => df.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);


            // Lab and Course Relationship is defiend here 
            builder.Entity<Course>()
                .HasOne(c => c.ConnectedCourse)              
                .WithOne()                                    
                .HasForeignKey<Course>(c => c.ConnectedCourseId)  
                .OnDelete(DeleteBehavior.Restrict);

            // Course Section Foriegn Key Constraints are defined here
            builder.Entity<CourseSection>() 
                .HasOne(cs => cs.ProgramBatchSection)
                .WithMany()
                .HasForeignKey(cs => cs.SectionId)
                .OnDelete(DeleteBehavior.Restrict);  // Change CASCADE to RESTRICT

            builder.Entity<CourseSection>()
                .HasOne(cs => cs.Course)
                .WithMany()
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.Restrict);  // Change CASCADE to RESTRICT

            builder.Entity<CourseSection>()
                .HasOne(cs => cs.Faculty)
                .WithMany()
                .HasForeignKey(cs => cs.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);  // Change CASCADE to RESTRICT

            builder.Entity<CourseSection>()
                .HasOne(cs => cs.Semester)
                .WithMany()
                .HasForeignKey(cs => cs.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);  // Change CASCADE to RESTRICT

            // University → Campuses (Restrict deletion if campuses exist)
            builder.Entity<Campus>()
                .HasOne(c => c.University)
                .WithMany(u => u.Campuses)
                .HasForeignKey(c => c.UniversityId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent deletion if campuses exist

            // Campus → Schools (Restrict deletion if schools exist)
            builder.Entity<School>()
                .HasOne(s => s.Campus)
                .WithMany(c => c.Schools)
                .HasForeignKey(s => s.CampusId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent deletion if schools exist

            // School → Departments (Restrict deletion if departments exist)
            builder.Entity<Department>()
                .HasOne(d => d.School)
                .WithMany(s => s.Departments)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent deletion if departments exist
        }
    }
}
    

