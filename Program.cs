using Dynamic_RBAMS;
using Dynamic_RBAMS.Data;
using Dynamic_RBAMS.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Dynamic_RBAMS.AutoMapper;
using System.Text;
using Dynamic_RBAMS.Features.RoleManagement;
using Dynamic_RBAMS.Features.Common.Services;
using Dynamic_RBAMS.Features.DepartmentManagement.Repositories;
using Dynamic_RBAMS.Features.DepartmentManagement.Services;
using Dynamic_RBAMS.Features.Common.Repositories;
using Dynamic_RBAMS.Features.UserManagement.Services;
using Dynamic_RBAMS.Features.UserManagement.Models;
using Dynamic_RBAMS.Features.AuthenticationManagement.Services;
using Dynamic_RBAMS.Features.SchoolManagement.Repositories;
using Dynamic_RBAMS.Features.SchoolManagement.Services;
using Dynamic_RBAMS.Features.CampusManagement.Repositories;
using Dynamic_RBAMS.Features.CampusManagement.Services;
using Dynamic_RBAMS.Features.UniveristyManagement.Repositories;
using Dynamic_RBAMS.Features.UniveristyManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

// Add DbContext.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Options support.
builder.Services.AddOptions();

// Add Identity services.
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddRoleManager<RoleManager<ApplicationRole>>() // Use your custom ApplicationRole
    .AddDefaultTokenProviders();

// Register Identity-related services.
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<ApplicationRole>>();
builder.Services.AddScoped<IUserStore<ApplicationUser>, UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, string>>();
builder.Services.AddScoped<IRoleStore<ApplicationRole>, RoleStore<ApplicationRole, ApplicationDbContext, string>>();

// Enable access to the HttpContext globally
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<ICampusEntityAuthorizationService, CampusEntityAuthorizationService>();
builder.Services.AddScoped<IEntityRepository, EntityRepository>();

// Register Services and Repositories.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUniversityService, UniversityService>();
builder.Services.AddScoped<IUniversityRepository, UniversityRepository>();
builder.Services.AddScoped<ICampusService, CampusService>();
builder.Services.AddScoped<ICampusRepository, CampusRepository>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
// ✅ Register AutoMapper first
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

// Register Authorization.
builder.Services.AddAuthorization();

// Add JWT authentication.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
    };
    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
});

// Add CORS policy.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:5174")
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials();
    });
});

// Build the application.
var app = builder.Build();

// Apply Authorization Policies Dynamically from DB using the Options pattern.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Resolve the authorization options via IOptions<AuthorizationOptions>
    var authorizationOptions = scope.ServiceProvider.GetRequiredService<IOptions<Microsoft.AspNetCore.Authorization.AuthorizationOptions>>().Value;

    // Fetch permissions from the database.
    var permissions = context.Permissions.AsNoTracking().ToList();

    // Add policies dynamically based on permissions.
    foreach (var permission in permissions)
    {
        authorizationOptions.AddPolicy(permission.Name, policy =>
            policy.RequireClaim("Permission", permission.Name));
    }
}

// Global Exception Middleware.
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

// Seed predefined permissions.
await app.Services.SeedPermissions();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
