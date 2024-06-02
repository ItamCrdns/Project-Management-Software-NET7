using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces.Task_interfaces
{
    public interface ITaskManagement
    {
        // Fully implemented in Task Management Controller
        Task<OperationResult<int>> CreateTask(TaskDto task, int employeeSupervisorId, int projectId, List<IFormFile>? images, List<int>? employeeIds, bool shouldStartNow);
        Task<OperationResult> SetTasksStartBulk(int[] taskIds, int employeeId);
        Task<OperationResult> SetTaskStart(int taskId, int employeeId);
        Task<OperationResult> SetTasksFinishedBulk(int[] taskIds, int employeeId);
        Task<OperationResult> SetTaskFinished(int taskId, int employeeId);
    }
}
