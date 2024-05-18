using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;
using System.Linq.Expressions;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IIssue
    {
        Task<DataCountPages<IssueShowcaseDto>> GetIssuesShowcaseByEmployeeUsername(string username, int page, int pageSize);
        Task<DataCountPages<IssueShowcaseDto>> GetAllIssuesShowcase(int page, int pageSize);
        Task<DataCountPages<IssueDto>> GetAllIssues(FilterParams filterParams);
        Expression<Func<Issue, IssueDto>> GetIssuePredicate();
        Task<DataCountPages<IssueDto>> GetIssuesByTaskId(int taskId, FilterParams filterParams);
        Task<OperationResult<int>> CreateIssue(IssueDto issue, int employeeId, int taskId, bool shouldStartNow);
        Task<EntityParticipantOrOwnerDTO<IssueDto>> GetIssueById(int issueId, int taskId, int projectId, int userId);
    }
}
