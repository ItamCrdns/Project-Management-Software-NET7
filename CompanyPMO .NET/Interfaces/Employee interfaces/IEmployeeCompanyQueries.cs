using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces.Employee_interfaces
{
    public interface IEmployeeCompanyQueries
    {
        // Fully implemented in Company Controller
        Task<Dictionary<string, object>> GetEmployeesByCompanyPaginated(int companyId, int page, int pageSize);
        Task<DataCountPages<EmployeeShowcaseDto>> SearchEmployeesByCompanyPaginated(string search, int companyId, int page, int pageSize);
        Task<Dictionary<string, object>> GetAndSearchEmployeesByProjectsCreatedInClient(string? employeeIds, int clientId, int page, int pageSize);
    }
}
