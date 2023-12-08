using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

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
                 .UseInMemoryDatabase(databaseName: "CompanyPMO")
                 .Options;

                return options;
            }
        }

        private static async Task<ApplicationDbContext> GetDatabaseContext()
        {
            var dbContext = new ApplicationDbContext(CreateNewContextOptions);
            dbContext.Database.EnsureCreated();

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
                            Fixed = DateTime.Now.AddHours(1),
                            IssueCreatorId = 1,
                            TaskId = (i % 3) + 1
                        });
                }
            }

            if (!await dbContext.Tasks.AnyAsync())
            {
                for (int i = 1 ; i < 4; i++)
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

            if (!await dbContext.Employees.AnyAsync())
            {
                for (int i = 1; i < 11; i++)
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

            result.Should().BeOfType<DataCountAndPagesizeDto<IEnumerable<IssueDto>>>();
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

            result.Should().BeOfType<DataCountAndPagesizeDto<IEnumerable<IssueShowcaseDto>>>();
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

            result.Should().BeOfType<DataCountAndPagesizeDto<ICollection<IssueShowcaseDto>>>();
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

            result.Should().BeOfType<DataCountAndPagesizeDto<ICollection<IssueShowcaseDto>>>();
            result.Data.Should().BeOfType<List<IssueShowcaseDto>>();
            result.Data.Should().HaveCount(0);
            result.Count.Should().BeGreaterThanOrEqualTo(0);
            result.Pages.Should().BeGreaterThanOrEqualTo(0);
            result.Should().NotBeNull();
        }
    }
}
