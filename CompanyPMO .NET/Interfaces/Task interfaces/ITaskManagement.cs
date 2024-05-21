using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces.Task_interfaces
{
    public interface ITaskManagement
    {
        // Fully implemented in Task Management Controller
        Task<OperationResult<int>> CreateTask(TaskDto task, int employeeId, int projectId, List<IFormFile>? images, List<int>? employeeIds, bool shouldStartNow);
        Task<bool> StartingWorkingOnTask(int userId, int taskId);
        Task<bool> FinishedWorkingOnTask(int userId, int taskId);
        Task<(string status, IEnumerable<EmployeeShowcaseDto>)> AddEmployeesToTask(int taskId, List<int> employeeIds);
    }
}
