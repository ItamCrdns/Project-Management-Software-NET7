using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Tests.Repository
{
    public class IssueRepositoryTests
    {
        private readonly IUtility _utility;
        public IssueRepositoryTests()
        {
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
                            Finished = DateTime.Now.AddHours(1),
                            IssueCreatorId = 1,
                            TaskId = (i % 3) + 1
                        });
                }
            }

            if (!await dbContext.Tasks.AnyAsync())
            {
                for (int i = 1; i < 4; i++)
                {
                    dbContext.Tasks.Add(
                        new CompanyPMO_.NET.Models.Task
                        {
                            Name = $"Task {i}",
                            Description = $"Description {i}",
                            Created = DateTime.Now,
                            StartedWorking = DateTime.Now.AddMinutes(15),
                            Finished = DateTime.Now.AddHours(1),
                            TaskCreatorId = 1,
                            ProjectId = 1
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

            if (!await dbContext.Employees.AnyAsync())
            {
                for (int i = 2; i < 11; i++)
                {
                    dbContext.EmployeeIssues.Add(
                        new EmployeeIssue
                        {
                            EmployeeId = i,
                            IssueId = i
                        });

                    dbContext.EmployeeIssues.Add(new EmployeeIssue
                    {
                        EmployeeId = i,
                        IssueId = 1
                    });
                }
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async void IssueRepository_GetAllIssues_ReturnsAllIssues()
        {
            var dbContext = await GetDatabaseContext();
            var issueRepository = new IssueRepository(dbContext, _utility);

            var filterParams = A.Fake<FilterParams>();

            var dbContextIssues = await dbContext.Issues.ToListAsync();

            ICollection<Issue> issuesCollection = dbContextIssues.ToList();

            var tupleResult = (issuesCollection, 10, 1);

            A.CallTo(() => _utility.GetAllEntities<Issue>(A<FilterParams>._, A<List<string>>._))
                .Returns(tupleResult);

            var result = await issueRepository.GetAllIssues(filterParams);

            result.Should().BeOfType<DataCountPages<IssueDto>>();
            result.Data.Should().BeOfType<List<IssueDto>>();
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
            result.Should().NotBeNull();
        }

        [Fact]
        public async void IssueRepository_GetAllIssuesShowcase_ReturnsAllIssuesShowcase()
        {
            var dbContext = await GetDatabaseContext();
            var issueRepository = new IssueRepository(dbContext, _utility);

            int page = 1;
            int pageSize = 5;

            var result = await issueRepository.GetAllIssuesShowcase(page, pageSize);

            result.Should().BeOfType<DataCountPages<IssueShowcaseDto>>();
            result.Data.Should().BeOfType<List<IssueShowcaseDto>>();
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
            result.Should().NotBeNull();
        }

        [Fact]
        public async void IssueRepository_GetIssuesShowcaseByEmployeeUsername_ReturnsIssuesShowcaseByEmployeeUsername()
        {
            var dbContext = await GetDatabaseContext();
            var issueRepository = new IssueRepository(dbContext, _utility);

            string username = "test0";
            int page = 1;
            int pageSize = 5;

            IEnumerable<int> issuesIds = new List<int> { 1, 2, 3, 4, 5 };

            var tupleResult = (issuesIds, 5, 1);

            A.CallTo(() => _utility.GetEntitiesEmployeeCreatedOrParticipates<EmployeeIssue, Issue>(A<string>._, A<string>._, A<string>._, A<int?>._, A<int?>._))
                .Returns(tupleResult);

            var result = await issueRepository.GetIssuesShowcaseByEmployeeUsername(username, page, pageSize);

            result.Should().BeOfType<DataCountPages<IssueShowcaseDto>>();
            result.Data.Should().BeOfType<List<IssueShowcaseDto>>();
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
            result.Should().NotBeNull();
        }

        [Fact]
        public async void IssueRepository_GetIssuesShowcaseByEmployeeUsername_ReturnsNoIssuesShowcase()
        {
            var dbContext = await GetDatabaseContext();
            var issueRepository = new IssueRepository(dbContext, _utility);

            string username = "test300";
            int page = 1;
            int pageSize = 5;

            IEnumerable<int> issuesIds = new List<int> { };

            var tupleResult = (issuesIds, 0, 0);

            A.CallTo(() => _utility.GetEntitiesEmployeeCreatedOrParticipates<EmployeeIssue, Issue>(A<string>._, A<string>._, A<string>._, A<int?>._, A<int?>._))
                .Returns(tupleResult);

            var result = await issueRepository.GetIssuesShowcaseByEmployeeUsername(username, page, pageSize);

            result.Should().BeOfType<DataCountPages<IssueShowcaseDto>>();
            result.Data.Should().BeOfType<List<IssueShowcaseDto>>();
            result.Data.Should().HaveCount(0);
            result.Count.Should().BeGreaterThanOrEqualTo(0);
            result.Pages.Should().BeGreaterThanOrEqualTo(0);
            result.Should().NotBeNull();
        }

        [Fact]
        public async void IssueRepository_GetIssuesByTaskId_ReturnsIssues()
        {
            int taskId = 1;

            var fakeFilterParams = A.Fake<FilterParams>();

            var dbContext = await GetDatabaseContext();

            var issueRepository = new IssueRepository(dbContext, _utility);

            int[] issuesIds = [1, 2, 3];

            var tupleResult = (issuesIds, 3, 1);

            A.CallTo(() => _utility.GetEntitiesByEntityId<Issue>(
                A<int>._,
                A<string>._,
                A<string>._,
                A<int>._,
                A<int>._))
                .Returns(tupleResult);

            Expression<Func<Issue, bool>> fakeBoolExpression = x => true; // Just evaluate to true
            Expression<Func<Issue, object>> fakeObjectExpression = x => x.Name;

            var tupleExpressionsResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<Issue>(
                A<int>._, A<IEnumerable<int>>._, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
                .Returns(tupleExpressionsResult);

            var result = await issueRepository.GetIssuesByTaskId(taskId, fakeFilterParams);

            result.Should().BeOfType<DataCountPages<IssueDto>>();
            result.Data.Should().BeOfType<List<IssueDto>>();
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
            result.Should().NotBeNull();
        }

        [Fact]
        public async void IssueRepository_GetIssuesByTaskId_ReturnsNoIssues()
        {
            int taskId = 9999;

            var fakeFilterParams = A.Fake<FilterParams>();

            var dbContext = await GetDatabaseContext();

            var issueRepository = new IssueRepository(dbContext, _utility);

            int[] issuesIds = [];

            var tupleResult = (issuesIds, 0, 0);

            A.CallTo(() => _utility.GetEntitiesByEntityId<Issue>(
                A<int>._,
                A<string>._,
                A<string>._,
                A<int>._,
                A<int>._))
                .Returns(tupleResult);

            Expression<Func<Issue, bool>> fakeBoolExpression = x => x.Name == "DOES NOT EXIST"; // Just evaluate to true
            Expression<Func<Issue, object>> fakeObjectExpression = x => x.Name;

            var tupleExpressionsResult = (fakeBoolExpression, fakeObjectExpression);

            A.CallTo(() => _utility.BuildWhereAndOrderByExpressions<Issue>(
                A<int>._, A<IEnumerable<int>>._, A<string>._, A<string>._, A<string>._, A<FilterParams>._))
                .Returns(tupleExpressionsResult);

            var result = await issueRepository.GetIssuesByTaskId(taskId, fakeFilterParams);

            result.Should().BeOfType<DataCountPages<IssueDto>>();
            result.Data.Should().BeOfType<List<IssueDto>>();
            result.Data.Should().HaveCount(0);
            result.Count.Should().BeGreaterThanOrEqualTo(0);
            result.Pages.Should().BeGreaterThanOrEqualTo(0);
            result.Should().NotBeNull();
        }

        [Fact]
        public async void IssueRepository_CreateIssue_ReturnsSuccess()
        {
            int supervisorId = 1;
            int taskId = 1;

            IssueDto newIssues = new()
            {
                Name = "FakeName",
                Description = "FakeDescription",
                Created = DateTime.Now,
                StartedWorking = DateTime.Now.AddMinutes(15),
                ExpectedDeliveryDate = DateTime.Now.AddHours(1),
            };

            var dbContext = await GetDatabaseContext();

            var issueRepository = new IssueRepository(dbContext, _utility);

            var result = await issueRepository.CreateIssue(newIssues, supervisorId, taskId, true);

            result.Should().BeOfType<OperationResult<int>>();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Issue created successfully");
        }

        [Fact]
        public async void IssueRepository_CreateIssue_ReturnsFailure()
        {
            int supervisorId = 1;
            int taskId = 1;

            IssueDto newIssues = new()
            {
                Name = "",
                Description = "",
                Created = DateTime.Now,
                StartedWorking = DateTime.Now.AddMinutes(15),
                ExpectedDeliveryDate = DateTime.Now.AddHours(1),
            };

            var dbContext = await GetDatabaseContext();

            var issueRepository = new IssueRepository(dbContext, _utility);

            var result = await issueRepository.CreateIssue(newIssues, supervisorId, taskId, false);
            
            result.Should().BeOfType<OperationResult<int>>();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Issue name and description are required");
        }

        [Fact]
        public async void IssueRepository_GetIssueById_ReturnsIssue()
        {
            int issueId = 1;
            int taskId = 1;
            int projectId = 1;
            int userId = 1;

            var dbContext = await GetDatabaseContext();

            var issueRepository = new IssueRepository(dbContext, _utility);

            var result = await issueRepository.GetIssueById(issueId, taskId, projectId, userId);

            result.Should().NotBeNull();
            result.Should().BeOfType<EntityParticipantOrOwnerDTO<IssueDto>>();
            result.Entity.Should().NotBeNull();
            result.Entity.Should().BeOfType<IssueDto>();
            result.Entity.IssueCreator.Should().NotBeNull();
            result.Entity.IssueCreator.Should().BeOfType<EmployeeShowcaseDto>();
            result.Entity.IssueCreator.Username.Should().NotBeNullOrEmpty();
            result.Entity.IssueCreator.Username.Should().NotBeNullOrWhiteSpace();
            result.Entity.Task.Should().NotBeNull();
            result.Entity.Task.Should().BeOfType<TaskShowcaseDto>();
            result.Entity.Task.Name.Should().NotBeNullOrEmpty();
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
    }
}
