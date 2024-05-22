using CompanyPMO_.NET.Common;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IWorkload
    {
        Task<OperationResult> UpdateEmployeeAssignedProjectsCount(int[] employees);
        Task<OperationResult> UpdateEmployeeCompletedProjects(int[] employees); // List<string> is a list of the updated workloadsums returned. Example: ["Very high", "High", "None"]. Just for testing purposes
        Task<OperationResult> CreateWorkloadEntityForEmployee(int employeeId);
    }
}
