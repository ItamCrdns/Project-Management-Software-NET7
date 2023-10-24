using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Dictionary<string, object>> GetIssuesShowcaseByEmployeeUsername(string username, int page, int pageSize)
        {
            // Get all the issues Ids of all the employees that have the issue but the creator
            var (issuesIds, _, _) = await _utilityService.GetEntitiesByEmployeeUsername<EmployeeIssue>(username, "IssueId", null, null);

            // The EmployeeIssue junction table does not include the task creator
            int taskCreatorId = await _context.Employees
                .Where(u => u.Username.Equals(username))
                .Select(i => i.EmployeeId)
                .FirstOrDefaultAsync();

            int toSkip = (page - 1) * pageSize;

            int totalIssuesCount = await _context.Issues
                .Where(i => issuesIds.Contains(i.IssueId) || i.IssueCreator.Equals(taskCreatorId))
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalIssuesCount / pageSize);

            ICollection<IssueShowcaseDto> issues = await _context.Issues
                .Where(i => issuesIds.Contains(i.IssueId) || i.IssueCreator.Equals(taskCreatorId))
                .Select(i => new IssueShowcaseDto
                {
                    IssueId = i.IssueId,
                    Name = i.Name,
                    IsCreator = i.IssueCreator.Equals(taskCreatorId) // Used to identify if the requested user is the creator of the issue
                })
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            var result = new Dictionary<string, object>
            {
                { "data", issues },
                { "count", totalIssuesCount },
                { "pages", totalPages }
            };

            return result;
        }
    }
}
