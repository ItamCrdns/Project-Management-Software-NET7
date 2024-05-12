using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using FakeItEasy;
using CompanyPMO_.NET.Services;

namespace Tests.Repository
{
    public class ResetPasswordRepositoryTests
    {
        private readonly IEmailSender _emailSender;
        public ResetPasswordRepositoryTests()
        {
            _emailSender = A.Fake<IEmailSender>();
        }
        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions
        {
            get
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

                return options;
            }
        }

        private static async Task<ApplicationDbContext> GetDatabaseContext()
        {
            var dbContext = new ApplicationDbContext(CreateNewContextOptions);
            await dbContext.Database.EnsureCreatedAsync();

            await ResetDb.Reset(dbContext);

            if (!await dbContext.Employees.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    dbContext.Employees.Add(
                        new Employee
                        {
                            Username = $"test{i}",
                            Email = $"test{i}",
                            PhoneNumber = $"test{i}",
                            Password = BCrypt.Net.BCrypt.HashPassword($"test{i}"),
                            FirstName = $"test{i}",
                            LastName = $"test{i}",
                            Gender = $"test{i}",
                            Created = DateTime.UtcNow,
                            ProfilePicture = $"test{i}",
                            LastLogin = DateTime.UtcNow,
                            CompanyId = (i % 2) + 1, // Only company Ids one and two
                            TierId = (i % 2) + 1,
                            LockedEnabled = true,
                            LoginAttempts = i,
                            LockedUntil = DateTime.UtcNow,
                            SupervisorId = i
                        });
                }
            }

            if (!await dbContext.ResetPasswordRequests.AnyAsync())
            {
                dbContext.ResetPasswordRequests.Add(new ResetPasswordRequest
                {
                    EmployeeId = 1,
                    RequestGuid = Guid.NewGuid(),
                    Token = 123456,
                    TokenExpiry = DateTime.UtcNow.AddHours(1)
                });

                dbContext.ResetPasswordRequests.Add(new ResetPasswordRequest
                {
                    EmployeeId = 2,
                    RequestGuid = Guid.NewGuid(),
                    Token = 654321,
                    TokenExpiry = DateTime.UtcNow.AddHours(-1)
                });
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async void ResetPasswordRequestRepository_RequestExists_ReturnsTrueValidToken()
        {
            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var requestGuid = dbContext.ResetPasswordRequests.First().RequestGuid;
            var result = await repository.RequestExists(requestGuid);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Valid");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_RequestExists_ReturnsFalseNotFound()
        {
            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var requestGuid = Guid.NewGuid();
            var result = await repository.RequestExists(requestGuid);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Request not found");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_RequestExists_ReturnsTrueExpiredToken()
        {
            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var requestGuid = dbContext.ResetPasswordRequests.Skip(1).First().RequestGuid;
            var result = await repository.RequestExists(requestGuid);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Expired");
            result.Data.Should().Be("test1");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_GenerateResetPasswordToken_ReturnsToken()
        {
            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = repository.GenerateResetPasswordToken();

            result.Should().BeOfType(typeof(int));
            result.Should().BeGreaterThan(0);
            result.ToString().Length.Should().Be(6);
        }

        [Fact]
        public async void ResetPasswordRequestRepository_RequestPasswordReset_ReturnsSuccess()
        {
            string email = "test1";

            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = await repository.RequestPasswordReset(email);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Your request to reset your password has been received. Please review your email and enter the 6-digit code that was sent to you.");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_RequestPasswordReset_ReturnsFailure()
        {
            string email = "test100";

            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = await repository.RequestPasswordReset(email);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("The email address provided is not registered. Please contact your administrator.");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_ResetPasswordWithToken_ReturnsSuccess()
        {
            string email = "test0";

            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = await repository.ResetPasswordWithToken(email, 123456, "newPassword");

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Your password has been reset successfully.");
            result.Data.Should().NotBeNullOrEmpty();
            result.Data.Should().Be("test0");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_ResetPasswordWithToken_ReturnsFailureNoEmailProvided()
        {
            string email = "   ";

            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = await repository.ResetPasswordWithToken(email, 999999, "newPassword");

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("The email address cannot be empty.");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_ResetPasswordWithToken_ReturnsFailureWrongToken()
        {
            string email = "test0";

            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = await repository.ResetPasswordWithToken(email, 999999, "newPassword");

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("The token provided is invalid or has expired. Please request a new token.");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_ResetPasswordWithToken_ReturnsFailureNoNewPassword()
        {
            string email = "test1";

            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = await repository.ResetPasswordWithToken(email, 123456, "   ");

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("The new password cannot be empty.");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_ResetPasswordWithToken_ReturnsFailureEmailDoesntExist()
        {
            string email = "test100";

            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = await repository.ResetPasswordWithToken(email, 123456, "newPassword");

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("The email address provided is not registered. Please contact your administrator.");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_ResetPasswordWithToken_ReturnsFailureTokenExpired()
        {
            string email = "test0";

            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = await repository.ResetPasswordWithToken(email, 654321, "newPassword");

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("The token provided is invalid or has expired. Please request a new token.");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_IsTokenValid_ReturnsTrueValid()
        {
            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var token = 123456;
            var requestGuid = dbContext.ResetPasswordRequests.First().RequestGuid;
            var result = await repository.IsTokenValid(token, requestGuid);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Valid");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_IsTokenValid_ReturnsFalseExpired()
        {
            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var token = 654321;
            var requestGuid = dbContext.ResetPasswordRequests.Skip(1).First().RequestGuid;
            var result = await repository.IsTokenValid(token, requestGuid);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Expired");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_IsTokenValid_ReturnsFalseInvalid()
        {
            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var token = 999999;
            var requestGuid = dbContext.ResetPasswordRequests.Skip(1).First().RequestGuid;
            var result = await repository.IsTokenValid(token, requestGuid);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("The token provided is invalid or has expired.");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_ResetPasswordWithCurrentPassword_ReturnsSuccess()
        {
            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = await repository.ResetPasswordWithCurrentPassword(1, "test0", "newPassword");

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Your password has been reset successfully.");
        }

        [Fact]
        public async void ResetPasswordRequestRepository_ResetPasswordWithCurrentPassword_ReturnsFailureWrongPassword()
        {
            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = await repository.ResetPasswordWithCurrentPassword(1, "wrongPassword", "newPassword");

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("The current password provided is incorrect.");
            result.Data.Should().BeFalse();
        }

        [Fact]
        public async void ResetPasswordRequestRepository_ResetPasswordWithCurrentPassword_ReturnsFailureAnyFieldEmpytString()
        {
            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = await repository.ResetPasswordWithCurrentPassword(1, "test0", "   ");

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Please fill all fields.");
            result.Data.Should().BeFalse();
        }

        [Fact]
        public async void ResetPasswordRequestRepository_ResetPasswordWithCurrentPassword_ReturnsFailureNoEmployee()
        {
            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = await repository.ResetPasswordWithCurrentPassword(999, "test0", "newPassword");

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Employee not found.");
            result.Data.Should().BeFalse();
        }

        [Fact]
        public async void ResetPasswordRequestRepository_ResetPasswordWithCurrentPassword_ReturnsFailurePasswordTooShort()
        {
            var dbContext = await GetDatabaseContext();
            var repository = new ResetPasswordRequestRepository(dbContext, _emailSender);

            var result = await repository.ResetPasswordWithCurrentPassword(1, "test0", "123");

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("The new password must be at least 8 characters long.");
            result.Data.Should().BeFalse();
        }
    }
}
