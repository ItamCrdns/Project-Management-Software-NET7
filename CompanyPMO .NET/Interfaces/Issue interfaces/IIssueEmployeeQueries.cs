using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces.Issue_interfaces
{
    public interface IIssueEmployeeQueries
    {
        // Fully implemented in Employee Controller
        Task<DataCountPages<IssueShowcaseDto>> GetIssuesShowcaseByEmployeeUsername(string username, int page, int pageSize);
    }
}
