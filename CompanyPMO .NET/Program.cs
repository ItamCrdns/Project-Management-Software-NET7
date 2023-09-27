using CloudinaryDotNet;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Repository;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICompany, CompanyRepository>();
builder.Services.AddScoped<IEmployee, EmployeeRepository>();
builder.Services.AddScoped<IImage, ImageRepository>();
builder.Services.AddScoped<IProject, ProjectRepository>();
builder.Services.AddScoped<ITask, TaskRepository>();
builder.Services.AddScoped<IUserIdentity, UserIdentityRepository>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    });

// Claim based authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SupervisorOnly", policy => policy.RequireClaim(ClaimTypes.Role, "supervisor"));
    options.AddPolicy("EmployeesAllowed", policy => policy.RequireClaim(ClaimTypes.Role, "employee", "supervisor"));

    //var userIdClaimValue = "";
    //var userIdClaim = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);

    //if(userIdClaim is not null)
    //{
    //    userIdClaimValue = userIdClaim.Value;
    //}

    //options.
});

// Cloudinary
DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
Cloudinary cloduinary = new(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
cloduinary.Api.Secure = true;

builder.Services.AddSingleton(cloduinary);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
