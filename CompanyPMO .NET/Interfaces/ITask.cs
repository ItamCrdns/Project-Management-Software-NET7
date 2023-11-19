using CompanyPMO_.NET.Dto;
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
        Task<Dictionary<string, object>> GetTasksShowcaseByProjectId(int projectId, int page, int pageSize);
        Task<List<Models.Task>> GetTasks(int page, int pageSize);
        Task<DataCountAndPagesizeDto<IEnumerable<TaskDto>>> GetAllTasks(FilterParams filterParams);
        ICollection<Image> SelectImages(ICollection<Image> images);
        Task<(string status, IEnumerable<EmployeeDto>)> AddEmployeesToTask(int taskId, List<int> employeeIds);
        Task<bool> DoesTaskExist(int taskId);
        Task<bool> IsEmployeeAlreadyInTask(int employeeId, int taskId);
        Task<Dictionary<string, object>> GetTasksByEmployeeUsername(string username, int page, int pageSize);
        Task<Dictionary<string, object>> GetTasksShowcaseByEmployeeUsername(string username, int page, int pageSize);
        Task<DataCountAndPagesizeDto<IEnumerable<TaskDto>>> GetTasksByProjectId(int projectId, int page, int pageSize);
        Task<Dictionary<string, object>> GetAllTasksShowcase(int page, int pageSize);
        IEnumerable<TaskDto> TaskDtoSelectQuery(ICollection<Models.Task> tasks);
    }
}
