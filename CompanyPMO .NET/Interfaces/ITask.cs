using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface ITask
    {
        Task<(Models.Task, List<Image>)> CreateTask(Models.Task task, int employeeId, int projectId, List<IFormFile>? images);
    }
}
