using CompanyPMO_.NET.Common;

namespace CompanyPMO_.NET.Interfaces.Workload_interfaces
{
    public interface IWorkloadProject
    {
        Task<OperationResult> UpdateEmployeeAssignedProjects(int[] employeeIds);
        Task<OperationResult> UpdateEmployeeCompletedProjects(int[] employees);
        Task<OperationResult> UpdateEmployeeCreatedProjects(int employeeId); // Will be called when you create a new project
    }
}
