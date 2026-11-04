using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using EmploymentPortal.Data;
using EmploymentPortal.Models;
using EmploymentPortal.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Employment & Recruitment Management Portal API",
        Version = "v1",
        Description = "Production ASP.NET Core Channel API matching SRS specification.",
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Format: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                }
            },
            Array.Empty<string>()
        }
    });
});

// 2. Database
builder.Services.AddDbContext<PortalDbContext>(options =>
    options.UseInMemoryDatabase("EmploymentPortalDb"));

// 3. DI Services & HttpClient
builder.Services.AddHttpClient<IVendorService, VendorService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// 4. JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"] ?? "SuperSecretKeyFallback1234567890!SafeJWTKeyForPOC";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "EmploymentPortal",
        ValidAudience = jwtSettings["Audience"] ?? "EmploymentPortalUsers",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
    };
});

builder.Services.AddAuthorization();

// 5. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// 6. Seed Sample Data on Startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PortalDbContext>();
    if (!db.Jobs.Any())
    {
        var sampleJob = new Job
        {
            Title = "Senior Full-Stack Engineer",
            Description = "Lead architecture for enterprise web portals.",
            Requirements = ".NET 8, React, Next.js, SQLite / PostgreSQL",
            SalaryRange = "$120,000 - $160,000",
            Location = "Remote",
            Status = "ACTIVE",
        };
        db.Jobs.Add(sampleJob);

        var sampleCandidate = new Candidate
        {
            FullName = "Taha Hussain",
            Email = "taha.candidate@example.com",
            PhoneNumber = "+1-555-0199",
            Qualification = "BS Computer Science",
            ExperienceYears = 5,
            VerificationStatus = "VERIFIED",
        };
        db.Candidates.Add(sampleCandidate);

        db.SaveChanges();
    }
}

app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();