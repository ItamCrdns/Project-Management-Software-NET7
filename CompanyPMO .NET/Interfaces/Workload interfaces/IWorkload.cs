using CompanyPMO_.NET.Common;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IWorkload
    {
        Task<OperationResult> CreateWorkloadEntityForEmployee(int employeeId);
    }
}
