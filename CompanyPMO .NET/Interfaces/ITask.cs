using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface ITask
    {
        Task<(Models.Task, List<Image>)> CreateTask(Models.Task task, int employeeId, int projectId, List<IFormFile>? images);
        // When hitting those two endpoints, update their dates
        Task<bool> StartingWorkingOnTask(int userId, int taskId);
        Task<bool> FinishedWorkingOnTask(int userId, int taskId);
        Task<Models.Task> GetTaskById(int taskId);
        Task<List<Employee>> GetEmployeesWorkingOnTask(int taskId); // Get employees working in a certain task
        Task<List<Models.Task>> GetTasksByProjectId(int projectId);
    }
}
