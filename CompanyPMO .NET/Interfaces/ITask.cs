using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface ITask
    {
        Task<OperationResult<int>> CreateTask(TaskDto task, int employeeId, int projectId, List<IFormFile>? images, List<int>? employeeIds, bool shouldStartNow);
        // When hitting those two endpoints, update their dates
        Task<bool> StartingWorkingOnTask(int userId, int taskId);
        Task<bool> FinishedWorkingOnTask(int userId, int taskId);
        Task<Models.Task> GetTaskById(int taskId);
        Task<List<Employee>> GetEmployeesWorkingOnTask(int taskId); // Get employees working in a certain task
        Task<DataCountPages<TaskShowcaseDto>> GetTasksShowcaseByProjectId(int projectId, int page, int pageSize);
        Task<List<Models.Task>> GetTasks(int page, int pageSize);
        Task<DataCountPages<TaskDto>> GetAllTasks(FilterParams filterParams);
        ICollection<Image> SelectImages(ICollection<Image> images);
        Task<(string status, IEnumerable<EmployeeShowcaseDto>)> AddEmployeesToTask(int taskId, List<int> employeeIds);
        Task<bool> DoesTaskExist(int taskId);
        Task<bool> IsEmployeeAlreadyInTask(int employeeId, int taskId);
        Task<DataCountPages<TaskDto>> GetTasksByEmployeeUsername(string username, int page, int pageSize);
        Task<DataCountPages<TaskShowcaseDto>> GetTasksShowcaseByEmployeeUsername(string username, int page, int pageSize);
        Task<DataCountPages<TaskDto>> GetTasksByProjectId(int projectId, FilterParams filterParams);
        Task<DataCountPages<TaskShowcaseDto>> GetAllTasksShowcase(int page, int pageSize);
        Task<DataCountPages<ProjectTaskGroup>> GetTasksGroupedByProject(FilterParams filterParams, int tasksPage, int tasksPageSize, int employeeId);
        IEnumerable<TaskDto> TaskDtoSelectQuery(ICollection<Models.Task> tasks);
    }
}
