using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IProject
    {
        Task<(Project, List<Image>)> CreateProject(Project project, int employeeSupervisorId, List<IFormFile>? images);
        Task<Project> GetProjectById(int projectId);
    }
}
