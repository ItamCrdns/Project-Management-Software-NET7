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
                    dbContext.Workload.Add(new Workload { WorkloadId = j });
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

            if (!await dbContext.Tasks.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    dbContext.Tasks.Add(
                        new CompanyPMO_.NET.Models.Task
                        {
                            Name = $"Task {i}",
                            Description = $"Description {i}",
                            Created = DateTime.Now.AddMinutes(i), // Increment on iteration
                            StartedWorking = (i == 0 || i == 1) ? DateTime.UtcNow : null,
                            ExpectedDeliveryDate = DateTime.Now.AddHours(-2),
                            TaskCreatorId = 1,
                            ProjectId = (i % 3) + 1
                        });
                }
            }

            if (!await dbContext.EmployeeTasks.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    dbContext.EmployeeTasks.Add(
                        new EmployeeTask
                        {
                            EmployeeId = i,
                            TaskId = i
                        });
                }
            }

            if (!await dbContext.Issues.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    dbContext.Issues.Add(
                        new Issue
                        {
                            Name = $"Issue {i}",
                            Description = $"Description {i}",
                            Created = DateTime.Now,
                            StartedWorking = DateTime.Now.AddMinutes(15),
                            ExpectedDeliveryDate = DateTime.Now.AddHours(-2),
                            IssueCreatorId = 1,
                            TaskId = (i % 3) + 1
                        });
                }
            }

            if (!await dbContext.EmployeeIssues.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    dbContext.EmployeeIssues.Add(
                        new EmployeeIssue
                        {
                            EmployeeId = i,
                            IssueId = i
                        });
                }
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeAssignedProjects_ReturnsSuccess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeAssignedProjects([1, 2, 3]);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Employee assigned projects count updated successfully.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeAssignedProjects_ReturnsSomethingWentWrong()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeAssignedProjects([4, 5, 6]);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Something went wrong");
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
        public async void WorkloadRepository_UpdateEmployeeCompletedProjects_ReturnsSomethingWentWrong()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCompletedProjects([4, 5, 6]);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Something went wrong");
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
        public async void WorkloadRepository_UpdateEmployeeCompletedTasks_ReturnsSomethingWentWrong()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCompletedTasks([4, 5, 6]);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Something went wrong");
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

        [Fact]
        public async void WorkloadRepository_GetWorkloadByEmployee_ReturnsWorkload()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.GetWorkloadByEmployee("test0");

            result.Should().NotBeNull();
            result.WorkloadId.Should().Be(1);
            result.Employee.EmployeeId.Should().Be(1);
        }

        [Fact]
        public async void WorkloadRepository_GetWorkloadByEmployee_ReturnsNull()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.GetWorkloadByEmployee("test999");

            result.Should().BeNull();
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeAssignedTasks_ReturnsSuccess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeAssignedTasks([1, 2, 3]);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Employee assigned tasks count updated successfully.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeAssignedTasks_ReturnsSomethingWentWrong()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeAssignedTasks([4, 5, 6]);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Something went wrong");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeAssignedIssues_ReturnsSuccess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeAssignedIssues([1, 2, 3]);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Employee assigned issues count updated successfully.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeAssignedIssues_ReturnsErrorNoWorkloadsToUpdate()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeAssignedIssues([4, 5, 6]);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Something went wrong");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeCompletedIssues_ReturnsSuccess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCompletedIssues([1, 2, 3]);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Employee completed issues count updated successfully.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeCompletedIssues_ReturnsErrorNoWorkloadsToUpdate()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCompletedIssues([4, 5, 6]);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Something went wrong");
        }

        [Fact]
        public async void WorkloadRepository_UpdateOverdueProjects_ReturnsSucess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateOverdueProjects();

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Overdue projects updated successfully.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateOverdueTasks_ReturnsSucess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateOverdueTasks();

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Overdue tasks updated successfully.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateOverdueIssues_ReturnsSucess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateOverdueIssues();

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Overdue issues updated successfully.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeCreatedTasks_ReturnsSuccess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCreatedTasks(1);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Employee created tasks count updated successfully.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeCreatedTasks_ReturnsNotFound()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCreatedTasks(4);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Workload entity not found.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeCreatedIssues_ReturnsSuccess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCreatedIssues(1);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Employee created issues count updated successfully.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeCreatedIssues_ReturnsNotFound()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCreatedIssues(4);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Workload entity not found.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeCreatedProjects_ReturnsSuccess()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCreatedProjects(1);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Employee created projects count updated successfully.");
        }

        [Fact]
        public async void WorkloadRepository_UpdateEmployeeCreatedProjects_ReturnsNotFound()
        {
            var dbContext = await GetDatabaseContext();
            var workloadRepository = new WorkloadRepository(dbContext);

            var result = await workloadRepository.UpdateEmployeeCreatedProjects(4);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Workload entity not found.");
        }
    }
}
