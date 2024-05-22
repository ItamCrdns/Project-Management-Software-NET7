using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Tests.Repository
{
    public class WorkloadRepositoryTests
    {
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
                            Name = $"test{j}",
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

            if (!await dbContext.Projects.AnyAsync())
            {
                for (int j = 1; j < 3; j++)
                {
                    dbContext.Projects.Add(
                        new Project
                        {
                            Name = $"test{j}",
                            Description = $"test{j}",
                            Created = DateTime.UtcNow,
                            Finished = null,
                            ProjectCreatorId = j,
                            CompanyId = j,
                            Priority = j,
                            ExpectedDeliveryDate = DateTime.UtcNow,
                            Lifecycle = $"test{j}"
                        });
                }
            }

            if (!await dbContext.Workload.AnyAsync())
            {
                for (int j = 1; j < 3; j++)
                {
                    dbContext.Workload.Add(new Workload{ WorkloadId = j });
                }
            }

            if (!await dbContext.EmployeeProjects.AnyAsync())
            {
                for (int j = 1; j < 3; j++)
                {
                    dbContext.EmployeeProjects.Add(
                        new EmployeeProject
                        {
                            EmployeeId = j,
                            ProjectId = j
                        });
                }
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeAssignedProjectsCount_ReturnsSuccess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeAssignedProjectsCount([1, 2, 3]);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Employee assigned projects count updated successfully.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeAssignedProjectsCount_ReturnsErrorNoWorkloadsToUpdate()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeAssignedProjectsCount([4, 5, 6]);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("No workloads to update");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeCompletedProjects_ReturnsSuccess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCompletedProjects([1, 2, 3]);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Employee completed projects count updated successfully.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeCompletedProjects_ReturnsErrorNoWorkloadsToUpdate()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCompletedProjects([4, 5, 6]);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("No workloads to update");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeCompletedTasks_ReturnsSuccess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCompletedTasks([1, 2, 3]);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Employee completed tasks count updated successfully.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeCompletedTasks_ReturnsErrorNoWorkloadsToUpdate()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCompletedTasks([4, 5, 6]);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("No workloads to update");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeWorkloadAssignedTasksAndIssues_ReturnsSuccess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeWorkloadAssignedTasksAndIssues([1, 2, 3]);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Employee workload assigned tasks and issues updated successfully.");
            result.Data.Should().BeOfType<List<WorkloadDto>>();
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            foreach (var workload in result.Data)
            {
                workload.WorkloadSum.Should().NotBeNullOrEmpty();
                workload.WorkloadSum.Should().BeOneOf("Very High", "High", "Medium", "Low", "None");
            }   
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeWorkloadAssignedTasksAndIssues_ReturnsErrorNoWorkloadsToUpdate()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeWorkloadAssignedTasksAndIssues([4, 5, 6]);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("No workloads to update");
        }

        [Fact]
        public async void WorkloadRepository_CreateWorkloadEntityForEmployee_ReturnsCreated()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.CreateWorkloadEntityForEmployee(4);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Workload entity created successfully.");
        }

        [Fact]
        public async void WorkloadRepository_CreateWorkloadEntityForEmployee_ReturnsErrorAlreadyExists()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.CreateWorkloadEntityForEmployee(1);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Workload entity already exists for this employee.");
        }

        [Fact]
        public async void WorkloadRepository_CreateWorkloadEntityForEmployee_ReturnsErrorEmployeeNotFound()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.CreateWorkloadEntityForEmployee(11);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Employee not found.");
        }
    }
}
