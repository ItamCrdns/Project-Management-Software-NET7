using CloudinaryDotNet;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Repository;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICompany, CompanyRepository>();
builder.Services.AddScoped<IEmployee, EmployeeRepository>();
builder.Services.AddScoped<IImage, ImageRepository>();
builder.Services.AddScoped<IProject, ProjectRepository>();
builder.Services.AddScoped<ITask, TaskRepository>();
builder.Services.AddScoped<IUserIdentity, UserIdentityRepository>();
builder.Services.AddScoped<IUtility, UtilityRepository>();
builder.Services.AddScoped<IIssue, IssueRepository>();
builder.Services.AddScoped<IJwt, JwtService>();

// Add services to the container.

// Cors
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins, policy => { policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials(); });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// JWT -- We store the JWT Inside the cookie
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "JwtToken";
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["JwtToken"];
                return Task.CompletedTask;
            }
        };
    });

// Claim based authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SupervisorOnly", policy => policy.RequireClaim(ClaimTypes.Role, "supervisor"));
    options.AddPolicy("EmployeesAllowed", policy => policy.RequireClaim(ClaimTypes.Role, "employee", "supervisor"));
});

// Cloudinary
DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
Cloudinary cloduinary = new(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
cloduinary.Api.Secure = true;

builder.Services.AddSingleton(cloduinary);

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

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
