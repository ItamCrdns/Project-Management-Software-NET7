using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IIssue
    {
        Task<DataCountAndPagesizeDto<ICollection<IssueShowcaseDto>>> GetIssuesShowcaseByEmployeeUsername(string username, int page, int pageSize);
        Task<DataCountAndPagesizeDto<IEnumerable<IssueShowcaseDto>>> GetAllIssuesShowcase(int page, int pageSize);
        Task<DataCountAndPagesizeDto<IEnumerable<IssueDto>>> GetAllIssues(FilterParams filterParams);
        ICollection<IssueDto> IssueSelectQuery(ICollection<Issue> issues); 
    }
}
