using CloudinaryDotNet;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Interfaces.Company_interfaces;
using CompanyPMO_.NET.Interfaces.Employee_interfaces;
using CompanyPMO_.NET.Interfaces.Issue_interfaces;
using CompanyPMO_.NET.Interfaces.Project_interfaces;
using CompanyPMO_.NET.Interfaces.Task_interfaces;
using CompanyPMO_.NET.Repository;
using CompanyPMO_.NET.Services;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICompany, CompanyRepository>();
builder.Services.AddScoped<ICompanyManagement, CompanyRepository>();

builder.Services.AddScoped<IEmployeeAuthentication, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeCompanyQueries, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeManagement, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeProjectQueries, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeQueries, EmployeeRepository>();

builder.Services.AddScoped<IIssue, IssueRepository>();
builder.Services.AddScoped<IIssueEmployeeQueries, IssueRepository>();
builder.Services.AddScoped<IIssueManagement, IssueRepository>();
builder.Services.AddScoped<IIssueTaskQueries, IssueRepository>();

builder.Services.AddScoped<IProjectCompanyQueries, ProjectRepository>();
builder.Services.AddScoped<IProjectEmployeeQueries, ProjectRepository>();
builder.Services.AddScoped<IProjectManagement, ProjectRepository>();
builder.Services.AddScoped<IProjectQueries, ProjectRepository>();

builder.Services.AddScoped<ITask, TaskRepository>();
builder.Services.AddScoped<ITaskEmployeeQueries, TaskRepository>();
builder.Services.AddScoped<ITaskProjectQueries, TaskRepository>();
builder.Services.AddScoped<ITaskManagement, TaskRepository>();

builder.Services.AddScoped<IJwt, JwtService>();

builder.Services.AddScoped<ILatestStuff, LatestStuffRepository>();

builder.Services.AddScoped<IResetPasswordRequest, ResetPasswordRequestRepository>();

builder.Services.AddScoped<IWorkload, WorkloadRepository>();
builder.Services.AddScoped<ITimeline, TimelineRepository>();

builder.Services.AddScoped<IImage, ImageService>();
builder.Services.AddScoped<IUtility, UtilityService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();

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

// Postgres
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
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
    options.AddPolicy("SupervisorOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Supervisor"));
    options.AddPolicy("EmployeesAllowed", policy => policy.RequireClaim(ClaimTypes.Role, "Employee", "Intern", "Trainee", "Supervisor"));
    options.AddPolicy("OnlyUser", policy =>
    {
        policy.RequireClaim(ClaimTypes.NameIdentifier);
        policy.RequireAssertion(context =>
        {
            var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

            if(idClaim is not null)
            {
                var idFromUrl = context.Resource as DefaultHttpContext;
                var employeeId = idFromUrl.Request.RouteValues["employeeId"].ToString();
                
                if(!string.IsNullOrEmpty(employeeId) && idClaim.Value.Equals(employeeId) || context.User.HasClaim(ClaimTypes.Role, "Supervisor"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        });
    });
});

// Cloudinary
DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
Cloudinary cloduinary = new(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
cloduinary.Api.Secure = true;

builder.Services.AddSingleton(cloduinary);

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seed")
{
    Seed.SeedData(app);
}

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
