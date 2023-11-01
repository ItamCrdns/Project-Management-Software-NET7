using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IIssue
    {
        Task<Dictionary<string, object>> GetIssuesShowcaseByEmployeeUsername(string username, int page, int pageSize);
        Task<Dictionary<string, object>> GetAllIssuesShowcase(int page, int pageSize);
        Task<Dictionary<string, object>> GetAllIssues(int page, int pageSize);
        ICollection<IssueDto> IssueSelectQuery(ICollection<Issue> issues); 
    }
}
