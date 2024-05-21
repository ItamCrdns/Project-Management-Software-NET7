using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces.Issue_interfaces
{
    public interface IIssueTaskQueries
    {
        // Fully implemented in Task Controller
        Task<DataCountPages<IssueDto>> GetIssuesByTaskId(int taskId, FilterParams filterParams);
    }
}
