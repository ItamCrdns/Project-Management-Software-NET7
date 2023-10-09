using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IProject
    {
        Task<(Project, List<Image>)> CreateProject(Project project, int employeeSupervisorId, List<IFormFile>? images, int companyId);
        Task<(bool updated, ProjectDto)> UpdateProject(int employeeId, int projectId, ProjectDto projectDto, List<IFormFile>? images);
        Task<Project> GetProjectById(int projectId);
        Task<bool> SetProjectFinalized(int projectId);
        Task<bool> DoesProjectExist(int projectId);
        Task<(string status, IEnumerable<ImageDto>)> AddImagesToExistingProject(int projectId, List<IFormFile>? images);
        Task<IEnumerable<ProjectDto>> GetAllProjects(int page, int pageSize);
        Task<Dictionary<string, List<ProjectDto>>> GetProjectsGroupedByCompany(int page, int pageSize);
        ICollection<Image> SelectImages(ICollection<Image> images);
        Task<(string status, IEnumerable<EmployeeDto>)> AddEmployeesToProject(int projectId, List<int> employees);
        Task<bool> IsEmployeeAlreadyInProject(int employeeId, int projectId);
    }
}
