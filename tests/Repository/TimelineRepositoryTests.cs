using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Hubs;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using FakeItEasy;

namespace Tests.Repository
{
    public class TimelineRepositoryTests
    {
        private readonly IHubContext<TimelineHub, ITimelineClient> _hubContext;
        public TimelineRepositoryTests()
        {
            _hubContext = A.Fake<IHubContext<TimelineHub, ITimelineClient>>();
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
                            SupervisorId = i == 8 ? null : i,
                            PasswordVerified = i == 8 ? null : DateTime.UtcNow.AddMinutes(-i)
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

            if (!await dbContext.Timelines.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    dbContext.Timelines.Add(
                        new Timeline
                        {
                            Event = $"test{i}",
                            Created = DateTime.UtcNow,
                            EmployeeId = (i % 2) + 1,
                            Type = $"test{i}"
                        });
                }
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async void CreateTimelineEvent_ShouldReturnSuccess()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var timelineRepository = new TimelineRepository(dbContext, _hubContext);

            var timelineDto = new TimelineDto
            {
                Event = "Test Event",
                EmployeeId = 1,
                Type = "Test Type"
            };

            // Act
            var result = await timelineRepository.CreateTimelineEvent(timelineDto, UserRoles.Supervisor);

            // Assert
            result.Should().BeOfType<OperationResult>();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Timeline event created successfully.");
        }

        [Fact]
        public async void CreateTimelineEvent_ShouldReturnError()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var timelineRepository = new TimelineRepository(dbContext, _hubContext);

            var timelineDto = new TimelineDto
            {
                Event = "Test Event",
                EmployeeId = 1,
                Type = "Test Type"
            };

            // Act
            var result = await timelineRepository.CreateTimelineEvent(timelineDto, "Invalid tier");

            // Assert
            result.Should().BeOfType<OperationResult>();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Tier not found.");
        }

        [Fact]
        public async void CreateTimelineEventsBulk_ShouldReturnSuccess()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var timelineRepository = new TimelineRepository(dbContext, _hubContext);

            var timelineDtos = new List<TimelineDto>
            {
                new() {
                    Event = "Test Event",
                    EmployeeId = 1,
                    Type = "Test Type"
                },
                new() {
                    Event = "Test Event",
                    EmployeeId = 1,
                    Type = "Test Type"
                }
            };

            // Act
            var result = await timelineRepository.CreateTimelineEventsBulk(timelineDtos, UserRoles.Supervisor);

            // Assert
            result.Should().BeOfType<OperationResult>();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Timeline events created successfully.");
        }

        [Fact]
        public async void CreateTimelineEventsBulk_ShouldReturnError()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var timelineRepository = new TimelineRepository(dbContext, _hubContext);

            var timelineDtos = new List<TimelineDto>
            {
                new() {
                    Event = "Test Event",
                    EmployeeId = 1,
                    Type = "Test Type"
                },
                new() {
                    Event = "Test Event",
                    EmployeeId = 1,
                    Type = "Test Type"
                }
            };

            // Act
            var result = await timelineRepository.CreateTimelineEventsBulk(timelineDtos, "Invalid tier");

            // Assert
            result.Should().BeOfType<OperationResult>();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Tier not found.");
        }

        [Fact]
        public async void DeleteTimelineEvents_ShouldReturnSuccess()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var timelineRepository = new TimelineRepository(dbContext, _hubContext);

            var timelineIds = new int[] { 1, 2, 3 };

            // Act
            var result = await timelineRepository.DeleteTimelineEvents(timelineIds);

            // Assert
            result.Should().BeOfType<OperationResult>();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Timeline events deleted successfully.");
        }

        [Fact]
        public async void GetTimelineEvents_ShouldReturnDataCountPages()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var timelineRepository = new TimelineRepository(dbContext, _hubContext);

            var filterParams = new FilterParams
            {
                Page = 1,
                PageSize = 10
            };

            // Act
            var result = await timelineRepository.GetTimelineEvents(filterParams);

            // Assert
            result.Should().BeOfType<DataCountPages<TimelineShowcaseDto>>();
            result.Data.Should().HaveCount(10);
        }

        [Fact]
        public async void GetTimelineEventsByEmployee_ShouldReturnDataCountPages()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var timelineRepository = new TimelineRepository(dbContext, _hubContext);

            var filterParams = new FilterParams
            {
                Page = 1,
                PageSize = 10
            };

            // Act
            var result = await timelineRepository.GetTimelineEventsByEmployee(1, filterParams);

            // Assert
            result.Should().BeOfType<DataCountPages<TimelineShowcaseDto>>();
            result.Data.Should().HaveCount(5);
        }

        [Fact]
        public async void GetTimelineEvent_ShouldReturnEvent()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var timelineRepository = new TimelineRepository(dbContext, _hubContext);

            // Act
            var result = await timelineRepository.GetTimelineEvent(1);

            // Assert
            result.Should().BeOfType<TimelineDto>();
            result.Event.Should().Be("test0");
            result.Employee.Should().NotBeNull();
            result.Employee.Username.Should().Be("test0");
        }

        [Fact]
        public async void GetTimelineEvent_ShouldReturnNull()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var timelineRepository = new TimelineRepository(dbContext, _hubContext);

            // Act
            var result = await timelineRepository.GetTimelineEvent(100);

            // Assert
            result.Should().BeNull();
        }
    }
}
