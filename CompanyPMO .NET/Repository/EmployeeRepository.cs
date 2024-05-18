﻿using CompanyPMO_.NET.Common;
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
        private readonly IUtility _utilityService;

        public EmployeeRepository(ApplicationDbContext context, IImage imageService, IUtility utilityService)
        {
            _context = context;
            _imageService = imageService;
            _utilityService = utilityService;
        }

        public async Task<(AuthenticationResult result, string message, EmployeeDto employee)> AuthenticateEmployee(string username, string password)
        {
            bool? isAccountLocked = await IsAccountLocked(username);

            var employee = await _context.Employees
                .FirstOrDefaultAsync(u => u.Username.Equals(username));

            if (isAccountLocked is true)
            {
                var minutes = _utilityService.MinutesUntilTimeArrival(employee.LockedUntil);
                return (new AuthenticationResult { Blocked = true }, $"Your account has been blocked for {minutes} minutes.", null);
            }

            if (!string.IsNullOrEmpty(password))
            {
                if (employee is null)
                {
                    return (new AuthenticationResult { DoesntExist = true }, "Apparently, this user does not exist.", null);
                }

                if (BCrypt.Net.BCrypt.Verify(password, employee.Password))
                {
                    if (employee.LoginAttempts > 0)
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
            return await _context.Employees
                .Where(x => x.EmployeeId == employeeId)
                .Include(t => t.Tier)
                .FirstOrDefaultAsync();
        }

        public async Task<DataCountPages<EmployeeShowcaseDto>> GetEmployeesBySupervisorId(int supervisorId, FilterParams filterParams)
        {
            // TODO: FIX WORKLOAD ORDER BY. NOT WORKING
            var (whereExpression, orderByExpression) = _utilityService.BuildWhereAndOrderByExpressions<Employee>(supervisorId, null, "SupervisorId", "Created", filterParams);

            bool ShallOrderAscending = filterParams.Sort is not null && filterParams.Sort.Equals("ascending");
            bool ShallOrderDescending = filterParams.Sort is not null && filterParams.Sort.Equals("descending");

            var page = filterParams.Page;
            var pageSize = filterParams.PageSize;

            int toSkip = (page - 1) * pageSize;

            int totalEmployeesCount = await _context.Employees
                .Where(whereExpression)
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalEmployeesCount / pageSize);

            List<EmployeeShowcaseDto> employees = new();

            if (ShallOrderAscending)
            {
                employees = await _context.Employees
                    .Where(whereExpression)
                    .OrderBy(orderByExpression)
                    .Select(x => new EmployeeShowcaseDto
                    {
                        EmployeeId = x.EmployeeId,
                        Username = x.Username,
                        ProfilePicture = x.ProfilePicture,
                        LastLogin = x.LastLogin,
                        Tier = new TierDto
                        {
                            TierId = x.Tier.TierId,
                            Name = x.Tier.Name,
                            Duty = x.Tier.Duty
                        },
                        Workload = x.Workload
                    })
                    .Skip(toSkip)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else if (ShallOrderDescending || (!ShallOrderAscending && !ShallOrderDescending))
            {
                employees = await _context.Employees
                    .Where(whereExpression)
                    .OrderByDescending(orderByExpression)
                    .Select(x => new EmployeeShowcaseDto
                    {
                        EmployeeId = x.EmployeeId,
                        Username = x.Username,
                        ProfilePicture = x.ProfilePicture,
                        LastLogin = x.LastLogin,
                        Tier = new TierDto
                        {
                            TierId = x.Tier.TierId,
                            Name = x.Tier.Name,
                            Duty = x.Tier.Duty
                        },
                        Workload = x.Workload
                    })
                    .Skip(toSkip)
                    .Take(pageSize)
                    .ToListAsync();
            }

            return new DataCountPages<EmployeeShowcaseDto>
            {
                Data = employees,
                Count = totalEmployeesCount,
                Pages = totalPages
            };
        }

        public async Task<EmployeeDto?> GetEmployeeByUsername(string username)
        {
            return await _context.Employees
                .Where(e => e.Username == username)
                .Select(x => new EmployeeDto
                {
                    EmployeeId = x.EmployeeId,
                    Username = x.Username,
                    ProfilePicture = x.ProfilePicture,
                    Tier = x.Tier,
                    Supervisor = x.Supervisor == null ? null : new EmployeeDto
                    {
                        EmployeeId = x.Supervisor.EmployeeId,
                        Username = x.Supervisor.Username,
                        ProfilePicture = x.Supervisor.ProfilePicture
                    },
                    Workload = x.Workload
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Employee?> GetEmployeeForClaims(string username)
        {
            return await _context.Employees
                .Include(x => x.Tier)
                .Where(u => u.Username.Equals(username))
                .Select(employee => new Employee
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    Tier = employee.Tier
                })
                .FirstOrDefaultAsync();
        }

        public async Task<DataCountPages<EmployeeShowcaseDto>> GetEmployeesShowcasePaginated(int userId, int page, int pageSize)
        {
            int entitiesToSkip = (page - 1) * pageSize;

            int totalEmployeesCount = await _context.Employees
                .Where(x => x.EmployeeId != userId)
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalEmployeesCount / pageSize);

            var employees = await _context.Employees
                .Where(x => x.EmployeeId != userId) // Exclude the employee that is making the request
                .OrderByDescending(p => p.Username)
                .Select(employee => new EmployeeShowcaseDto
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    ProfilePicture = employee.ProfilePicture
                })
                .Skip(entitiesToSkip)
                .Take(pageSize)
                .ToListAsync();

            return new DataCountPages<EmployeeShowcaseDto>
            {
                Data = employees,
                Count = totalEmployeesCount,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<EmployeeShowcaseDto>> GetProjectEmployees(int projectId, int page, int pageSize)
        {
            int toSkip = (page - 1) * pageSize;

            var employees = await _context.Employees
                .Where(x => _context.EmployeeProjects.Where(p => p.ProjectId.Equals(projectId)).Select(e => e.EmployeeId).Contains(x.EmployeeId))
                .OrderByDescending(x => x.Username)
                .Select(employee => new EmployeeShowcaseDto
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    ProfilePicture = employee.ProfilePicture
                })
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            int count = await _context.EmployeeProjects
                .Where(p => p.ProjectId.Equals(projectId))
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)count / pageSize);

            return new DataCountPages<EmployeeShowcaseDto>
            {
                Data = employees,
                Count = count,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<EmployeeShowcaseDto>> GetEmployeesWorkingInTheSameCompany(string username, int page, int pageSize)
        {
            int toSkip = (page - 1) * pageSize;

            int totalEmployeesCount = await _context.Employees
                .Where(x => x.CompanyId == _context.Employees.FirstOrDefault(x => x.Username == username).CompanyId && x.Username != username)
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalEmployeesCount / pageSize);

            IEnumerable<EmployeeShowcaseDto> employees = await _context.Employees
                .OrderByDescending(p => p.Username)
                .Where(x => x.CompanyId == _context.Employees.FirstOrDefault(x => x.Username == username).CompanyId && x.Username != username)
                .Select(employee => new EmployeeShowcaseDto
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    ProfilePicture = employee.ProfilePicture
                })
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            return new DataCountPages<EmployeeShowcaseDto>
            {
                Data = employees,
                Count = totalEmployeesCount,
                Pages = totalPages
            };
        }

        public async Task<bool?> IsAccountLocked(string username)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(u => u.Username.Equals(username));

            if (employee is null)
            {
                return null;
            }

            if (employee.LoginAttempts >= 5)
            {
                employee.LockedEnabled = true;
                employee.LoginAttempts = 0;
                employee.LockedUntil = DateTime.UtcNow.AddMinutes(5);

                var notification = new Notification
                {
                    Name = "System notification",
                    Content = "Your account was temporary blocked because of multiple failed login attempts.",
                    Created = DateTime.UtcNow,
                    ReceiverId = employee.EmployeeId
                };

                _context.Add(notification);
                _ = await _context.SaveChangesAsync();
            }

            if (employee.LockedEnabled)
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

        public async Task<(string result, bool status)> RegisterEmployee(EmployeeRegisterDto employee, IFormFile image)
        {
            string username = employee.Username.ToLower();

            bool usernameExists = await _context.Employees
                .AnyAsync(u => u.Username.Equals(username));

            if (usernameExists)
            {
                return ("Username already registered", false);
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(employee.Password, salt);

            var (imageUrl, _) = await _imageService.UploadToCloudinary(image, 300, 300);

            if (employee.Email is null || employee.PhoneNumber is null || employee.FirstName is null || employee.Gender is null || employee.LastName is null || employee.Role is null)
            {
                return ("Email, first name, last name, gender, phone number and role cannot be null", false);
            }

            var newEmployee = new Employee
            {
                Username = employee.Username,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Password = hashedPassword,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                Created = DateTime.UtcNow,
                ProfilePicture = imageUrl,
                LastLogin = DateTime.UtcNow,
                CompanyId = employee.CompanyId,
                TierId = employee.TierId,
                SupervisorId = employee.SupervisorId // CHANGE THIS !
            };

            _context.Add(newEmployee);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return ("Employee created", true);
            }
            else
            {
                return ("Something went wrong", false);
            }
        }

        public async Task<Dictionary<string, object>> GetEmployeesByCompanyPaginated(int companyId, int page, int pageSize)
        {
            int entitiesToSkip = (page - 1) * pageSize;

            int totalEmployeesCount = await _context.Employees
                .Where(c => c.CompanyId.Equals(companyId))
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalEmployeesCount / pageSize);

            // Save only what we need
            List<EmployeeShowcaseDto> employees = await _context.Employees
                .OrderByDescending(p => p.Username)
                .Where(c => c.CompanyId.Equals(companyId)) // Filter by only company
                .Select(employee => new EmployeeShowcaseDto
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    ProfilePicture = employee.ProfilePicture
                })
                .Skip(entitiesToSkip)
                .Take(pageSize)
                .ToListAsync();

            // Create a dictionary to return both the employees and the total count of employees
            var result = new Dictionary<string, object>
            {
                { "data", employees },
                { "count", totalEmployeesCount },
                { "pages", totalPages }
            };

            return result;
        }

        public async Task<DataCountPages<EmployeeShowcaseDto>> SearchProjectEmployees(string search, int projectId, int page, int pageSize)
        {
            int toSkip = (page - 1) * pageSize;

            var employees = await _context.Employees
                .Where(x => _context.EmployeeProjects.Where(p => p.ProjectId.Equals(projectId)).Select(e => e.EmployeeId).Contains(x.EmployeeId) && x.Username.Contains(search))
                .OrderByDescending(x => x.Username)
                .Select(employee => new EmployeeShowcaseDto
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    ProfilePicture = employee.ProfilePicture
                })
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            int count = await _context.EmployeeProjects.Where(p => p.ProjectId.Equals(projectId)).CountAsync();

            int totalPages = (int)Math.Ceiling((double)employees.Count / pageSize);

            return new DataCountPages<EmployeeShowcaseDto>
            {
                Data = employees,
                Count = count,
                Pages = totalPages
            };
        }

        public IEnumerable<EmployeeShowcaseDto> EmployeeShowcaseQuery(IEnumerable<Employee> employees)
        {
            IEnumerable<EmployeeShowcaseDto> employeeDtos = employees.Select(employee => new EmployeeShowcaseDto
            {
                EmployeeId = employee.EmployeeId,
                Username = employee.Username,
                ProfilePicture = employee.ProfilePicture
            }).ToList();

            return employeeDtos;
        }

        public async Task<DataCountPages<EmployeeShowcaseDto>> SearchEmployeesByCompanyPaginated(string search, int companyId, int page, int pageSize)
        {
            int toSkip = (page - 1) * pageSize;

            // Total count of found employees
            int totalEmployeesCount = await _context.Employees
                .Where(c => c.CompanyId.Equals(companyId) && c.Username.Contains(search))
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalEmployeesCount / pageSize);

            List<EmployeeShowcaseDto> employees = await _context.Employees
                .OrderByDescending(p => p.Username)
                .Where(i => i.CompanyId.Equals(companyId) && i.Username.Contains(search)) // Filter ny company and search parameters
                .Select(employee => new EmployeeShowcaseDto
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    ProfilePicture = employee.ProfilePicture
                })
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            return new DataCountPages<EmployeeShowcaseDto>
            {
                Data = employees,
                Count = totalEmployeesCount,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<EmployeeShowcaseDto>> SearchEmployeesWorkingInTheSameCompany(string search, string username, int page, int pageSize)
        {
            int toSkip = (page - 1) * pageSize;

            int totalEmployeesCount = await _context.Employees
                .Where(x => x.CompanyId == _context.Employees.FirstOrDefault(x => x.Username == username).CompanyId && x.Username.Contains(search) && x.Username != username)
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalEmployeesCount / pageSize);

            IEnumerable<EmployeeShowcaseDto> employees = await _context.Employees
                .OrderByDescending(p => p.Username)
                .Where(x => x.CompanyId == _context.Employees.FirstOrDefault(x => x.Username == username).CompanyId && x.Username.Contains(search) && x.Username != username)
                .Select(employeeIds => new EmployeeShowcaseDto
                {
                    EmployeeId = employeeIds.EmployeeId,
                    Username = employeeIds.Username,
                    ProfilePicture = employeeIds.ProfilePicture
                })
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            return new DataCountPages<EmployeeShowcaseDto>
            {
                Data = employees,
                Count = totalEmployeesCount,
                Pages = totalPages
            };
        }

        public async Task<TierDto> GetEmployeeTier(int employeeId)
        {
            var tier = await _context.Employees
                .Where(e => e.EmployeeId.Equals(employeeId))
                .Select(t => new TierDto
                {
                    TierId = t.Tier.TierId,
                    Name = t.Tier.Name,
                    Duty = t.Tier.Duty
                })
                .FirstOrDefaultAsync();

            return tier;
        }

        public Task<string> GetEmployeeUsernameById(int employeeId)
        {
            var username = _context.Employees
                .Where(e => e.EmployeeId.Equals(employeeId))
                .Select(u => u.Username)
                .FirstOrDefaultAsync();

            return username;
        }

        public async Task<Dictionary<string, object>> GetAndSearchEmployeesByProjectsCreatedInClient(string? employeeIds, int clientId, int page, int pageSize)
        {
            // Returns selectedEmployees: employeeIds, allEmployees: all employees that created projects in the requested client
            // Default values if no queryparams are passed
            int requestedPage = page == 0 ? 1 : page;
            int requestedPageSize = pageSize == 0 ? 10 : pageSize;

            int toSkip = (requestedPage - 1) * requestedPageSize;

            int[] employeeIdsArray = employeeIds?.Split('-').Select(int.Parse).ToArray();

            var allEmployees = await _context.Employees
                .Where(e => _context.Projects.Where(p => p.CompanyId.Equals(clientId)).Select(p => p.ProjectCreatorId).Contains(e.EmployeeId))
                .Select(employee => new EmployeeShowcaseDto
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    ProfilePicture = employee.ProfilePicture
                })
                .OrderByDescending(x => x.EmployeeId) // Not optimal. Might create a new field in employee called latestProjectCreated and order by that
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            int allEmployeesCount = await _context.Employees
                .Where(e => _context.Projects.Where(p => p.CompanyId.Equals(clientId)).Select(p => p.ProjectCreatorId).Contains(e.EmployeeId))
                .CountAsync();

            int allEmployeesPages = (int)Math.Ceiling((double)allEmployeesCount / requestedPageSize);

            var selectedEmployees = await _context.Employees
                .Where(e => employeeIdsArray.Contains(e.EmployeeId))
                .Select(employee => new EmployeeShowcaseDto
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    ProfilePicture = employee.ProfilePicture
                })
                .OrderByDescending(x => x.EmployeeId) // Not optimal. Might create a new field in employee called latestProjectCreated and order by that
                .ToListAsync();

            var allEmployeesCountAndPages = new DataCountPages<EmployeeShowcaseDto>
            {
                Data = allEmployees,
                Count = allEmployeesCount,
                Pages = allEmployeesPages
            };

            var result = new Dictionary<string, object>
            {
                { "selectedEmployees", selectedEmployees },
                { "allEmployees", allEmployeesCountAndPages }
            };

            return result;

        }

        public async Task<IEnumerable<EmployeeShowcaseDto>> GetEmployeesFromAListOfEmployeeIds(string employeeIds)
        {
            if (string.IsNullOrEmpty(employeeIds))
            {
                // Return an empty array if the employeeIds string are not provided
                return Enumerable.Empty<EmployeeShowcaseDto>();
            }

            int[] employeeIdsArray = employeeIds.Split('-').Select(int.Parse).ToArray();

            IEnumerable<EmployeeShowcaseDto> employees = await _context.Employees
                .Where(e => employeeIdsArray.Contains(e.EmployeeId))
                .Select(employee => new EmployeeShowcaseDto
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    ProfilePicture = employee.ProfilePicture
                })
                .ToListAsync();

            return employees;
        }

        public async Task<DataCountPages<EmployeeShowcaseDto>> SearchEmployeesShowcasePaginated(int userId, string search, int page, int pageSize)
        {
            int toSkip = (page - 1) * pageSize;

            // Total count of foung employees
            int totalEmployeesCount = await _context.Employees
                .Where(e => e.Username.Contains(search) && e.EmployeeId != userId)
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalEmployeesCount / pageSize);

            List<EmployeeShowcaseDto> employees = await _context.Employees
                .OrderByDescending(e => e.Username)
                .Where(e => e.Username.Contains(search) && e.EmployeeId != userId)
                .Select(employee => new EmployeeShowcaseDto
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    ProfilePicture = employee.ProfilePicture
                })
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            return new DataCountPages<EmployeeShowcaseDto>
            {
                Data = employees,
                Count = totalEmployeesCount,
                Pages = totalPages
            };
        }

        public async Task<OperationResult<EmployeeShowcaseDto>> UpdateEmployee(int employeeId, UpdateEmployeeDto employee, IFormFile? profilePicture, string? currentPassword)
        {
            Employee employeeToUpdate = await _context.Employees.FindAsync(employeeId);

            if (employeeToUpdate is null)
            {
                return new OperationResult<EmployeeShowcaseDto> { Success = false, Message = "Employee not found", Data = new EmployeeShowcaseDto() };
            }

            if (currentPassword is null || string.IsNullOrWhiteSpace(currentPassword))
            {
                // If the password has been verified it means the user has provided the correct password no more than 5 minutes ago
                var latestVerification = await PasswordLastVerification(employeeId);

                if (latestVerification.Success is false)
                {
                    return new OperationResult<EmployeeShowcaseDto> { Success = false, Message = latestVerification.Message, Data = new EmployeeShowcaseDto() };
                }
                // If true we just continue with the updateq
            }
            else // If a password is provided compare
            {
                if (!BCrypt.Net.BCrypt.Verify(currentPassword, employeeToUpdate.Password))
                {
                    return new OperationResult<EmployeeShowcaseDto> { Success = false, Message = "Wrong password", Data = new EmployeeShowcaseDto() };
                }
            }

            if (employee.Email is not null && !string.IsNullOrWhiteSpace(employee.Email))
            {
                employeeToUpdate.Email = employee.Email;
            }

            if (employee.PhoneNumber is not null && !string.IsNullOrWhiteSpace(employee.PhoneNumber))
            {
                employeeToUpdate.PhoneNumber = employee.PhoneNumber;
            }

            if (employee.Password is not null && !string.IsNullOrWhiteSpace(employee.Password))
            {
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(employee.Password, salt);
                employeeToUpdate.Password = hashedPassword;
            }

            if (employee.FirstName is not null && !string.IsNullOrWhiteSpace(employee.FirstName))
            {
                employeeToUpdate.FirstName = employee.FirstName;
            }

            if (employee.LastName is not null && !string.IsNullOrWhiteSpace(employee.LastName))
            {
                employeeToUpdate.LastName = employee.LastName;
            }

            if (employee.Gender is not null && !string.IsNullOrWhiteSpace(employee.Gender))
            {
                employeeToUpdate.Gender = employee.Gender;
            }

            if (profilePicture is not null && profilePicture.Length > 0)
            {
                var (imageUrl, _) = await _imageService.UploadToCloudinary(profilePicture, 300, 300);
                employeeToUpdate.ProfilePicture = imageUrl;
            }

            _context.Employees.Update(employeeToUpdate);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                EmployeeShowcaseDto returnEmployee = new()
                {
                    EmployeeId = employeeToUpdate.EmployeeId,
                    Username = employeeToUpdate.Username,
                    ProfilePicture = employeeToUpdate.ProfilePicture
                };

                return new OperationResult<EmployeeShowcaseDto> { Success = true, Message = "Employee updated", Data = returnEmployee };
            }
            else
            {
                return new OperationResult<EmployeeShowcaseDto> { Success = false, Message = "Something went wrong", Data = new EmployeeShowcaseDto() };
            }
        }

        public async Task<OperationResult<bool>> ConfirmPassword(int employeeId, string password)
        {
            var pw = await _context.Employees.Where(x => x.EmployeeId == employeeId).Select(x => x.Password).FirstOrDefaultAsync();

            bool passwordMatches = BCrypt.Net.BCrypt.Verify(password, pw);

            if (passwordMatches)
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                employee.PasswordVerified = DateTime.UtcNow;
                int rowsAffected = await _context.SaveChangesAsync();

                string message = rowsAffected > 0 ? "Password confirmed" : "Password confirmed, but failed to update the database";

                return new OperationResult<bool> { Success = true, Message = message, Data = true };
            }
            else
            {
                return new OperationResult<bool> { Success = false, Message = "Password does not match", Data = false };
            }
        }

        public async Task<OperationResult<DateTime>> PasswordLastVerification(int employeeId)
        {
            DateTime? lastVerification = await _context.Employees.Where(x => x.EmployeeId == employeeId).Select(x => x.PasswordVerified).FirstOrDefaultAsync();

            if (lastVerification is null)
            {
                return new OperationResult<DateTime>
                {
                    Success = false,
                    Message = "Password has never been verified",
                    Data = DateTime.UtcNow
                };
            }

            DateTime lastVerificationPlus5 = lastVerification.Value.AddMinutes(5);

            if (DateTime.UtcNow < lastVerificationPlus5)
            {
                return new OperationResult<DateTime>
                {
                    Success = true,
                    Message = "Password has been verified",
                    Data = lastVerification.Value
                };
            }
            else
            {
                return new OperationResult<DateTime>
                {
                    Success = false,
                    Message = "Password not verified in the last 5 minutes",
                    Data = lastVerification.Value
                };
            }
        }
    }
}
