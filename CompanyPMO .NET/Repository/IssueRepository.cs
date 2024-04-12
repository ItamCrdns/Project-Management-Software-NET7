using CompanyPMO_.NET.Common;
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

        public async Task<OperationResult<int>> CreateIssue(IssueDto issue, int employeeId, int taskId, bool shouldStartNow)
        {
            if (string.IsNullOrWhiteSpace(issue.Name) || string.IsNullOrWhiteSpace(issue.Description))
            {
                return new OperationResult<int>
                {
                    Success = false,
                    Message = "Issue name and description are required",
                    Data = 0
                };
            }

            var task = await _context.Tasks.FindAsync(taskId);

            Issue newIssue = new()
            {
                Name = issue.Name,
                Description = issue.Description,
                Created = DateTime.UtcNow,
                TaskId = taskId,
                IssueCreatorId = employeeId,
                ExpectedDeliveryDate = issue.ExpectedDeliveryDate,
                StartedWorking = shouldStartNow ? DateTime.UtcNow : null
            };

            _context.Add(newIssue);

            task.LatestIssueCreation = DateTime.UtcNow;
            _context.Update(task);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected is 0)
            {
                return new OperationResult<int>
                {
                    Success = false,
                    Message = "Failed to create issue",
                    Data = 0
                };
            }

            return new OperationResult<int>
            {
                Success = true,
                Message = "Issue created successfully",
                Data = newIssue.IssueId
            };
        }

        public async Task<DataCountPages<IssueDto>> GetAllIssues(FilterParams filterParams)
        {
            List<string> navProperties = new() { "IssueCreator", "Employees", "Task" };

            var (issues, totalIssuesCount, totalPages) = await _utilityService.GetAllEntities<Issue>(filterParams, navProperties);

            var issueDtos = IssueSelectQuery(issues);

            return new DataCountPages<IssueDto>
            {
                Data = issueDtos,
                Count = totalIssuesCount,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<IssueShowcaseDto>> GetAllIssuesShowcase(int page, int pageSize)
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

            return new DataCountPages<IssueShowcaseDto>
            {
                Data = issues,
                Count = totalIssuesCount,
                Pages = totalPages
            }; ;
        }

        public async Task<DataCountPages<IssueDto>> GetIssuesByTaskId(int taskId, FilterParams filterParams)
        {
            var (issueIds, totalIssuesCount, totalPages) = await _utilityService.GetEntitiesByEntityId<Issue>(taskId, "TaskId", "IssueId", filterParams.Page, filterParams.PageSize);

            var (whereExpression, orderByExpression) = _utilityService.BuildWhereAndOrderByExpressions<Issue>(taskId, issueIds, "IssueId", "TaskId", "Created", filterParams);

            bool shallOrderAscending = filterParams.Sort is not null && filterParams.Sort.Equals("ascending");
            bool shallOrderDescending = filterParams.Sort is not null && filterParams.Sort.Equals("descending");

            List<IssueDto> issues = new();

            if (shallOrderAscending)
            {
                issues = await _context.Issues
                    .Where(whereExpression)
                    .OrderBy(orderByExpression)
                    .Select(x => new IssueDto
                    {
                        IssueId = x.IssueId,
                        Name = x.Name,
                        Created = x.Created,
                        Employees = x.Employees.Select(e => new EmployeeShowcaseDto
                        {
                            EmployeeId = e.EmployeeId,
                            Username = e.Username,
                            ProfilePicture = e.ProfilePicture
                        }).ToList(),
                        IssueCreator = new EmployeeShowcaseDto
                        {
                            EmployeeId = x.IssueCreator.EmployeeId,
                            Username = x.IssueCreator.Username,
                            ProfilePicture = x.IssueCreator.ProfilePicture
                        },
                        Task = new TaskShowcaseDto
                        {
                            TaskId = x.Task.TaskId,
                            Name = x.Task.Name
                        }
                    })
                    .ToListAsync();
            }
            else if (shallOrderDescending || (!shallOrderAscending && !shallOrderDescending))
            {
                issues = await _context.Issues
                    .Where(whereExpression)
                    .OrderByDescending(orderByExpression)
                    .Select(x => new IssueDto
                    {
                        IssueId = x.IssueId,
                        Name = x.Name,
                        Created = x.Created,
                        Employees = x.Employees.Select(e => new EmployeeShowcaseDto
                        {
                            EmployeeId = e.EmployeeId,
                            Username = e.Username,
                            ProfilePicture = e.ProfilePicture
                        }).ToList(),
                        IssueCreator = new EmployeeShowcaseDto
                        {
                            EmployeeId = x.IssueCreator.EmployeeId,
                            Username = x.IssueCreator.Username,
                            ProfilePicture = x.IssueCreator.ProfilePicture
                        },
                        Task = new TaskShowcaseDto
                        {
                            TaskId = x.Task.TaskId,
                            Name = x.Task.Name
                        }
                    })
                    .ToListAsync();
            }

            return new DataCountPages<IssueDto>
            {
                Data = issues,
                Count = totalIssuesCount,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<IssueShowcaseDto>> GetIssuesShowcaseByEmployeeUsername(string username, int page, int pageSize)
        {
            var (issuesIds, totalIssuesCount, totalPages) = await _utilityService.GetEntitiesEmployeeCreatedOrParticipates<EmployeeIssue, Issue>(username, "IssueCreatorId", "IssueId", page, pageSize);

            ICollection<IssueShowcaseDto> issues = await _context.Issues
                .Where(i => issuesIds.Contains(i.IssueId))
                .Select(i => new IssueShowcaseDto
                {
                    IssueId = i.IssueId,
                    Name = i.Name
                })
                .ToListAsync();

            return new DataCountPages<IssueShowcaseDto>
            {
                Data = issues,
                Count = totalIssuesCount,
                Pages = totalPages
            };
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
