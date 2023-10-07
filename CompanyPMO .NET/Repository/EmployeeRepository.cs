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

        public Employee EmployeeQuery(Employee employee)
        {
            var query = new Employee
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

            return query;
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

            return EmployeeQuery(employee);
        }

        public async Task<IEnumerable<Employee>> GetEmployeeBySupervisorId(int supervisorId)
        {
            var employees = await _context.Employees
                .Where(s => s.SupervisorId.Equals(supervisorId))
                .Include(t => t.Tier)
                .Include(c => c.Company)
                .ToListAsync();

            return employees;
        }

        public async Task<EmployeeDto> GetEmployeeByUsername(string username)
        {
            Employee employee = await _context.Employees
                .Where(e => e.Username.Equals(username))
                .Include(t => t.Tier)
                .Include(s => s.Supervisor)
                .FirstOrDefaultAsync();
                
            // Count the number of projects, tasks and issues  the employee is working on
            int projectCount = await _context.EmployeeProjects
                .Where(p => p.EmployeeId.Equals(employee.EmployeeId))
                .Select(i => i.EmployeeId)
                .CountAsync();

            int taskCount = await _context.EmployeeTasks
                .Where(t => t.EmployeeId.Equals(employee.EmployeeId))
                .Select(i => i.EmployeeId)
                .CountAsync();

            int issueCount = await _context.EmployeeIssues
                .Where(i => i.EmployeeId.Equals(employee.EmployeeId))
                .Select(i => i.EmployeeId)
                .CountAsync();

            var employeeDto = new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                Username = employee.Username,
                Role = employee.Role,
                ProfilePicture = employee.ProfilePicture,
                Tier = employee.Tier,
                Supervisor = employee.SupervisorId is not null ? new EmployeeDto
                {
                    Username = employee.Supervisor.Username,
                    ProfilePicture = employee.Supervisor.ProfilePicture,
                } : null,
                ProjectCount = projectCount,
                TaskCount = taskCount,
                IssueCount = issueCount
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

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesWorkingInTheSameCompany(string username)
        {
            int companyId = await _context.Employees
                .Where(u => u.Username.Equals(username))
                .Select(c => c.CompanyId)
                .FirstOrDefaultAsync();

            IEnumerable<EmployeeDto> employees = await _context.Employees
                .Where(c => c.CompanyId.Equals(companyId) && !c.Username.Equals(username)) // Does not equal ugly af syntax
                .Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    Username = e.Username,
                    ProfilePicture = e.ProfilePicture
                })
                .ToListAsync();

            return employees;
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsByEmployeeUsername(string username)
        {
            // Materialize employeeId and projectIds into memory and then query the projects
            int employeeId = await _context.Employees
                .Where(u => u.Username.Equals(username))
                .Select(i => i.EmployeeId)
                .FirstOrDefaultAsync();

            List<int> projectIds = await _context.EmployeeProjects
                .Where(i => i.EmployeeId.Equals(employeeId))
                .Select(p => p.ProjectId)
                .ToListAsync();

            List<ProjectDto> projects = new();

            foreach(var id in projectIds)
            {
                var project = await _context.Projects
                    .Where(p => p.ProjectId.Equals(id))
                    .Select(p => new ProjectDto
                    {
                        ProjectId = p.ProjectId,
                        Name = p.Name,
                        Description = p.Description
                    })
                    .FirstOrDefaultAsync();

                projects.Add(project);
            }

            return projects;
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
