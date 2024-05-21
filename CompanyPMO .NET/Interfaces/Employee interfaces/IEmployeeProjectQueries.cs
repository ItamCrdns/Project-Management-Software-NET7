using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces.Employee_interfaces
{
    public interface IEmployeeProjectQueries
    {
        // Fully implemented in Project Controller
        Task<DataCountPages<EmployeeShowcaseDto>> GetProjectEmployees(int projectId, int page, int pageSize);
        Task<DataCountPages<EmployeeShowcaseDto>> SearchProjectEmployees(string search, int projectId, int page, int pageSize);
    }
}
