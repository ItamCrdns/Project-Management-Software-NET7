using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces.Workload_interfaces
{
    public interface IWorkloadEmployee
    {
        Task<Workload> GetWorkloadByEmployee(string username);
    }
}
