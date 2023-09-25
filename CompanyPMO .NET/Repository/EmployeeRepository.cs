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

        public EmployeeRepository(ApplicationDbContext context, IImage imageService)
        {
            _context = context;
            _imageService = imageService;
        }
        public async Task<(bool authenticated, string result, EmployeeDto employee)> AuthenticateEmployee(string username, string password)
        {
            bool? isAccountLocked = await IsAccountLocked(username);

            var employee = await _context.Employees
                .FirstOrDefaultAsync(u => u.Username.Equals(username));

            if (isAccountLocked is true)
            {
                return (false, $"Access to your account is currently restricted until {employee.LockedUntil}. You can try again after this time.", null);
            }

            if(!string.IsNullOrEmpty(password))
            {
                if(employee is null)
                {
                    return (false, "Something went wrong.", null);
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

                    return (true, $"Welcome, {employeeReturn.Username}", employeeReturn);
                }
                else
                {
                    employee.LoginAttempts++;
                    _ = await _context.SaveChangesAsync();

                    return (false, $"Wrong credentials. You have tried {employee.LoginAttempts} times", null);
                }
            }

            return (false, "Something went wrong.", null);
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
                TierId = employee.TierId
            };

            _context.Add(newEmployee);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
