using CompanyPMO_.NET.Common;

namespace CompanyPMO_.NET.Interfaces.Workload_interfaces
{
    public interface IWorkloadOverdues
    {
        Task<OperationResult> UpdateOverdueProjects();
        Task<OperationResult> UpdateOverdueTasks();
        Task<OperationResult> UpdateOverdueIssues();
    }
}
