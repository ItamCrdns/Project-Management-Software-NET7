using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IWorkload
    {
        Task<OperationResult> UpdateEmployeeAssignedProjectsCount(int[] employees);
        Task<OperationResult> UpdateEmployeeCompletedProjects(int[] employees); // List<string> is a list of the updated workloadsums returned. Example: ["Very high", "High", "None"]. Just for testing purposes
        Task<OperationResult<List<WorkloadDto>>> UpdateEmployeeWorkloadAssignedTasksAndIssues(int[] employees);
        Task<OperationResult> CreateWorkloadEntityForEmployee(int employeeId);
    }
}
