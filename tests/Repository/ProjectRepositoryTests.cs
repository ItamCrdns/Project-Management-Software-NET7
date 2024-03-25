using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
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
        public ProjectRepositoryTests()
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
                            Finished = DateTime.UtcNow,
                            ProjectCreatorId = (i % 2) + 1,
                            CompanyId = (i % 2) + 1, // Only company Ids one and two
                            Priority = i,
                            ExpectedDeliveryDate = DateTime.UtcNow.AddMinutes(5),
                            Lifecycle = $"Lifecycle {i}"
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
        public async void ProjectRepository_AddEmployeesToProject_ReturnsAddedEmployees()
        {
            int projectId = 1;
            List<int> fakeEmployeeIds = [1, 2, 3, 4, 5, 6, 7];

            var fakeEmployeeProject = A.Fake<EmployeeProject>();
            var fakeProject = A.Fake<Project>();
            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            IEnumerable<EmployeeShowcaseDto> fakeEmployeeDtos = new List<EmployeeShowcaseDto>
            {
                new() {
                    EmployeeId = 1,
                    Username = "Test",
                    ProfilePicture = "Test"
                },
                new()
                {
                    EmployeeId = 2,
                    Username = "Test2",
                    ProfilePicture = "Test2"
                }
            };

            var tupleResult = ("Success", fakeEmployeeDtos);

            A.CallTo(() => _utility.AddEmployeesToEntity<EmployeeProject, Project>
            (A<List<int>>._, A<string>._, A<int>._, A<Func<int, int, Task<bool>>>._))
                .Returns(tupleResult);

            var result = await projectRepository.AddEmployeesToProject(projectId, fakeEmployeeIds);

            result.Should().BeEquivalentTo(tupleResult);
            result.Item2.Should().BeEquivalentTo(fakeEmployeeDtos);
            result.Item2.Should().HaveCountGreaterThan(1);
            result.Item2.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            result.status.Should().Be("Success").And.BeOfType<string>();
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

            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            IEnumerable<ImageDto> fakeImages = new List<ImageDto>
            {
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
            };

            var tupleResult = ("Success", fakeImages);

            A.CallTo(() => _image.AddImagesToExistingEntity(A<int>._, A<List<IFormFile>>._, A<string>._, A<int>._))
                .Returns(tupleResult);

            var result = await projectRepository.AddImagesToExistingProject(projectId, fakeIFormFileList);

            result.Should().BeEquivalentTo(tupleResult);
            result.Item2.Should().BeEquivalentTo(fakeImages);
            result.Item2.Should().HaveCountGreaterThan(1);
            result.Item2.Should().BeOfType(typeof(List<ImageDto>));
            result.status.Should().Be("Success").And.BeOfType<string>();
        }

        [Fact]
        public async void ProjectRepository_CreateProject_ReturnsSuccess()
        {
            int supervisorId = 1;
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
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.CreateProject(newProject, supervisorId, fakeIFormFileList, companyId, employees, false);

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
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.CreateProject(newProject, supervisorId, fakeIFormFileList, companyId, employees, false);

            result.Should().NotBeNull();
            result.Message.Should().Be("Project name and description are required");
            result.Success.Should().BeFalse();
            result.Data.Should().BeOfType(typeof(int));
            result.Data.Should().Be(0);
        }

        [Fact]
        public async void ProjectRepository_DoesProjectExist_ReturnsTrue()
        {
            int projectId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.DoesProjectExist(projectId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void ProjectRepository_DoesProjectExist_ReturnsFalse()
        {
            int projectId = 100;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.DoesProjectExist(projectId);

            result.Should().BeFalse();
        }

        [Fact]
        public async void ProjectRepository_GetAllProjects_ReturnsAllProjects()
        {
            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var filterParams = A.Fake<FilterParams>();

            var tupleResult = (dbContext.Projects.ToList(), 1, 1);

            A.CallTo(() => _utility.GetAllEntities<Project>(A<FilterParams>._, A<List<string>>._))
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
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            int companyId = 1;
            var filterParams = A.Fake<FilterParams>();
            filterParams.Page = 1;
            filterParams.PageSize = 10;

            Expression<Func<Project, bool>> fakeBoolExpression = project => true; // Just evaluate to true
            Expression<Func<Project, object>> fakeObjectExpression = project => project.Name;

            var tupleResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<Project>(
                A<int>._, A<IEnumerable<int>>._, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
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
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            int companyId = 100;
            var filterParams = A.Fake<FilterParams>();
            filterParams.Page = 1;
            filterParams.PageSize = 10;

            Expression<Func<Project, bool>> fakeBoolExpression = project => project.ProjectId == 777;
            Expression<Func<Project, object>> fakeObjectExpression = project => project.Name;

            var tupleResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<Project>(
                A<int>._, A<IEnumerable<int>>._, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
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
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

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
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.GetProjectById(projectId, 2);

            result.Should().BeNull();
        }

        [Fact]
        public async void ProjectRepository_GetProjectEntityById_ReturnsProject()
        {
            int projectId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

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
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.GetProjectEntityById(projectId);

            result.Should().BeNull();
        }

        [Fact]
        public async void ProjectRepository_GetProjectsGroupedByCompany_ReturnsGroupedProjects()
        {
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();

            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var fakeFilterParams = A.Fake<FilterParams>();

            var result = await projectRepository.GetProjectsGroupedByCompany(fakeFilterParams, page, pageSize,1 );

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
        public async void ProjectRepository_IsEmployeeAlreadyInProject_ReturnsTrue()
        {
            int employeeId = 7;
            int projectId = 7;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.IsEmployeeAlreadyInProject(employeeId, projectId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void ProjectRepository_IsEmployeeAlreadyInProject_ReturnsFalse()
        {
            int employeeId = 2;
            int projectId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.IsEmployeeAlreadyInProject(employeeId, projectId);

            result.Should().BeFalse();
        }

        [Fact]
        public async void ProjectRepository_ProjectSelectQuery_ReturnsProjectDtoCollection()
        {
            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var fakeProjects = dbContext.Projects.ToList();

            var result = projectRepository.ProjectSelectQuery(fakeProjects);

            result.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeOfType(typeof(List<ProjectDto>));
        }

        [Fact]
        public async void ProjectRepository_SelectImages_ReturnsImageCollection()
        {
            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var fakeImages = dbContext.Images.ToList();

            var result = projectRepository.SelectImages(fakeImages);

            result.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeOfType(typeof(List<Image>));
        }

        [Fact]
        public async void ProjectRepository_SetProjectFinalized_ReturnsTrue()
        {
            int projectId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.SetProjectFinalized(projectId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void ProjectRepository_SetProjectFinalized_ReturnsFalse()
        {
            int projectId = 100;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.SetProjectFinalized(projectId);

            result.Should().BeFalse();
        }

        //[Fact]
        //public async void ProjectRepository_UpdateProject_ReturnsProjectHasUpdated()
        //{
        // Not implemented yet
        //}

        [Fact]
        public async void ProjectRepository_GetProjectsByEmployeeUsername_ReturnsProjectsDto()
        {
            string username = "test1";
            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var filterParams = A.Fake<FilterParams>();

            var tupleEntitiesResult = (new List<int> { 1, 2, 3, 4 }, 1, 1);

            A.CallTo(() => _utility.GetEntitiesEmployeeCreatedOrParticipates<EmployeeProject, Project>(
                A<string>._, A<string>._, A<string>._, A<int>._, A<int>._))
                .Returns(tupleEntitiesResult);

            Expression<Func<Project, bool>> fakeBoolExpression = project => true; // Just evaluate to true
            Expression<Func<Project, object>> fakeObjectExpression = project => project.Name;

            var tupleExpressionsResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<Project>(
                null, A<IEnumerable<int>>._, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
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
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var filterParams = A.Fake<FilterParams>();

            var tupleEntitiesResult = (new List<int> { 777, 7778, 67676, 12312312 }, 0, 0);

            A.CallTo(() => _utility.GetEntitiesEmployeeCreatedOrParticipates<EmployeeProject, Project>(
                               A<string>._, A<string>._, A<string>._, A<int>._, A<int>._))
                .Returns(tupleEntitiesResult);

            Expression<Func<Project, bool>> fakeBoolExpression = project => project.ProjectId == 777;
            Expression<Func<Project, object>> fakeObjectExpression = project => project.Name;

            var tupleExpressionsResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<Project>(
                               null, A<IEnumerable<int>>._, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
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
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

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
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

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
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

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
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.IsParticipant(projectId, employeeId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void ProjectRepository_IsParticipant_ReturnsFalse()
        {
            int projectId = 1;
            int employeeId = 100;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.IsParticipant(projectId, employeeId);

            result.Should().BeFalse(); 
        }

        [Fact]
        public async void ProjectRepository_IsOwner_ReturnsTrue()
        {
            int projectId = 1;
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.IsOwner(projectId, employeeId);

            result.Should().BeTrue();
        }

        [Fact]
        public async void ProjectRepository_IsOwner_ReturnsFalse()
        {
            int projectId = 1;
            int employeeId = 100;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.IsOwner(projectId, employeeId);

            result.Should().BeFalse();
        }

        [Fact]
        public async void ProjectRepository_GetProjectNameCreatorLifecyclePriorityAndTeam_ReturnsProjectSomeInfoDto()
        {
            int projectId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

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
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.GetProjectNameCreatorLifecyclePriorityAndTeam(projectId);

            result.Should().BeNull();
        }

        [Fact]
        public async void ProjectRepository_GetProjectShowcase_ReturnsProject()
        {
            int projectId = 1;

            var dbContext = await GetDatabaseContext();
            var projectRepository = new ProjectRepository(dbContext, _image, _utility);

            var result = await projectRepository.GetProjectShowcase(projectId);

            result.Should().BeOfType(typeof(ProjectShowcaseDto));
            result.ProjectId.Should().Be(projectId);
            result.Name.Should().NotBeNullOrEmpty().And.Be("Project 0");

        }
    }
}
