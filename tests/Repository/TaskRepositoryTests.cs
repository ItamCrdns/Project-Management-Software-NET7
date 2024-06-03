using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Interfaces.Timeline_interfaces;
using CompanyPMO_.NET.Interfaces.Workload_interfaces;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;
using Task = CompanyPMO_.NET.Models.Task;

namespace Tests.Repository
{
    public class TaskRepositoryTests
    {
        private readonly IUtility _utility;
        private readonly IWorkloadTask _workload;
        private readonly ITimelineManagement _timelineManagement;
        public TaskRepositoryTests()
        {
            _utility = A.Fake<IUtility>();
            _workload = A.Fake<IWorkloadTask>();
            _timelineManagement = A.Fake<ITimelineManagement>();
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

            if (!await dbContext.Tasks.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    dbContext.Tasks.Add(
                        new Task
                        {
                            Name = $"Task {i}",
                            Description = $"Description {i}",
                            Created = DateTime.Now.AddMinutes(i), // Increment on iteration
                            StartedWorking = (i == 0 || i == 1) ? DateTime.UtcNow : null,
                            Finished = (i == 6 || i == 7) ? DateTime.Now.AddHours(1) : null,
                            TaskCreatorId = 1,
                            ProjectId = (i % 3) + 1
                        });
                }
            }

            if (!await dbContext.Projects.AnyAsync())
            {
                for (int i = 1; i < 4; i++)
                {
                    dbContext.Projects.Add(
                        new Project
                        {
                            Name = $"Project {i}",
                            Description = $"Description {i}",
                            Created = DateTime.Now,
                            Finished = DateTime.Now.AddHours(1),
                            ProjectCreatorId = 1,
                            CompanyId = (i % 2) + 1,
                            Priority = i,
                            ExpectedDeliveryDate = DateTime.Now.AddDays(1),
                            Lifecycle = $"Lifecycle {i}"
                        });
                }
            }

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
                            Password = $"test{i}",
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

            if (!await dbContext.EmployeeTasks.AnyAsync())
            {
                for (int i = 1; i < 11; i++)
                {
                    dbContext.EmployeeTasks.Add(
                        new EmployeeTask
                        {
                            EmployeeId = i <= 2 ? 4 : i,
                            TaskId = i
                        });
                }
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async void TaskRepository_CreateTask_ReturnsSuccess()
        {
            int employeeId = 1;
            int projectId = 1;

            List<IFormFile> fakeIFormFileList =
                [
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test")), 0, 0, "Data", "test.jpg"),
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 2")), 0, 0, "Data 2", "test2.jpg"),
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 3")), 0, 0, "Data 3", "test3.jpg")
                    ];

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var fakeTask = A.Fake<TaskDto>();
            fakeTask.Name = "Test";
            fakeTask.Description = "Test Description";
            fakeTask.Created = DateTime.Now;

            List<int> employeeIds = [2, 3];

            var result = await taskRepository.CreateTask(fakeTask, employeeId, projectId, fakeIFormFileList, employeeIds, true);

            result.Should().NotBeNull();
            result.Message.Should().Be("Task created successfully");
            result.Success.Should().BeTrue();
            result.Data.Should().BeOfType(typeof(int));
            result.Data.Should().NotBe(0);
        }

        [Fact]
        public async void TaskRepository_CreateTask_ReturnsFailure()
        {
            int employeeId = 1;
            int projectId = 1;

            List<IFormFile> fakeIFormFileList = [];

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var fakeTask = A.Fake<TaskDto>();
            fakeTask.Name = "";
            fakeTask.Description = "";
            List<int> employeeIds = [2, 3];

            var result = await taskRepository.CreateTask(fakeTask, employeeId, projectId, fakeIFormFileList, employeeIds, true);

            result.Should().NotBeNull();
            result.Message.Should().Be("Task name and description are required");
            result.Success.Should().BeFalse();
            result.Data.Should().BeOfType(typeof(int));
            result.Data.Should().Be(0);
        }

        [Fact]
        public async void TaskRepository_CreateTask_ReturnsSuccessYouCantAddYourselfError()
        {
            int employeeId = 1;
            int projectId = 1;

            List<IFormFile> fakeIFormFileList = [];

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var fakeTask = A.Fake<TaskDto>();
            fakeTask.Name = "Test";
            fakeTask.Description = "Test Description";
            fakeTask.Created = DateTime.Now;
            List<int> employeeIds = [1, 2, 3];

            A.CallTo(() => _workload.UpdateEmployeeCreatedTasks(A<int>._)).Returns(new OperationResult { Success = true, Message = "Success" });

            A.CallTo(() => _workload.UpdateEmployeeAssignedTasks(A<int[]>._)).Returns(new OperationResult { Success = true, Message = "Success" });

            var result = await taskRepository.CreateTask(fakeTask, employeeId, projectId, fakeIFormFileList, employeeIds, true);

            result.Should().NotBeNull();
            result.Message.Should().Be("Task created successfully");
            result.Success.Should().BeTrue();
            result.Data.Should().BeOfType(typeof(int));
            result.Data.Should().NotBe(0);
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().Contain("You can't add yourself");
        }

        [Fact]
        public async void TaskRepository_CreateTask_ReturnsSuccessWithErrors()
        {
            int employeeId = 1;
            int projectId = 1;

            List<IFormFile> fakeIFormFileList =
                [
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test")), 0, 0, "Data", "test.jpg"),
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 2")), 0, 0, "Data 2", "test2.jpg"),
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 3")), 0, 0, "Data 3", "test3.jpg")
                    ];

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var fakeTask = A.Fake<TaskDto>();
            fakeTask.Name = "Test";
            fakeTask.Description = "Test Description";
            fakeTask.Created = DateTime.Now;

            List<int> employeeIds = [2, 3];

            A.CallTo(() => _workload.UpdateEmployeeCreatedTasks(A<int>._)).Returns(new OperationResult { Success = false, Message = "Error" });

            A.CallTo(() => _workload.UpdateEmployeeAssignedTasks(A<int[]>._)).Returns(new OperationResult { Success = false, Message = "Error2" });

            var result = await taskRepository.CreateTask(fakeTask, employeeId, projectId, fakeIFormFileList, employeeIds, true);

            result.Should().NotBeNull();
            result.Message.Should().Be("Task created successfully");
            result.Success.Should().BeTrue();
            result.Data.Should().BeOfType(typeof(int));
            result.Data.Should().NotBe(0);
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.Should().HaveCount(2);
            result.Errors[0].Should().Be("Failed to update the workload of the task creator. Error = Error");
            result.Errors[1].Should().Be("Failed to update the workload of the employees. Error = Error2");
        }

        [Fact]
        public async void TaskRepository_DoesTaskExist_ReturnsTrue()
        {
            int taskId = 1;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.DoesTaskExist(taskId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void TaskRepository_DoesTaskExist_ReturnsFalse()
        {
            int taskId = 1337;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.DoesTaskExist(taskId);

            result.Should().BeFalse();
        }

        [Fact]
        public async void TaskRepository_GetEmployeesWorkingOnTask_ReturnsListOfEmployees()
        {
            int taskId = 1;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.GetEmployeesWorkingOnTask(taskId);

            result.Should().BeOfType<List<Employee>>();
            result.Should().NotBeEmpty();
            result.Should().HaveCountGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void TaskRepository_GetEmployeesWorkingOnTask_ReturnsEmptyList()
        {
            int taskId = 11;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.GetEmployeesWorkingOnTask(taskId);

            result.Should().BeOfType<List<Employee>>();
            result.Should().BeEmpty();
        }

        [Fact]
        public async void TaskRepository_GetTaskById_ReturnsTask()
        {
            int taskId = 1;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.GetTaskById(taskId, 1, 1);

            result.Should().NotBeNull();
            result.Should().BeOfType<EntityParticipantOrOwnerDTO<TaskDto>>();
            result.Entity.Should().NotBeNull();
            result.Entity.Should().BeOfType<TaskDto>();
            result.Entity.TaskCreator.Should().NotBeNull();
            result.Entity.TaskCreator.Should().BeOfType<EmployeeShowcaseDto>();
            result.Entity.TaskCreator.Username.Should().NotBeNullOrEmpty();
            result.Entity.Project.Should().NotBeNull();
            result.Entity.Project.Should().BeOfType<ProjectShowcaseDto>();
            result.Entity.Project.Name.Should().NotBeNullOrEmpty();
            result.Entity.Name.Should().NotBeNullOrEmpty();
            result.Entity.EmployeeCount.Should().BeGreaterThanOrEqualTo(1);
            result.Entity.Employees.Should().NotBeEmpty();
            result.Entity.Employees.Should().HaveCountGreaterThanOrEqualTo(1);
            foreach (var employee in result.Entity.Employees)
            {
                employee.Should().NotBeNull();
                employee.Should().BeOfType<EmployeeShowcaseDto>();
                employee.Username.Should().NotBeNullOrEmpty();
            }
            result.IsOwner.Should().BeTrue();
            result.IsParticipant.Should().BeFalse();
        }

        [Fact]
        public async void TaskRepository_GetTaskById_ReturnsNoTask()
        {
            int taskId = 11123;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.GetTaskById(taskId, 2, 1);

            result.Should().BeNull();
        }

        [Fact]
        public async void TaskRepository_GetTasks_ReturnsTasks()
        {
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.GetTasks(page, pageSize);

            result.Should().BeOfType<List<Task>>();
            result.Should().NotBeEmpty();
            result.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Should().HaveCountGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void TaskRepository_GetTasks_ReturnsNoTasks()
        {
            int page = 100;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();

            dbContext.Tasks.RemoveRange(dbContext.Tasks);
            await dbContext.SaveChangesAsync();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.GetTasks(page, pageSize);

            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public async void TaskRepository_GetTasksShowcaseByEmployeeUsername_ReturnsTasks()
        {
            string username = "test1";
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            int[] tasksIds = [1, 2, 3];

            var tupleResult = (tasksIds, 10, 1);

            A.CallTo(() => _utility.GetEntitiesEmployeeCreatedOrParticipates<EmployeeTask, Task>(
                A<string>._,
                A<string>._,
                A<string>._,
                A<int>._,
                A<int>._))
                .Returns(tupleResult);

            var result = await taskRepository.GetTasksShowcaseByEmployeeUsername(username, page, pageSize);

            result.Should().BeOfType<DataCountPages<TaskShowcaseDto>>();
            result.Data.Should().NotBeEmpty();
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Data.Should().BeOfType<List<TaskShowcaseDto>>();
        }

        [Fact]
        public async void TaskRepository_GetTasksShowcaseByEmployeeUsername_ReturnsNoTasks()
        {
            string username = "test1";
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            int[] tasksIds = [];

            var tupleResult = (tasksIds, 0, 0);

            A.CallTo(() => _utility.GetEntitiesEmployeeCreatedOrParticipates<EmployeeTask, Task>(
                A<string>._,
                A<string>._,
                A<string>._,
                A<int>._,
                A<int>._))
                .Returns(tupleResult);

            var result = await taskRepository.GetTasksShowcaseByEmployeeUsername(username, page, pageSize);

            result.Should().BeOfType<DataCountPages<TaskShowcaseDto>>();
            result.Data.Should().BeEmpty();
            result.Data.Should().BeOfType<List<TaskShowcaseDto>>();
        }

        [Fact]
        public async void TaskRepository_GetTasksByProjectId_ReturnsTasks()
        {
            int projectId = 1;

            var fakeFilterParams = A.Fake<FilterParams>();

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            Expression<Func<Task, bool>> fakeBoolExpression = x => true; // Just evaluate to true
            Expression<Func<Task, object>> fakeObjectExpression = x => x.Name;

            var tupleExpressionsResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<Task>(
                A<int>._, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
                .Returns(tupleExpressionsResult);

            var result = await taskRepository.GetTasksByProjectId(projectId, fakeFilterParams);

            result.Should().BeOfType<DataCountPages<TaskDto>>();
            result.Data.Should().NotBeEmpty();
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
            result.Data.Should().BeOfType<List<TaskDto>>();
        }

        [Fact]
        public async void TaskRepository_GetTasksByProjectId_ReturnsNoTasks()
        {
            int projectId = 9999;

            var fakeFilterParams = A.Fake<FilterParams>();

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            Expression<Func<Task, bool>> fakeBoolExpression = x => x.Name == "DOES NOT EXIST"; // Just evaluate to true
            Expression<Func<Task, object>> fakeObjectExpression = x => x.Name;

            var tupleExpressionsResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<Task>(
                A<int>._, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
                .Returns(tupleExpressionsResult);

            var result = await taskRepository.GetTasksByProjectId(projectId, fakeFilterParams);

            result.Should().BeOfType<DataCountPages<TaskDto>>();
            result.Data.Should().BeEmpty();
            result.Count.Should().Be(0);
            result.Pages.Should().Be(0);
        }

        [Fact]
        public async void TaskRepository_GetTasksByEmployeeUsername_ReturnsTasks()
        {
            string username = "test1";
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            int[] tasksIds = [1, 2, 3];

            var tupleResult = (tasksIds, 10, 1);

            A.CallTo(() => _utility.GetEntitiesByEmployeeUsername<EmployeeTask>(
                A<string>._,
                A<string>._,
                A<int>._,
                A<int>._
                )).Returns(tupleResult);

            var result = await taskRepository.GetTasksByEmployeeUsername(username, page, pageSize);

            result.Should().BeOfType<DataCountPages<TaskDto>>();
            result.Data.Should().NotBeEmpty();
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Data.Should().BeOfType<List<TaskDto>>();
        }

        [Fact]
        public async void TaskRepository_GetAllTasksShowcase_ReturnsTasks()
        {
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.GetAllTasksShowcase(page, pageSize);

            result.Should().BeOfType<DataCountPages<TaskShowcaseDto>>();
            result.Data.Should().NotBeEmpty();
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Data.Should().BeOfType<List<TaskShowcaseDto>>();
        }

        [Fact]
        public async void TaskRepository_GetAllTasksShowcase_ReturnsNoTasks()
        {
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();

            dbContext.Tasks.RemoveRange(dbContext.Tasks);
            await dbContext.SaveChangesAsync();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.GetAllTasksShowcase(page, pageSize);

            result.Should().BeOfType<DataCountPages<TaskShowcaseDto>>();
            result.Data.Should().BeEmpty();
            result.Pages.Should().Be(0);
            result.Count.Should().Be(0);
            result.Data.Should().BeOfType<List<TaskShowcaseDto>>();
        }

        [Fact]
        public async void TaskRepository_GetAllTasks_ReturnsTasks()
        {
            var fakeFilterParams = A.Fake<FilterParams>();

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            ICollection<TaskDto> tasksCollection = [.. dbContext.Tasks.Select(x => new TaskDto
            {
                Name = x.Name
            })];

            var tupleResult = (tasksCollection, 10, 1);

            A.CallTo(() => _utility.GetAllEntities(
                A<FilterParams>._, A<Expression<Func<Task, TaskDto>>>._
                )).Returns(tupleResult);

            var result = await taskRepository.GetAllTasks(fakeFilterParams);

            result.Should().BeOfType<DataCountPages<TaskDto>>();
            result.Data.Should().NotBeEmpty();
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Data.Should().BeOfType<List<TaskDto>>();
        }

        [Fact]
        public async void TaskRepository_GetAllTasks_ReturnsNoTasks()
        {
            var fakeFilterParams = A.Fake<FilterParams>();

            var dbContext = await GetDatabaseContext();

            dbContext.Tasks.RemoveRange(dbContext.Tasks);
            await dbContext.SaveChangesAsync();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            ICollection<TaskDto> tasksCollection = [.. dbContext.Tasks.Select(x => new TaskDto
            {
                Name = x.Name
            })];

            var tupleResult = (tasksCollection, 0, 0);

            A.CallTo(() => _utility.GetAllEntities(
                A<FilterParams>._, A<Expression<Func<Task, TaskDto>>>._
                )).Returns(tupleResult);

            var result = await taskRepository.GetAllTasks(fakeFilterParams);

            result.Should().BeOfType<DataCountPages<TaskDto>>();
            result.Data.Should().BeEmpty();
            result.Pages.Should().Be(0);
            result.Count.Should().Be(0);
            result.Data.Should().BeOfType<List<TaskDto>>();
        }

        [Fact]
        public async void TaskRepository_GetTasksGroupedByProject_ReturnsGroupedTasks()
        {
            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            FilterParams filterParams = new()
            {
                Page = 1,
                PageSize = 10,
            };

            var result = await taskRepository.GetTasksGroupedByProject(filterParams, 1, 5, 1);

            result.Data.Should().BeOfType<List<ProjectTaskGroup>>();
            result.Data.Should().NotBeEmpty();
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Should().NotBeNull();
            foreach (var data in result.Data)
            {
                data.Tasks.Should().NotBeEmpty();
                data.Tasks.Should().HaveCountGreaterThanOrEqualTo(1);
                data.Count.Should().BeGreaterThanOrEqualTo(1);
                data.Pages.Should().BeGreaterThanOrEqualTo(1);
                data.Tasks.Should().BeOfType<List<TaskShowcaseDto>>();
            }
        }

        [Fact]
        public async void TaskRepository_GetTasksShowcaseByProjectId_ReturnsTasksList()
        {
            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.GetTasksShowcaseByProjectId(1, 1, 10);

            result.Data.Should().BeOfType<List<TaskShowcaseDto>>();
            result.Data.Should().NotBeEmpty();
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
            foreach (var data in result.Data)
            {
                data.TaskCreator.Should().NotBeNull();
                data.TaskCreator.Should().BeOfType<EmployeeShowcaseDto>();
                data.TaskCreator.Username.Should().NotBeNullOrEmpty();
                data.Project.Should().NotBeNull();
                data.Project.Should().BeOfType<ProjectSomeInfoDto>();
                data.Project.Name.Should().NotBeNullOrEmpty();
                data.Name.Should().NotBeNullOrEmpty();
                data.Created.Should().NotBe(default);
            }
        }

        [Fact]
        public async void TaskRepository_IsParticipant_ReturnsTrue()
        {
            int taskId = 7;
            int employeeId = 7;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.IsParticipant(taskId, employeeId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void TaskRepository_IsParticipant_ReturnsFalse()
        {
            int taskId = 1;
            int employeeId = 100;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.IsParticipant(taskId, employeeId);

            result.Should().BeFalse();
        }

        [Fact]
        public async void TaskRepository_IsOwner_ReturnsTrue()
        {
            int taskId = 1;
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.IsOwner(taskId, employeeId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void TaskRepository_IsOwner_ReturnsFalse()
        {
            int taskId = 1;
            int employeeId = 100;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.IsOwner(taskId, employeeId);

            result.Should().BeFalse();
        }

        [Fact]
        public async void TaskRepository_SetTasksStartBulk_ReturnsSuccess()
        {
            int[] taskIds = [7, 8, 9];
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.SetTasksStartBulk(taskIds, employeeId);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Tasks started successfully");
        }

        [Fact]
        public async void TaskRepository_SetTasksStartBulk_ReturnsSucessButSomeTasksAlreadyStarted()
        {
            int[] taskIds = [1, 2, 3];
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.SetTasksStartBulk(taskIds, employeeId);

            result.Should().NotBeNull();
            result.Message.Should().Be("Tasks started successfully, however some tasks were already started");
            result.Success.Should().BeTrue();
            result.Errors.Should().HaveCountGreaterThanOrEqualTo(1);
            foreach (var error in result.Errors)
            {
                error.Should().NotBeNullOrEmpty();
                error.Should().BeOfType(typeof(string));
            }
            result.Errors[0].Should().Be("Task 1 is already started");
            result.Errors[1].Should().Be("Task 2 is already started");
        }

        [Fact]
        public async void TaskRepository_SetTasksStartBulk_ReturnsFailureTasksNotFound()
        {
            int[] taskIds = [100, 200, 300];
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.SetTasksStartBulk(taskIds, employeeId);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("No tasks found");
        }

        [Fact]
        public async void TaskRepository_SetTasksStartBulk_ReturnsFailureAllTasksAlreadyStarted()
        {
            int[] taskIds = [1, 2, 3];
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            foreach (var taskId in taskIds)
            {
                var task = await dbContext.Tasks.FindAsync(taskId);
                task.StartedWorking = DateTime.Now;
            }

            await dbContext.SaveChangesAsync();

            var result = await taskRepository.SetTasksStartBulk(taskIds, employeeId);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("All tasks are already started");
        }

        [Fact]
        public async void TaskRepository_SetTaskStart_ReturnsSuccess()
        {
            int taskId = 7;
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.SetTaskStart(taskId, employeeId);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Task started successfully");
        }

        [Fact]
        public async void TaskRepository_SetTaskStart_ReturnsFailureTaskNotFound()
        {
            int taskId = 100;
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.SetTaskStart(taskId, employeeId);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Task not found");
        }

        [Fact]
        public async void TaskRepository_SetTaskStart_ReturnsFailureTaskAlreadyStarted()
        {
            int taskId = 1;
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var task = await dbContext.Tasks.FindAsync(taskId);
            task.StartedWorking = DateTime.Now;

            await dbContext.SaveChangesAsync();

            var result = await taskRepository.SetTaskStart(taskId, employeeId);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Task is already started");
        }

        [Fact]
        public async void TaskRepository_SetTasksFinishedBulk_ReturnsSuccess()
        {
            int[] taskIds = [1, 5, 9];
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.SetTasksFinishedBulk(taskIds, employeeId);

            result.Should().NotBeNull();
            result.Message.Should().Be("Tasks finished successfully");
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async void TaskRepository_SetTasksFinishedBulk_ReturnsSucessButSomeTasksAlreadyFinished()
        {
            int[] taskIds = [5, 7, 8];
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.SetTasksFinishedBulk(taskIds, employeeId);

            result.Should().NotBeNull();
            result.Message.Should().Be("Tasks finished successfully, however some tasks were already finished");
            result.Success.Should().BeTrue();
            result.Errors.Should().HaveCountGreaterThanOrEqualTo(1);
            foreach (var error in result.Errors)
            {
                error.Should().NotBeNullOrEmpty();
                error.Should().BeOfType(typeof(string));
            }
            result.Errors[0].Should().Be("Task 7 is already finished");
            result.Errors[1].Should().Be("Task 8 is already finished");
        }

        [Fact]
        public async void TaskRepository_SetTasksFinishedBulk_ReturnsFailureTasksNotFound()
        {
            int[] taskIds = [100, 200, 300];
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.SetTasksFinishedBulk(taskIds, employeeId);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("No tasks found");
        }

        [Fact]
        public async void TaskRepository_SetTasksFinishedBulk_ReturnsFailureAllTasksAlreadyFinished()
        {
            int[] taskIds = [7, 8];
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.SetTasksFinishedBulk(taskIds, employeeId);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("All tasks are already finished");
        }

        [Fact]
        public async void TaskRepository_SetTaskFinished_ReturnsSuccess()
        {
            int taskId = 1;
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.SetTaskFinished(taskId, employeeId);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Task finished successfully");
        }

        [Fact]
        public async void TaskRepository_SetTaskFinished_ReturnsFailureTaskNotFound()
        {
            int taskId = 100;
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var result = await taskRepository.SetTaskFinished(taskId, employeeId);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Task not found");
        }

        [Fact]
        public async void TaskRepository_SetTaskFinished_ReturnsFailureTaskAlreadyStarted()
        {
            int taskId = 7;
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var taskRepository = new TaskRepository(dbContext, _utility, _workload, _timelineManagement);

            var task = await dbContext.Tasks.FindAsync(taskId);
            task.StartedWorking = DateTime.Now;

            await dbContext.SaveChangesAsync();

            var result = await taskRepository.SetTaskFinished(taskId, employeeId);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Task is already finished");
        }
    }
}
