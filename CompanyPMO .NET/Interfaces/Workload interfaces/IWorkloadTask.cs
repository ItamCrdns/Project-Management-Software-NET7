using CompanyPMO_.NET.Common;

namespace CompanyPMO_.NET.Interfaces.Workload_interfaces
{
    public interface IWorkloadTask
    {
        Task<OperationResult> UpdateEmployeeAssignedTasks(int[] employeeIds);
        Task<OperationResult> UpdateEmployeeCompletedTasks(int[] employees);
        Task<OperationResult> UpdateEmployeeCreatedTasks(int employeeId);
    }
}
