using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces.Issue_interfaces
{
    public interface IIssueManagement
    {
        // Fully implemented in Issue Management Controller
        Task<OperationResult<int>> CreateIssue(IssueDto issue, int employeeId, int taskId, bool shouldStartNow);
    }
}
