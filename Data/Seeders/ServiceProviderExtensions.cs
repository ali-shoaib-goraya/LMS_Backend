using LMS.Data;
using Microsoft.AspNetCore.Identity; 
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Data.Seeders 
{ 
    public static class ServiceProviderExtensions
    {
        public static async Task SeedPermissions(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure the database is created
            await context.Database.EnsureCreatedAsync();

            // Check and add predefined permissions
            foreach (var permission in PredefinedPermissions.AllPermissions)
            {
                if (!context.Permissions.Any(p => p.Name == permission.Name))
                {
                    context.Permissions.Add(permission);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
