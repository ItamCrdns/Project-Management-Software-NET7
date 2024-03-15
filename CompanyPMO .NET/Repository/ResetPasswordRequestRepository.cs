using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Services;
using Microsoft.EntityFrameworkCore;

namespace CompanyPMO_.NET.Repository
{
    public class ResetPasswordRequestRepository : IResetPasswordRequest
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        public ResetPasswordRequestRepository(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<OperationResult<string>> RequestExists(Guid requestGuid)
        {
            var request = await _context.ResetPasswordRequests.Include(x => x.Employee).FirstOrDefaultAsync(x => x.RequestGuid == requestGuid);

            if (request == null)
            {
                return new OperationResult<string>
                {
                    Success = false,
                    Message = "Request not found"
                };
            }

            return new OperationResult<string>
            {
                Success = true,
                Message = request.TokenExpiry < DateTime.UtcNow ? "Expired" : "Valid",
                Data = request.Employee.Email
            };
        }

        public int GenerateResetPasswordToken()
        {
            Random token = new();

            return token.Next(100000, 999999);
        }

        public async Task<OperationResult<Guid>> RequestPasswordReset(string email)
        {
            // Generate and set a token and its expiration date (15 minutes after its created)

            bool employeeWithEmailExists = await _context.Employees.AnyAsync(x => x.Email == email);

            if (!employeeWithEmailExists)
            {
                return new OperationResult<Guid> { Success = false, Message = "The email address provided is not registered. Please contact your administrator." };
            }

            bool tokenForEmployeeAlreadyExists = await _context.ResetPasswordRequests.AnyAsync(x => x.Employee.Email == email);

            if (tokenForEmployeeAlreadyExists)
            {
                // Delete old token, then continue generating a new one
                var oldTokens = await _context.ResetPasswordRequests
                    .Where(x => x.Employee.Email == email)
                    .ToListAsync();

                _context.ResetPasswordRequests.RemoveRange(oldTokens);

                await _context.SaveChangesAsync();
            }

            var tokenRequest = new ResetPasswordRequest
            {
                EmployeeId = await _context.Employees.Where(x => x.Email == email).Select(e => e.EmployeeId).FirstOrDefaultAsync(),
                Token = GenerateResetPasswordToken(),
                TokenExpiry = DateTime.UtcNow.AddMinutes(15)
            };

            _context.ResetPasswordRequests.Update(tokenRequest);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                // Send email with the token
                string emailMessage = $"Someone requested to reset your password.\nYour code is: {tokenRequest.Token} \nIf you did not request a password reset, you can safely ignore this email.";
                await _emailSender.SendEmailAsync(email, "Reset your password", emailMessage);

                return new OperationResult<Guid> { Success = true, Message = "Your request to reset your password has been received. Please review your email and enter the 6-digit code that was sent to you.", Data = tokenRequest.RequestGuid };
            }
            else
            {
                return new OperationResult<Guid> { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<OperationResult<string>> ResetPasswordWithToken(string email, int token, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new OperationResult<string> { Success = false, Message = "The email address cannot be empty." };
            }

            var employee = await _context.Employees.Where(x => x.Email == email).FirstOrDefaultAsync();

            if (employee is null)
            {
                return new OperationResult<string> { Success = false, Message = "The email address provided is not registered. Please contact your administrator." };
            }

            var tokenRequest = await _context.ResetPasswordRequests
                .Where(x => x.EmployeeId == employee.EmployeeId)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return new OperationResult<string> { Success = false, Message = "The new password cannot be empty." };
            }

            if (tokenRequest == null || tokenRequest.Token != token || token <= 0)
            {
                return new OperationResult<string> { Success = false, Message = "The token provided is invalid or has expired. Please request a new token." };
            }

            if (tokenRequest.TokenExpiry < DateTime.UtcNow)
            {
                return new OperationResult<string> { Success = false, Message = "The token provided has expired. Please request a new one." };
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword, salt);

            employee.Password = hashedPassword;

            _context.Employees.Update(employee);
            _context.ResetPasswordRequests.Remove(tokenRequest);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult<string> { Success = true, Message = "Your password has been reset successfully.", Data = employee.Username  };
            }
            else
            {
                return new OperationResult<string> { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<OperationResult<bool>> IsTokenValid(int token, Guid requestGuid)
        {
            var requestToken = await _context.ResetPasswordRequests.FirstOrDefaultAsync(x => x.Token == token && x.RequestGuid == requestGuid);

            if (requestToken is null)
            {
                return new OperationResult<bool> { Success = false, Message = "The token provided is invalid or has expired." };
            }

            var isTokenExpired = requestToken.TokenExpiry < DateTime.UtcNow;

            if (isTokenExpired)
            {
                return new OperationResult<bool> { Success = false, Message = "Expired" };
            }

            return new OperationResult<bool> { Success = true, Message = "Valid" };
        }
    }
}
