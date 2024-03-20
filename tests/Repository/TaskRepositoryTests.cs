using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;

namespace Tests.Repository
{
    public class TaskRepositoryTests
    {
        private readonly IImage _image;
        private readonly IUtility _utility;
        public TaskRepositoryTests()
        {
            _image = A.Fake<IImage>();
            _utility = A.Fake<IUtility>();
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
                        new CompanyPMO_.NET.Models.Task
                        {
                            Name = $"Task {i}",
                            Description = $"Description {i}",
                            Created = DateTime.Now.AddMinutes(i), // Increment on iteration
                            StartedWorking = DateTime.Now.AddMinutes(15),
                            Finished = DateTime.Now.AddHours(1),
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
                            Role = $"test{i}",
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
                            EmployeeId = (i % 2) + 1,
                            TaskId = (i % 3) + 1
                        });
                }
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async void TaskRepository_AddEmployeesToTask_ReturnsAdded()
        {
            int taskId = 1;
            List<int> employeeIds = [1, 2, 3];

            IEnumerable<EmployeeShowcaseDto> employeesDto = new List<EmployeeShowcaseDto>
            {
                new()
                {
                    EmployeeId = 1,
                    Username = "Test1",
                    ProfilePicture = "profilepicture.jpg"
                },
                new()
                {
                    EmployeeId = 2,
                    Username = "Test2",
                    ProfilePicture = "profilepicture.jpg"
                },
            };

            var tupleResult = ("Success", employeesDto);

            A.CallTo(() => _utility.AddEmployeesToEntity<EmployeeTask, CompanyPMO_.NET.Models.Task>(
                A<List<int>>._,
                A<string>._,
                A<int>._,
                A<Func<int, int, Task<bool>>>._))
                .Returns(tupleResult);

            var taskRepository = new TaskRepository(await GetDatabaseContext(), _image, _utility);

            var result = await taskRepository.AddEmployeesToTask(taskId, employeeIds);

            result.Item2.Should().BeEquivalentTo(employeesDto);
            result.status.Should().Be("Success");
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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var fakeTask = A.Fake<TaskDto>();
            fakeTask.Name = "Test";
            fakeTask.Description = "Test Description";
            fakeTask.Created = DateTime.Now;
            List<int> employeeIds = [1, 2, 3];

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
        public async void TaskRepository_DoesTaskExist_ReturnsTrue()
        {
            int taskId = 1;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = await taskRepository.DoesTaskExist(taskId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void TaskRepository_DoesTaskExist_ReturnsFalse()
        {
            int taskId = 1337;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = await taskRepository.DoesTaskExist(taskId);

            result.Should().BeFalse();
        }

        [Fact]
        public async void TaskRepository_FinishedWorkingOnTask_ReturnsTrue()
        {
            int taskId = 1;
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = await taskRepository.FinishedWorkingOnTask(employeeId, taskId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void TaskRepository_FinishedWorkingOnTask_ReturnsFalse()
        {
            int taskId = 1;
            int employeeId = 300;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = await taskRepository.FinishedWorkingOnTask(employeeId, taskId);

            result.Should().BeFalse();
        }

        [Fact]
        public async void TaskRepository_GetEmployeesWorkingOnTask_ReturnsListOfEmployees()
        {
            int taskId = 1;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = await taskRepository.GetEmployeesWorkingOnTask(taskId);

            result.Should().BeOfType<List<Employee>>();
            result.Should().BeEmpty();
        }

        [Fact]
        public async void TaskRepository_GetTaskById_ReturnsTask()
        {
            int taskId = 1;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = await taskRepository.GetTaskById(taskId);

            result.Should().BeOfType<CompanyPMO_.NET.Models.Task>();
            result.Should().NotBeNull();
        }

        [Fact]
        public async void TaskRepository_GetTaskById_ReturnsNoTask()
        {
            int taskId = 11123;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = await taskRepository.GetTaskById(taskId);

            result.Should().BeNull();
        }

        [Fact]
        public async void TaskRepository_GetTasks_ReturnsTasks()
        {
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = await taskRepository.GetTasks(page, pageSize);

            result.Should().BeOfType<List<CompanyPMO_.NET.Models.Task>>();
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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            int[] tasksIds = [1, 2, 3];

            var tupleResult = (tasksIds, 10, 1);

            A.CallTo(() => _utility.GetEntitiesEmployeeCreatedOrParticipates<EmployeeTask, CompanyPMO_.NET.Models.Task>(
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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            int[] tasksIds = [];

            var tupleResult = (tasksIds, 0, 0);

            A.CallTo(() => _utility.GetEntitiesEmployeeCreatedOrParticipates<EmployeeTask, CompanyPMO_.NET.Models.Task>(
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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            int[] tasksIds = [1, 2, 3];

            var tupleResult = (tasksIds, 10, 1);

            A.CallTo(() => _utility.GetEntitiesByEntityId<CompanyPMO_.NET.Models.Task>(
                A<int>._,
                A<string>._,
                A<string>._,
                A<int>._,
                A<int>._))
                .Returns(tupleResult);

            Expression<Func<CompanyPMO_.NET.Models.Task, bool>> fakeBoolExpression = x => true; // Just evaluate to true
            Expression<Func<CompanyPMO_.NET.Models.Task, object>> fakeObjectExpression = x => x.Name;

            var tupleExpressionsResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<CompanyPMO_.NET.Models.Task>(
                A<int>._, A<IEnumerable<int>>._, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            int[] tasksIds = [];

            var tupleResult = (tasksIds, 0, 0);

            A.CallTo(() => _utility.GetEntitiesByEntityId<CompanyPMO_.NET.Models.Task>(
                A<int>._,
                A<string>._,
                A<string>._,
                A<int>._,
                A<int>._))
                .Returns(tupleResult);

            Expression<Func<CompanyPMO_.NET.Models.Task, bool>> fakeBoolExpression = x => x.Name == "DOES NOT EXIST"; // Just evaluate to true
            Expression<Func<CompanyPMO_.NET.Models.Task, object>> fakeObjectExpression = x => x.Name;

            var tupleExpressionsResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<CompanyPMO_.NET.Models.Task>(
                A<int>._, A<IEnumerable<int>>._, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
                .Returns(tupleExpressionsResult);

            var result = await taskRepository.GetTasksByProjectId(projectId, fakeFilterParams);

            result.Should().BeOfType<DataCountPages<TaskDto>>();
            result.Data.Should().BeEmpty();
            result.Count.Should().Be(0);
            result.Pages.Should().Be(0);
        }

        [Fact]
        public async void TaskRepository_IsEmployeeAlreadyInTask_ReturnsTrue()
        {
            int taskId = 1;
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = await taskRepository.IsEmployeeAlreadyInTask(employeeId, taskId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void TaskRepository_IsEmployeeAlreadyInTask_ReturnsFalse()
        {
            int taskId = 1;
            int employeeId = 77777;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = await taskRepository.IsEmployeeAlreadyInTask(employeeId, taskId);

            result.Should().BeFalse();
        }

        [Fact]
        public async void TaskRepository_SelectImages_ReturnsImages()
        {
            ICollection<Image> images = new List<Image>
            {
                new()
                {
                    ImageId = 1,
                    EntityType = "Task",
                    EntityId = 1,
                    ImageUrl = "test.jpg",
                    PublicId = "test",
                    Created = DateTime.Now,
                    UploaderId = 1
                },
                new()
                {
                    ImageId = 2,
                    EntityType = "Task",
                    EntityId = 1,
                    ImageUrl = "test2.jpg",
                    PublicId = "test2",
                    Created = DateTime.Now,
                    UploaderId = 1
                },
                new()
                {
                    ImageId = 3,
                    EntityType = "Project",
                    EntityId = 1,
                    ImageUrl = "test3.jpg",
                    PublicId = "test3",
                    Created = DateTime.Now,
                    UploaderId = 1
                }
            };

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = taskRepository.SelectImages(images);

            result.Should().BeEquivalentTo(images.Where(x => x.EntityType == "Task"));
            result.Should().BeOfType<List<Image>>();
            result.Should().NotBeEmpty();
            result.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Should().NotBeEquivalentTo(images.Where(x => x.EntityType == "Project"));
        }

        [Fact]
        public async void TaskRepository_StartingWorkingOnTask_ReturnsTrue()
        {
            int taskId = 1;
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = await taskRepository.StartingWorkingOnTask(employeeId, taskId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void TaskRepository_StartingWorkingOnTask_ReturnsFalse()
        {
            int taskId = 9999;
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = await taskRepository.StartingWorkingOnTask(employeeId, taskId);

            result.Should().BeFalse();
        }

        [Fact]
        public async void TaskRepository_GetTasksByEmployeeUsername_ReturnsTasks()
        {
            string username = "test1";
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var tupleResult = (dbContext.Tasks.ToList(), 10, 1);

            A.CallTo(() => _utility.GetAllEntities<CompanyPMO_.NET.Models.Task>(
                A<FilterParams>._, A<List<string>>._
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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var tupleResult = (dbContext.Tasks.ToList(), 0, 0);

            A.CallTo(() => _utility.GetAllEntities<CompanyPMO_.NET.Models.Task>(
                A<FilterParams>._, A<List<string>>._
                )).Returns(tupleResult);

            var result = await taskRepository.GetAllTasks(fakeFilterParams);

            result.Should().BeOfType<DataCountPages<TaskDto>>();
            result.Data.Should().BeEmpty();
            result.Pages.Should().Be(0);
            result.Count.Should().Be(0);
            result.Data.Should().BeOfType<List<TaskDto>>();
        }

        [Fact]
        public async void TaskRepository_TaskDtoSelectQuery_ReturnsTaskDto()
        {
            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

            var result = taskRepository.TaskDtoSelectQuery(dbContext.Tasks.ToList());

            result.Should().BeOfType<List<TaskDto>>();
            result.Should().NotBeEmpty();
            result.Should().HaveCountGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void TaskRepository_GetTasksGroupedByProject_ReturnsGroupedTasks()
        {
            var dbContext = await GetDatabaseContext();

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

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

            var taskRepository = new TaskRepository(dbContext, _image, _utility);

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
    }
}
