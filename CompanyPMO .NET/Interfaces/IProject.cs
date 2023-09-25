using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IProject
    {
        Task<Project> CreateProject(Project project);
    }
}
