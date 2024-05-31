using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Interfaces.Notification_interfaces;
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

namespace Tests.Repository
{
    public class ProjectRepositoryTests
    {
        private readonly IImage _image;
        private readonly IUtility _utility;
        private readonly IWorkloadProject _workload;
        private readonly ITimelineManagement _timelineManagement;
        private readonly INotificationManagement _notificationManagement;
        public ProjectRepositoryTests()
        {
            _image = A.Fake<IImage>();
            _utility = A.Fake<IUtility>();
            _workload = A.Fake<IWorkloadProject>();
            _timelineManagement = A.Fake<ITimelineManagement>();
            _notificationManagement = A.Fake<INotificationManagement>();
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

            if (!await dbContext.Projects.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    dbContext.Projects.Add(
                        new Project
                        {
                            Name = $"Project {i}",
                            Description = $"Project {i} Description",
                            Created = DateTime.UtcNow,
                            Finished = (i == 6 || i == 7) ? DateTime.Now.AddHours(1) : null,
                            ProjectCreatorId = (i % 2) + 1,
                            CompanyId = (i % 2) + 1, // Only company Ids one and two
                            Priority = i,
                            ExpectedDeliveryDate = DateTime.UtcNow.AddMinutes(5),
                            Lifecycle = $"Lifecycle {i}",
                            StartedWorking = (i == 0 || i == 1) ? DateTime.UtcNow : null // Only projects 0 and 1 are started
                        });
                }
            }

            if (!await dbContext.Companies.AnyAsync())
            {
                for (int i = 1; i < 3; i++)
                {
                    dbContext.Companies.Add(
                        new Company
                        {
                            Name = $"Company {i}",
                            CeoUserId = i,
                            AddressId = i,
                            ContactEmail = $"Company {i} Email",
                            ContactPhoneNumber = $"Company {i} Phone Number",
                            AddedById = i,
                            Logo = $"Company {i} Logo"
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

            if (!await dbContext.EmployeeProjects.AnyAsync())
            {
                for (int i = 1; i < 11; i++)
                {
                    dbContext.EmployeeProjects.Add(
                        new EmployeeProject
                        {
                            EmployeeId = i <= 2 ? 4 : i,
                            ProjectId = i
                        });
                }
            }

            if (!await dbContext.Images.AnyAsync())
            {
                for (int i = 1; i < 3; i++)
                {
                    dbContext.Images.Add(
                        new Image
                        {
                            EntityType = "Project",
                            EntityId = i,
                            ImageUrl = $"test{i}",
                            PublicId = $"test{i}",
                            Created = DateTime.UtcNow,
                            UploaderId = i
                        });
                }
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async void ProjectRepository_AddImagesToExistingProject_ReturnsAddedImages()
        {
            int projectId = 5;
            List<IFormFile> fakeIFormFileList =
            [
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test")), 0, 0, "Data", "test.jpg"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 2")), 0, 0, "Data 2", "test2.jpg"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 3")), 0, 0, "Data 3", "test3.jpg")
                ];

            var dbContext = await GetDatabaseContext();
            var fakeProject = A.Fake<Project>();

            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            IEnumerable<ImageDto> fakeImages =
            [
                new()
                {
                    ImageId = 1,
                    EntityType = "Test",
                    EntityId = 1,
                    ImageUrl = "Test",
                    PublicId = "Test",
                    Created = DateTime.UtcNow
                },
                new()
                {
                    ImageId = 2,
                    EntityType = "Test2",
                    EntityId = 2,
                    ImageUrl = "Test2",
                    PublicId = "Test2",
                    Created = DateTime.UtcNow
                }
            ];

            var tupleResult = ("Success", fakeImages);

            A.CallTo(() => _image.AddImagesToExistingEntity(A<int>._, A<List<IFormFile>>._, A<string>._, A<int>._))
                .Returns(tupleResult);

            var result = await projectRepository.AddImagesToExistingProject(projectId, fakeIFormFileList);

            result.Should().BeEquivalentTo(tupleResult);
            result.Item2.Should().BeEquivalentTo(fakeImages);
            result.Item2.Should().HaveCountGreaterThan(1);
            result.Item2.Should().BeAssignableTo(typeof(IEnumerable<ImageDto>));
            result.status.Should().Be("Success").And.BeOfType<string>();
        }

        [Fact]
        public async void ProjectRepository_CreateProject_ReturnsSuccess()
        {
            int supervisorId = 1;
            string supervisorUsername = "test0";
            int companyId = 1;
            List<int> employees = [];

            List<IFormFile> fakeIFormFileList =
                [
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test")), 0, 0, "Data", "test.jpg"),
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 2")), 0, 0, "Data 2", "test2.jpg"),
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 3")), 0, 0, "Data 3", "test3.jpg")
                    ];

            var newProject = new Project
            {
                Name = "FakeName",
                Description = "FakeDescription",
                Created = DateTime.UtcNow,
                ProjectCreatorId = supervisorId,
                Priority = 1,
                CompanyId = companyId,
                ExpectedDeliveryDate = DateTime.UtcNow
            };

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.CreateProject(newProject, new EmployeeDto { EmployeeId = supervisorId, Username = supervisorUsername }, fakeIFormFileList, companyId, employees, false);

            result.Should().NotBeNull();
            result.Message.Should().Be("Project created successfully");
            result.Success.Should().BeTrue();
            result.Data.Should().BeOfType(typeof(int));
            result.Data.Should().NotBe(0);
        }

        [Fact]
        public async void ProjectRepository_CreateProject_ReturnsFailure()
        {
            int supervisorId = 1;
            string supervisorUsername = "test0";
            int companyId = 1;
            List<int> employees = [];

            List<IFormFile> fakeIFormFileList =
                [
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test")), 0, 0, "Data", "test.jpg"),
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 2")), 0, 0, "Data 2", "test2.jpg"),
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 3")), 0, 0, "Data 3", "test3.jpg")
                    ];

            var newProject = new Project
            {
                Name = "",
                Description = "",
                Created = DateTime.UtcNow,
                ProjectCreatorId = supervisorId,
                Priority = 1,
                CompanyId = companyId
            };

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.CreateProject(newProject, new EmployeeDto { EmployeeId = supervisorId, Username = supervisorUsername }, fakeIFormFileList, companyId, employees, false);

            result.Should().NotBeNull();
            result.Message.Should().Be("Project name and description are required");
            result.Success.Should().BeFalse();
            result.Data.Should().BeOfType(typeof(int));
            result.Data.Should().Be(0);
        }

        [Fact]
        public async void ProjectRepository_CreateProject_ReturnsSuccessWithErrors()
        {
            int supervisorId = 1;
            string supervisorUsername = "test0";
            int companyId = 1;
            List<int> employees = [4, 5, 7];

            List<IFormFile> fakeIFormFileList =
                [
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test")), 0, 0, "Data", "test.jpg"),
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 2")), 0, 0, "Data 2", "test2.jpg"),
                    new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 3")), 0, 0, "Data 3", "test3.jpg")
                    ];

            var newProject = new Project
            {
                Name = "FakeName",
                Description = "FakeDescription",
                Created = DateTime.UtcNow,
                ProjectCreatorId = supervisorId,
                Priority = 1,
                CompanyId = companyId,
                ExpectedDeliveryDate = DateTime.UtcNow
            };

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            A.CallTo(() => _workload.UpdateEmployeeCreatedProjects(A<int>._))
                .Returns(new OperationResult { Success = false, Message = "Error" });

            A.CallTo(() => _workload.UpdateEmployeeAssignedProjects(A < int[]>._))
                .Returns(new OperationResult { Success = false, Message = "Error2" });

            var result = await projectRepository.CreateProject(newProject, new EmployeeDto { EmployeeId = supervisorId, Username = supervisorUsername }, fakeIFormFileList, companyId, employees, false);

            result.Should().NotBeNull();
            result.Message.Should().Be("Project created successfully");
            result.Success.Should().BeTrue();
            result.Data.Should().BeOfType(typeof(int));
            result.Data.Should().NotBe(0);
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.Should().HaveCount(2);
            result.Errors[0].Should().Be("Failed to update the workload of the project creator. Error = Error");
            result.Errors[1].Should().Be("Failed to update the workload of the employees. Error = Error2");
        }

        [Fact]
        public async void ProjectRepository_DoesProjectExist_ReturnsTrue()
        {
            int projectId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.DoesProjectExist(projectId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void ProjectRepository_DoesProjectExist_ReturnsFalse()
        {
            int projectId = 100;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.DoesProjectExist(projectId);

            result.Should().BeFalse();
        }

        [Fact]
        public async void ProjectRepository_GetAllProjects_ReturnsAllProjects()
        {
            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var filterParams = A.Fake<FilterParams>();

            ICollection<ProjectDto> projectsCollection = [.. dbContext.Projects.Select(p => new ProjectDto
            {
                ProjectId = p.ProjectId
            })];

            var tupleResult = (projectsCollection, 1, 1);

            A.CallTo(() => _utility.GetAllEntities(A<FilterParams>._, A<Expression<Func<Project, ProjectDto>>>._))
                .Returns(tupleResult);

            var result = await projectRepository.GetAllProjects(filterParams);

            result.Should().BeOfType(typeof(DataCountPages<ProjectDto>));
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Data.Should().BeOfType(typeof(List<ProjectDto>));
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void ProjectRepository_GetProjectsByCompanyName_ReturnsProjects()
        {
            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            int companyId = 1;
            var filterParams = A.Fake<FilterParams>();
            filterParams.Page = 1;
            filterParams.PageSize = 10;

            Expression<Func<Project, bool>> fakeBoolExpression = project => true; // Just evaluate to true
            Expression<Func<Project, object>> fakeObjectExpression = project => project.Name;

            var tupleResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<Project>(
                A<int>._, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
                .Returns(tupleResult);

            var result = await projectRepository.GetProjectsByCompanyName(companyId, filterParams);

            result.Should().BeOfType(typeof(DataCountPages<ProjectDto>));
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Data.Should().BeOfType(typeof(List<ProjectDto>));
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void ProjectRepository_GetProjectsByCompanyName_ReturnsNoProjects()
        {
            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            int companyId = 100;
            var filterParams = A.Fake<FilterParams>();
            filterParams.Page = 1;
            filterParams.PageSize = 10;

            Expression<Func<Project, bool>> fakeBoolExpression = project => project.ProjectId == 777;
            Expression<Func<Project, object>> fakeObjectExpression = project => project.Name;

            var tupleResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<Project>(
                A<int>._, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
                .Returns(tupleResult);

            var result = await projectRepository.GetProjectsByCompanyName(companyId, filterParams);

            result.Should().BeOfType(typeof(DataCountPages<ProjectDto>));
            result.Data.Should().HaveCount(0);
            result.Data.Should().BeOfType(typeof(List<ProjectDto>));
            result.Count.Should().Be(0);
            result.Pages.Should().Be(0);
        }

        [Fact]
        public async void ProjectRepository_GetProjectById_ReturnsProject()
        {
            int projectId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.GetProjectById(projectId, 1);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(EntityParticipantOrOwnerDTO<ProjectDto>));
            result.Entity.Should().BeOfType(typeof(ProjectDto));
            result.Entity.ProjectId.Should().Be(projectId);
            result.Entity.Name.Should().NotBeNullOrEmpty().And.Be("Project 0");
            result.Entity.Creator.Should().BeOfType(typeof(EmployeeShowcaseDto));
            result.Entity.Creator.Username.Should().NotBeNullOrEmpty().And.Be("test0");
            result.Entity.Company.Should().BeOfType(typeof(CompanyShowcaseDto));
            result.Entity.Company.Name.Should().NotBeNullOrEmpty().And.Be("Company 1");
            result.Entity.EmployeeCount.Should().BeGreaterThanOrEqualTo(1);
            result.Entity.TasksCount.Should().Be(0);
            result.Entity.Team.Should().NotBeEmpty();
            foreach (var employee in result.Entity.Team)
            {
                employee.Should().NotBeNull();
                employee.Should().BeOfType(typeof(EmployeeShowcaseDto));
                employee.Username.Should().NotBeNullOrEmpty();
            }
            result.IsOwner.Should().BeTrue();
            result.IsParticipant.Should().BeFalse();
        }

        [Fact]
        public async void ProjectRepository_GetProjectById_ReturnsNoProject()
        {
            int projectId = 100;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.GetProjectById(projectId, 2);

            result.Should().BeNull();
        }

        [Fact]
        public async void ProjectRepository_GetProjectEntityById_ReturnsProject()
        {
            int projectId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.GetProjectEntityById(projectId);

            result.Should().BeOfType(typeof(Project));
            result.ProjectId.Should().Be(projectId);
            result.Name.Should().NotBeNullOrEmpty().And.Be("Project 0");
        }

        [Fact]
        public async void ProjectRepository_GetProjectEntityById_ReturnsNoProject()
        {
            int projectId = 100;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.GetProjectEntityById(projectId);

            result.Should().BeNull();
        }

        [Fact]
        public async void ProjectRepository_GetProjectsGroupedByCompany_ReturnsGroupedProjects()
        {
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();

            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var fakeFilterParams = A.Fake<FilterParams>();

            var result = await projectRepository.GetProjectsGroupedByCompany(fakeFilterParams, page, pageSize, 1);

            result.Data.Should().BeOfType<List<CompanyProjectGroup>>();
            result.Data.Should().NotBeEmpty();
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            foreach (var data in result.Data)
            {
                data.Projects.Should().NotBeEmpty();
                data.Projects.Should().HaveCountGreaterThanOrEqualTo(1);
                data.Count.Should().BeGreaterThanOrEqualTo(1);
                data.Pages.Should().BeGreaterThanOrEqualTo(1);
                data.Projects.Should().BeOfType<List<ProjectDto>>();
            }
        }

        [Fact]
        public async void ProjectRepository_SelectImages_ReturnsImageCollection()
        {
            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var fakeImages = dbContext.Images.ToList();

            var result = projectRepository.SelectImages(fakeImages);

            result.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeOfType(typeof(List<Image>));
        }

        [Fact]
        public async void ProjectRepository_GetProjectsByEmployeeUsername_ReturnsProjectsDto()
        {
            string username = "test1";
            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var filterParams = A.Fake<FilterParams>();

            var tupleEntitiesResult = (new List<int> { 1, 2, 3, 4 }, 1, 1);

            A.CallTo(() => _utility.GetEntitiesEmployeeCreatedOrParticipates<EmployeeProject, Project>(
                A<string>._, A<string>._, A<string>._, A<int>._, A<int>._))
                .Returns(tupleEntitiesResult);

            Expression<Func<Project, bool>> fakeBoolExpression = project => true; // Just evaluate to true
            Expression<Func<Project, object>> fakeObjectExpression = project => project.Name;

            var tupleExpressionsResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<Project>(
                null, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
                .Returns(tupleExpressionsResult);

            var result = await projectRepository.GetProjectsByEmployeeUsername(username, filterParams);

            result.Should().BeOfType(typeof(DataCountPages<ProjectDto>));
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Data.Should().BeOfType(typeof(List<ProjectDto>));
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void ProjectRepository_GetProjectsByEmployeeUsername_ReturnsNoProjectsDto()
        {
            string username = "test100";
            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var filterParams = A.Fake<FilterParams>();

            var tupleEntitiesResult = (new List<int> { 777, 7778, 67676, 12312312 }, 0, 0);

            A.CallTo(() => _utility.GetEntitiesEmployeeCreatedOrParticipates<EmployeeProject, Project>(
                               A<string>._, A<string>._, A<string>._, A<int>._, A<int>._))
                .Returns(tupleEntitiesResult);

            Expression<Func<Project, bool>> fakeBoolExpression = project => project.ProjectId == 777;
            Expression<Func<Project, object>> fakeObjectExpression = project => project.Name;

            var tupleExpressionsResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<Project>(
                null, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
                .Returns(tupleExpressionsResult);

            var result = await projectRepository.GetProjectsByEmployeeUsername(username, filterParams);

            result.Should().BeOfType(typeof(DataCountPages<ProjectDto>));
            result.Data.Should().HaveCount(0);
            result.Data.Should().BeOfType(typeof(List<ProjectDto>));
            result.Count.Should().Be(0);
            result.Pages.Should().Be(0);
        }

        [Fact]
        public async void ProjectRepository_GetProjectsShowcaseByEmployeeUsername_ReturnsProjectsShowcaseDto()
        {
            string username = "test1";
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var tupleEntitiesResult = (new List<int> { 1, 2, 3, 4 }, 1, 1);

            A.CallTo(() => _utility.GetEntitiesEmployeeCreatedOrParticipates<EmployeeProject, Project>(
                A<string>._, A<string>._, A<string>._, A<int>._, A<int>._)).Returns(tupleEntitiesResult);

            var result = await projectRepository.GetProjectsShowcaseByEmployeeUsername(username, page, pageSize);

            result.Should().BeOfType(typeof(DataCountPages<ProjectShowcaseDto>));
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Data.Should().BeOfType(typeof(List<ProjectShowcaseDto>));
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void ProjectRepository_GetProjectsShowcaseByEmployeeUsername_ReturnsNoProjectsShowcaseDto()
        {
            string username = "test100";
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var tupleEntitiesResult = (new List<int> { 777, 7778, 67676, 12312312 }, 0, 0);

            A.CallTo(() => _utility.GetEntitiesEmployeeCreatedOrParticipates<EmployeeProject, Project>(
                A<string>._, A<string>._, A<string>._, A<int>._, A<int>._)).Returns(tupleEntitiesResult);

            var result = await projectRepository.GetProjectsShowcaseByEmployeeUsername(username, page, pageSize);

            result.Should().BeOfType(typeof(DataCountPages<ProjectShowcaseDto>));
            result.Data.Should().HaveCount(0);
            result.Data.Should().BeOfType(typeof(List<ProjectShowcaseDto>));
            result.Count.Should().Be(0);
            result.Pages.Should().Be(0);
        }

        [Fact]
        public async void ProjectRepository_GetAllProjectsShowcase_ReturnsProjectsShowcaseDto()
        {
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.GetAllProjectsShowcase(page, pageSize);

            result.Should().BeOfType(typeof(DataCountPages<ProjectShowcaseDto>));
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Data.Should().BeOfType(typeof(List<ProjectShowcaseDto>));
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void ProjectRepository_IsParticipant_ReturnsTrue()
        {
            int projectId = 7;
            int employeeId = 7;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.IsParticipant(projectId, employeeId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void ProjectRepository_IsParticipant_ReturnsFalse()
        {
            int projectId = 1;
            int employeeId = 100;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.IsParticipant(projectId, employeeId);

            result.Should().BeFalse();
        }

        [Fact]
        public async void ProjectRepository_IsOwner_ReturnsTrue()
        {
            int projectId = 1;
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.IsOwner(projectId, employeeId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void ProjectRepository_IsOwner_ReturnsFalse()
        {
            int projectId = 1;
            int employeeId = 100;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.IsOwner(projectId, employeeId);

            result.Should().BeFalse();
        }

        [Fact]
        public async void ProjectRepository_GetProjectNameCreatorLifecyclePriorityAndTeam_ReturnsProjectSomeInfoDto()
        {
            int projectId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.GetProjectNameCreatorLifecyclePriorityAndTeam(projectId);

            result.Should().BeOfType(typeof(ProjectSomeInfoDto));
            result.ProjectId.Should().Be(projectId);
            result.Name.Should().NotBeNullOrEmpty().And.Be("Project 0");
            result.EmployeeCount.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void ProjectRepository_GetProjectNameCreatorLifecyclePriorityAndTeam_ReturnsNull()
        {
            int projectId = 100;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement); ;

            var result = await projectRepository.GetProjectNameCreatorLifecyclePriorityAndTeam(projectId);

            result.Should().BeNull();
        }

        [Fact]
        public async void ProjectRepository_GetProjectShowcase_ReturnsProject()
        {
            int projectId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.GetProjectShowcase(projectId);

            result.Should().BeOfType(typeof(ProjectShowcaseDto));
            result.ProjectId.Should().Be(projectId);
            result.Name.Should().NotBeNullOrEmpty().And.Be("Project 0");
        }

        [Fact]
        public async void ProjectRepository_SetProjectsFininishedBulk_ReturnsSuccess()
        {
            int[] projectIds = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.SetProjectsFininishedBulk(projectIds);

            result.Should().NotBeNull();
            result.Message.Should().Be("Projects finished successfully");
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async void ProjectRepository_SetProjectsFininishedBulk_ReturnsFailure()
        {
            int[] projectIds = [100, 200, 300, 400, 500, 600, 700, 800, 900, 1000];

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.SetProjectsFininishedBulk(projectIds);

            result.Should().NotBeNull();
            result.Message.Should().Be("No projects found");
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void ProjectRepository_SetProjectsFininishedBulk_ReturnsFailureAllProjectsAlreadyFinished()
        {
            int[] projectIds = [7, 8];

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.SetProjectsFininishedBulk(projectIds);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("All projects are already finished");
        }

        [Fact]
        public async void ProjectRepository_SetProjectsStartBulk_ReturnsSuccess()
        {
            int[] projectIds = [3, 4, 5, 6, 7, 8, 9, 10];

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.SetProjectsStartBulk(projectIds);

            result.Should().NotBeNull();
            result.Message.Should().Be("Projects started successfully");
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async void ProjectRepository_SetProjectsStartBulk_ReturnsSuccessButSomeProjectsAlreadyStarted()
        {
            int[] projectIds = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.SetProjectsStartBulk(projectIds);

            result.Should().NotBeNull();
            result.Message.Should().Be("Projects started successfully, however some projects were already started");
            result.Success.Should().BeTrue();
            result.Errors.Should().HaveCountGreaterThan(1);
            foreach (var error in result.Errors)
            {
                error.Should().NotBeNullOrEmpty();
                error.Should().BeOfType(typeof(string));
            }
            result.Errors[0].Should().Be("Project 1 is already started");
            result.Errors[1].Should().Be("Project 2 is already started");
        }

        [Fact]
        public async void ProjectRepository_SetProjectsStartBulk_ReturnsFailureProjectsNotFound()
        {
            int[] projectIds = [100, 200, 300, 400, 500, 600, 700, 800, 900, 1000];

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.SetProjectsStartBulk(projectIds);

            result.Should().NotBeNull();
            result.Message.Should().Be("No projects found");
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void ProjectRepository_SetProjectsStartBulk_ReturnsFailureAllProjectsAlreadyStarted()
        {
            int[] projectIds = [1, 2];

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.SetProjectsStartBulk(projectIds);

            result.Should().NotBeNull();
            result.Message.Should().Be("All projects are already started");
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void ProjectRepository_SetProjectStart_ReturnsSuccess()
        {
            int projectId = 5;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.SetProjectStart(projectId);

            result.Should().BeOfType(typeof(OperationResult));
            result.Message.Should().Be("Project started successfully");
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async void ProjectRepository_SetProjectStart_ReturnsFailureProjectNotFound()
        {
            int projectId = 100;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.SetProjectStart(projectId);

            result.Should().BeOfType(typeof(OperationResult));
            result.Message.Should().Be("Project not found");
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void ProjectRepository_SetProjectStart_ReturnsFailureProjectAlreadyStarted()
        {
            int projectId = 2;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.SetProjectStart(projectId);

            result.Should().BeOfType(typeof(OperationResult));
            result.Message.Should().Be("Project is already started");
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void ProjectRepository_SetProjectFinished_ReturnsSuccess()
        {
            int projectId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.SetProjectFinished(projectId);

            result.Should().BeOfType(typeof(OperationResult));
            result.Message.Should().Be("Project finished successfully");
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async void ProjectRepository_SetProjectFinished_ReturnsFailureProjectNotFound()
        {
            int projectId = 100;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.SetProjectFinished(projectId);

            result.Should().BeOfType(typeof(OperationResult));
            result.Message.Should().Be("Project not found");
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void ProjectRepository_SetProjectFinished_ReturnsFailureProjectAlreadyFinished()
        {
            int projectId = 7;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility, _workload, _timelineManagement, _notificationManagement);

            var result = await projectRepository.SetProjectFinished(projectId);

            result.Should().BeOfType(typeof(OperationResult));
            result.Message.Should().Be("Project is already finished");
            result.Success.Should().BeFalse();
        }
    }
}
