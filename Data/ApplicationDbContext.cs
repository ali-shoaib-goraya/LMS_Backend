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
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<FacultyCampus> FacultyCampuses { get; set; }
        public DbSet<School> schools { get; set; } 
        public DbSet<Department> Departments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
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
            //builder.Entity<RolePermissionClaim>().ToTable("RolePermissions");



            // Relationships for Student
            builder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict); // Adjust delete behavior as necessary

            builder.Entity<Student>()
                .HasOne(s => s.School)
                .WithMany(sc => sc.Students)
                .HasForeignKey(s => s.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Student>()
                .HasOne(s => s.Campus)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.CampusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationships for Faculty
            builder.Entity<Faculty>()
                .HasOne(f => f.Department)
                .WithMany(d => d.Faculties)
                .HasForeignKey(f => f.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Faculty>()
                .HasOne(f => f.School)
                .WithMany(sc => sc.Faculties)
                .HasForeignKey(f => f.SchoolID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FacultyCampus>()
                .HasKey(fc => new { fc.FacultyId, fc.CampusId }); // Composite key

            builder.Entity<FacultyCampus>()
                .HasOne(fc => fc.Faculty)
                .WithMany(f => f.FacultyCampuses)
                .HasForeignKey(uc => uc.FacultyId);

            builder.Entity<FacultyCampus>()
                .HasOne(fc => fc.Campus)
                .WithMany(c => c.FacultyCampuses)
                .HasForeignKey(fc => fc.CampusId);
        }
    }
}
    

