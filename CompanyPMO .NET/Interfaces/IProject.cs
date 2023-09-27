using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IProject
    {
        Task<(Project, List<Image>)> CreateProject(Project project, int employeeSupervisorId, List<IFormFile>? images);
        Task<(bool updated, Project)> UpdateProject(int projectId, ProjectDto project, List<IFormFile>? images);
        Task<Project> GetProjectById(int projectId);
        Task<bool> SetProjectFinalized(int projectId);
    }
}
