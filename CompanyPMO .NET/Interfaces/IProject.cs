using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IProject
    {
        Task<(Project, List<Image>)> CreateProject(Project project, int employeeSupervisorId, List<IFormFile>? images);
        Task<(bool updated, ProjectDto)> UpdateProject(int employeeId, int projectId, ProjectDto projectDto, List<IFormFile>? images);
        Task<Project> GetProjectById(int projectId);
        Task<Project?> GetProject(int projectId);
        Task<bool> SetProjectFinalized(int projectId);
        Task<bool> DoesProjectExist(int projectId);
        Task<IEnumerable<ImageDto>> AddImagesToExistingProject(int projectId, List<IFormFile>? images);
    }
}
