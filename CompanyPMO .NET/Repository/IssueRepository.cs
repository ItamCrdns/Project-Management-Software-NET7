using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace CompanyPMO_.NET.Repository
{
    public class IssueRepository : IIssue
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtility _utilityService;

        public IssueRepository(ApplicationDbContext context, IUtility utilityService)
        {
            _context = context;
            _utilityService = utilityService;
        }

        public async Task<DataCountAndPagesizeDto<IEnumerable<IssueDto>>> GetAllIssues(FilterParams filterParams)
        {
            List<string> navProperties = new() { "IssueCreator", "Employees", "Task" };

            var (issues, totalIssuesCount, totalPages) = await _utilityService.GetAllEntities<Issue>(filterParams, navProperties);

            var issueDtos = IssueSelectQuery(issues);

            var result = new DataCountAndPagesizeDto<IEnumerable<IssueDto>>
            {
                Data = issueDtos,
                Count = totalIssuesCount,
                Pages = totalPages
            };

            return result;
        }

        public async Task<Dictionary<string, object>> GetAllIssuesShowcase(int page, int pageSize)
        {
            int toSkip = (page - 1) * pageSize;
            IEnumerable<IssueShowcaseDto> issues = await _context.Issues
                .OrderByDescending(i => i.Created)
                .Select(i => new IssueShowcaseDto
                {
                    IssueId = i.IssueId,
                    Name = i.Name
                })
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            int totalIssuesCount = await _context.Issues.CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalIssuesCount / pageSize);

            var result = new Dictionary<string, object>
            {
                { "data", issues },
                { "count", totalIssuesCount },
                { "pages", totalPages }
            };

            return result;
        }

        public async Task<Dictionary<string, object>> GetIssuesShowcaseByEmployeeUsername(string username, int page, int pageSize)
        {
            var (issuesIds, totalIssuesCount, totalPages) = await _utilityService.GetEntitiesEmployeeCreatedOrParticipates<EmployeeIssue, Issue>(username, "IssueCreatorId", "IssueId", page, pageSize);

            ICollection<IssueShowcaseDto> issues = await _context.Issues
                .Where(i => issuesIds.Contains(i.IssueId))
                .Select(i => new IssueShowcaseDto
                {
                    IssueId = i.IssueId,
                    Name = i.Name,
                    //IsCreator = i.IssueCreator.Equals(issueCreatorId) // Used to identify if the requested user is the creator of the issue
                })
                .ToListAsync();

            var result = new Dictionary<string, object>
            {
                { "data", issues },
                { "count", totalIssuesCount },
                { "pages", totalPages }
            };

            return result;
        }

        public ICollection<IssueDto> IssueSelectQuery(ICollection<Issue> issues)
        {
            var issueDtos = issues.Select(issue => new IssueDto
            {
                IssueId = issue.IssueId,
                Name = issue.Name,
                Created = issue.Created,
                Employees = issue.Employees.Select(employee => new EmployeeShowcaseDto
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    ProfilePicture = employee.ProfilePicture
                }).ToList(),
                IssueCreator = new EmployeeShowcaseDto
                {
                    EmployeeId = issue.IssueCreator.EmployeeId,
                    Username = issue.IssueCreator.Username,
                    ProfilePicture = issue.IssueCreator.ProfilePicture
                },
                Task = new TaskShowcaseDto
                {
                    TaskId = issue.Task.TaskId,
                    Name = issue.Task.Name
                }
            }).ToList();

            return issueDtos;
        }
    }
}
