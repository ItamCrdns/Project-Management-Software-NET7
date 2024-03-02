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
    }
}
