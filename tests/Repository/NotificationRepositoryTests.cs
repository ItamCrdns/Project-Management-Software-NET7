using CompanyPMO_.NET.Hubs;
using CompanyPMO_.NET.Interfaces;
using Microsoft.AspNetCore.SignalR;
using FakeItEasy;
using CompanyPMO_.NET.Data;
using Microsoft.EntityFrameworkCore;
using CompanyPMO_.NET.Repository;
using FluentAssertions;
using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Models;

namespace Tests.Repository
{
    public class NotificationRepositoryTests
    {
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
        public NotificationRepositoryTests()
        {
            _hubContext = A.Fake<IHubContext<NotificationHub, INotificationClient>>();
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
            dbContext.Database.EnsureCreated();

            await ResetDb.Reset(dbContext);

            if (!await dbContext.Employees.AnyAsync())
            {
                for (int i = 0; i < 3; i++)
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
                            CompanyId = (i % 2) + 1,
                            TierId = (i % 2) + 1,
                            LockedEnabled = true,
                            LoginAttempts = i,
                            LockedUntil = DateTime.UtcNow,
                            SupervisorId = i == 3 ? null : i,
                            PasswordVerified = i == 3 ? null : DateTime.UtcNow.AddMinutes(-i)
                        });
                };
            }

            if (!await dbContext.Tiers.AnyAsync())
            {
                for (int j = 1; j < 3; j++)
                {
                    dbContext.Tiers.Add(
                        new Tier
                        {
                            Name = $"Supervisor",
                            Duty = $"test{j}",
                            Created = DateTime.UtcNow
                        });
                }
            }

            if (!await dbContext.Companies.AnyAsync())
            {
                for (int j = 1; j < 3; j++)
                {
                    dbContext.Companies.Add(
                        new Company
                        {
                            Name = $"test{j}",
                            CeoUserId = j,
                            AddressId = j,
                            ContactEmail = $"test{j}",
                            ContactPhoneNumber = $"test{j}",
                            AddedById = j,
                            Logo = $"test{j}"
                        });
                }
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async void SendNotificationsBulk_ShouldReturnSuccess()
        {
            var dbContext = await GetDatabaseContext();
            var notificationRepository = new NotificationRepository(dbContext, _hubContext);

            var notificationName = "Test Notification";
            var notificationContent = "Test Notification Content";
            var senderId = 1;
            var receiverIds = new int[] { 2, 3 };

            var result = await notificationRepository.SendNotificationsBulk(notificationName, notificationContent, senderId, receiverIds);

            result.Should().BeOfType<OperationResult>();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Notifications sent successfully");
        }

        [Fact]
        public async void SendNotificationsBulk_ShouldReturnFailureEmptyReceivers()
        {
            var dbContext = await GetDatabaseContext();
            var notificationRepository = new NotificationRepository(dbContext, _hubContext);

            var notificationName = "Test Notification";
            var notificationContent = "Test Notification Content";
            var senderId = 1;
            var receiverIds = new int[] { };

            var result = await notificationRepository.SendNotificationsBulk(notificationName, notificationContent, senderId, receiverIds);

            result.Should().BeOfType<OperationResult>();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Failed to send notifications");
        }
    }
}
