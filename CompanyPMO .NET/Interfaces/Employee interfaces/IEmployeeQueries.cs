using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces.Employee_interfaces
{
    // Fully implemented in EmployeeController
    public interface IEmployeeQueries
    {
        Task<Employee> GetEmployeeById(int employeeId);
        Task<EmployeeDto> GetEmployeeByUsername(string username);
        Task<TierDto> GetEmployeeTier(int employeeId);
        Task<DataCountPages<EmployeeShowcaseDto>> GetEmployeesBySupervisorId(int supervisorId, FilterParams filterParams);
        Task<DataCountPages<EmployeeShowcaseDto>> GetEmployeesShowcasePaginated(int userId, int page, int pageSize);
        Task<DataCountPages<EmployeeShowcaseDto>> SearchEmployeesShowcasePaginated(int userId, string search, int page, int pageSize);
        Task<DataCountPages<EmployeeShowcaseDto>> GetEmployeesWorkingInTheSameCompany(string username, int page, int pageSize);
        Task<DataCountPages<EmployeeShowcaseDto>> SearchEmployeesWorkingInTheSameCompany(string search, string username, int page, int pageSize);
    }
}
