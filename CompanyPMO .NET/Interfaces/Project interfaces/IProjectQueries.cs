using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces.Project_interfaces
{
    public interface IProjectQueries
    {
        // Fully implemented in Project Controller
        Task<DataCountPages<ProjectDto>> GetAllProjects(FilterParams filterParams);
        Task<DataCountPages<ProjectShowcaseDto>> GetAllProjectsShowcase(int page, int pageSize);
        Task<DataCountPages<CompanyProjectGroup>> GetProjectsGroupedByCompany(FilterParams filterParams, int projectsPage, int projectsPageSize, int employeeId);
        Task<EntityParticipantOrOwnerDTO<ProjectDto>> GetProjectById(int projectId, int userId);
        Task<ProjectSomeInfoDto> GetProjectNameCreatorLifecyclePriorityAndTeam(int projectId);
        Task<ProjectShowcaseDto> GetProjectShowcase(int projectId);
        Task<bool> IsParticipant(int projectId, int employeeId); // Used for checking if the employee is a participant in the project
        Task<bool> IsOwner(int projectId, int employeeId); // Used for checking if the employee is the owner of the project
    }
}
