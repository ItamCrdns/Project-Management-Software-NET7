using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IIssue
    {
        // Fully implemented in Issue Controller
        Task<DataCountPages<IssueShowcaseDto>> GetAllIssuesShowcase(int page, int pageSize);
        Task<DataCountPages<IssueDto>> GetAllIssues(FilterParams filterParams);
        Task<EntityParticipantOrOwnerDTO<IssueDto>> GetIssueById(int issueId, int taskId, int projectId, int userId);
    }
}
