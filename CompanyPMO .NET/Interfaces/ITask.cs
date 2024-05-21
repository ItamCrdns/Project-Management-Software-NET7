using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;
using Task = CompanyPMO_.NET.Models.Task;

namespace CompanyPMO_.NET.Interfaces
{
    public interface ITask
    {
        // Fully implemented in task controller
        Task<List<Employee>> GetEmployeesWorkingOnTask(int taskId); // Get employees working in a certain task
        Task<List<Task>> GetTasks(int page, int pageSize);
        Task<DataCountPages<TaskDto>> GetAllTasks(FilterParams filterParams);
        Task<DataCountPages<TaskShowcaseDto>> GetAllTasksShowcase(int page, int pageSize);
        Task<DataCountPages<ProjectTaskGroup>> GetTasksGroupedByProject(FilterParams filterParams, int tasksPage, int tasksPageSize, int employeeId); 
        Task<bool> IsParticipant(int taskId, int employeeId);
        Task<bool> IsOwner(int taskId, int employeeId);
    }
}
