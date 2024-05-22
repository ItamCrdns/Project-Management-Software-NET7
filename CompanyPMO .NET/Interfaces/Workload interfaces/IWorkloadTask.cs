using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces.Workload_interfaces
{
    public interface IWorkloadTask
    {
        // Might separate this into two methods later (UpdateEmployeeAssignedTasks and UpdateEmployeeAssignedIssues)
        Task<OperationResult<List<WorkloadDto>>> UpdateEmployeeWorkloadAssignedTasksAndIssues(int[] employees);
        Task<OperationResult> UpdateEmployeeCompletedTasks(int[] employees);
    }
}
