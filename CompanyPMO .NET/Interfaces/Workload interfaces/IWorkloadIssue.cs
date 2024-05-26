using CompanyPMO_.NET.Common;

namespace CompanyPMO_.NET.Interfaces.Workload_interfaces
{
    public interface IWorkloadIssue
    {
        Task<OperationResult> UpdateEmployeeAssignedIssues(int[] employeeIds);
        Task<OperationResult> UpdateEmployeeCompletedIssues(int[] employees);
        Task<OperationResult> UpdateEmployeeCreatedIssues(int employeeId);
    }
}
