using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IProject
    {
        Task<int> CreateProject(Project project, int employeeSupervisorId, List<IFormFile>? images, int companyId, List<int>? employees);
        Task<(bool updated, ProjectDto)> UpdateProject(int employeeId, int projectId, ProjectDto projectDto, List<IFormFile>? images);
        Task<ProjectDto> GetProjectById(int projectId);
        Task<Project> GetProjectEntityById(int projectId);
        Task<IEnumerable<ProjectDto>> GetProjectsByCompanyName(int companyId, int page, int pageSize);
        Task<bool> SetProjectFinalized(int projectId);
        Task<bool> DoesProjectExist(int projectId);
        Task<(string status, IEnumerable<ImageDto>)> AddImagesToExistingProject(int projectId, List<IFormFile>? images);
        Task<IEnumerable<ProjectDto>> GetAllProjects(int page, int pageSize);
        Task<Dictionary<string, List<ProjectDto>>> GetProjectsGroupedByCompany(int page, int pageSize);
        Task<Dictionary<string, object>> GetProjectsGroupedByUsername(string username, int page, int pageSize);
        ICollection<Image> SelectImages(ICollection<Image> images);
        ICollection<ProjectDto> ProjectSelectQuery(ICollection<Project> projects);
        Task<(string status, IEnumerable<EmployeeDto>)> AddEmployeesToProject(int projectId, List<int> employees);
        Task<bool> IsEmployeeAlreadyInProject(int employeeId, int projectId);
        Task<Dictionary<string, object>> GetProjectsByEmployeeUsername(string username, int page, int pageSize);
    }
}
