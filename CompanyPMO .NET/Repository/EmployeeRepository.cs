using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyPMO_.NET.Repository
{
    public class EmployeeRepository : IEmployee
    {
        private readonly ApplicationDbContext _context;
        private readonly IImage _imageService;
        private readonly IPatcher _patcherService;

        public EmployeeRepository(ApplicationDbContext context, IImage imageService, IPatcher patcherService)
        {
            _context = context;
            _imageService = imageService;
            _patcherService = patcherService;
        }
        public async Task<(AuthenticationResult result, string message, EmployeeDto employee)> AuthenticateEmployee(string username, string password)
        {
            bool? isAccountLocked = await IsAccountLocked(username);

            var employee = await _context.Employees
                .FirstOrDefaultAsync(u => u.Username.Equals(username));

            if (isAccountLocked is true)
            {
                return (new AuthenticationResult { Blocked = true }, $"Your account has been blocked for {_patcherService.MinutesUntilTimeArrival(employee.LockedUntil)} minutes.", null);
            }

            if(!string.IsNullOrEmpty(password))
            {
                if(employee is null)
                {
                    return (new AuthenticationResult { DoesntExist = true }, "Apparently, this user does not exist.", null);
                }

                if(BCrypt.Net.BCrypt.Verify(password, employee.Password))
                {
                    if(employee.LoginAttempts > 0)
                    {
                        employee.LoginAttempts = 0;
                        _ = await _context.SaveChangesAsync();
                    }

                    var employeeReturn = new EmployeeDto
                    {
                        EmployeeId = employee.EmployeeId,
                        Username = employee.Username,
                        ProfilePicture = employee.ProfilePicture
                    };

                    return (new AuthenticationResult { Authenticated = true }, $"Welcome, {employeeReturn.Username}", employeeReturn);
                }
                else
                {
                    employee.LoginAttempts++;
                    _ = await _context.SaveChangesAsync();

                    return (new AuthenticationResult { WrongCreds = true }, $"Wrong credentials. You have tried {employee.LoginAttempts} times", null);
                }
            }

            return (new AuthenticationResult { SomethingWrong = true }, "Something went wrong.", null);
        }

        public async Task<Employee> GetEmployeeById(int employeeId)
        {
            var employee = await _context.Employees
                .Where(e => e.EmployeeId.Equals(employeeId))
                .Include(p => p.Projects)
                .Include(c => c.Company)
                .Include(t => t.Tier)
                .Include(t => t.Tasks)
                .Include(i => i.Issues)
                .Include(s => s.Supervisor)
                .FirstOrDefaultAsync();

            var employeeDto = new Employee
            {
                EmployeeId = employee.EmployeeId,
                Username = employee.Username,
                Role = employee.Role,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Password = employee.Password,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                Created = employee.Created,
                ProfilePicture = employee.ProfilePicture,
                LastLogin = employee.LastLogin,
                LockedEnabled = employee.LockedEnabled,
                LoginAttempts = employee.LoginAttempts,
                LockedUntil = employee.LockedUntil,
                Tier = employee.Tier,
                Company = employee.Company,
                SupervisorId = employee.SupervisorId,
                Supervisor = new Employee
                {
                    EmployeeId = employee.Supervisor.EmployeeId,
                    Username = employee.Supervisor.Username,
                    ProfilePicture = employee.Supervisor.ProfilePicture,
                },
                Projects = employee.Projects.Select(project => new Project
                {
                    ProjectId = project.ProjectId,
                    Name = project.Name,
                    Description = project.Description,
                    Created = project.Created,
                    Finalized = project.Finalized
                }).ToList(),
                Tasks = employee.Tasks.Select(task => new Models.Task
                {
                    TaskId = task.TaskId,
                    Name = task.Name,
                    Description = task.Description,
                    Created = task.Created,
                    StartedWorking = task.StartedWorking,
                    Finished = task.Finished,
                }).ToList(),
                Issues = employee.Issues.Select(issue => new Issue
                {
                    IssueId = issue.IssueId,
                    Name = issue.Name,
                    Description = issue.Description,
                    Created = issue.Created,
                    StartedWorking = issue.StartedWorking,
                    Fixed = issue.Fixed,
                    IssueCreator = issue.IssueCreator,
                    TaskId = issue.TaskId
                }).ToList()
            };

            return employeeDto;
        }

        public async Task<Employee?> GetEmployeeForClaims(string username)
        {
            return await _context.Employees
                .Where(u => u.Username.Equals(username))
                .Select(employee => new Employee
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    Role = employee.Role
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool?> IsAccountLocked(string username)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(u => u.Username.Equals(username));

            if(employee is null)
            {
                return null;
            }

            if(employee.LoginAttempts >= 5)
            {
                employee.LockedEnabled = true;
                employee.LoginAttempts = 0;
                employee.LockedUntil = DateTimeOffset.UtcNow.AddMinutes(5);

                var notification = new Notification
                {
                    Name = "System notification",
                    Content = "Your account was temporary blocked because of multiple failed login attempts.",
                    Created = DateTimeOffset.UtcNow,
                    ReceiverId = employee.EmployeeId
                };

                _context.Add(notification);
                _ = await _context.SaveChangesAsync();
            }

            if(employee.LockedEnabled)
            {
                int dateComparasion = DateTimeOffset.Compare(DateTimeOffset.UtcNow, employee.LockedUntil.Value);

                if (dateComparasion > 0)
                {
                    employee.LockedUntil = null;
                    employee.LockedEnabled = false;
                    employee.LoginAttempts = 0;

                    _ = await _context.SaveChangesAsync();
                }
            }

            return employee.LockedEnabled;
        }

        public async Task<bool> RegisterEmployee(EmployeeRegisterDto employee, IFormFile image)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(employee.Password, salt);

            var (imageUrl, _) = await _imageService.UploadToCloudinary(image, 300, 300);

            var newEmployee = new Employee
            {
                Username = employee.Username,
                Role = employee.Role,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Password = hashedPassword,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                Created = DateTimeOffset.UtcNow,
                ProfilePicture = imageUrl,
                LastLogin = DateTimeOffset.UtcNow,
                CompanyId = employee.CompanyId,
                TierId = employee.TierId,
                SupervisorId = employee.SupervisorId // CHANGE THIS !
            };

            _context.Add(newEmployee);

            return await _context.SaveChangesAsync() > 0;
        }

        public Task<(bool updated, Employee)> UpdateEmployee(int employeeId, EmployeeDto employeeDto, IFormFile image)
        {
            throw new NotImplementedException();
        }
    }
}
