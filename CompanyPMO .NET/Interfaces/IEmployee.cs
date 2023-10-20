using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IEmployee
    {
        Task<(string result, bool status)> RegisterEmployee(EmployeeRegisterDto employee, IFormFile image);
        Task<(bool updated, Employee)> UpdateEmployee(int employeeId, EmployeeDto employeeDto, IFormFile image);
        Task<(AuthenticationResult result, string message, EmployeeDto employee)> AuthenticateEmployee(string username, string password);
        Task<bool?> IsAccountLocked(string username);
        Task<Employee> GetEmployeeForClaims(string username);
        Task<Employee> GetEmployeeById(int employeeId);
        Task<EmployeeDto> GetEmployeeByUsername(string username);
        Task<IEnumerable<Employee>> GetEmployeeBySupervisorId(int supervisorId);
        Employee EmployeeQuery(Employee employee);
        Task<IEnumerable<EmployeeDto>> GetEmployeesWorkingInTheSameCompany(string username, int page, int pageSize);
        Task<Dictionary<string, object>> GetProjectEmployees(int projectId, int page, int pageSize);
        Task<Dictionary<string, object>> SearchProjectEmployees(string search, int projectId, int page, int pageSize);
        Task<IEnumerable<EmployeeShowcaseDto>> GetEmployeesShowcasePaginated(int page, int pageSize);
        Task<Dictionary<string, object>> GetEmployeesByCompanyPaginated(int companyId, int page, int pageSize);
        Task<Dictionary<string, object>> SearchEmployeesByCompanyPaginated(string search, int companyId, int page, int pageSize);
        IEnumerable<EmployeeShowcaseDto> EmployeeShowcaseQuery(IEnumerable<Employee> employees);
    }
}