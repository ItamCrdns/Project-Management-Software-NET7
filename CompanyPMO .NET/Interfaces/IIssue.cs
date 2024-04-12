using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IIssue
    {
        Task<DataCountPages<IssueShowcaseDto>> GetIssuesShowcaseByEmployeeUsername(string username, int page, int pageSize);
        Task<DataCountPages<IssueShowcaseDto>> GetAllIssuesShowcase(int page, int pageSize);
        Task<DataCountPages<IssueDto>> GetAllIssues(FilterParams filterParams);
        ICollection<IssueDto> IssueSelectQuery(ICollection<Issue> issues); 
        Task<DataCountPages<IssueDto>> GetIssuesByTaskId(int taskId, FilterParams filterParams);
        Task<OperationResult<int>> CreateIssue(IssueDto issue, int employeeId, int taskId, bool shouldStartNow);
    }
}
