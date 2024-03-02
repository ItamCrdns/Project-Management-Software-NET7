using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IEmployee
    {
        Task<(string result, bool status)> RegisterEmployee(EmployeeRegisterDto employee, IFormFile image);
        Task<OperationResult<EmployeeShowcaseDto>> UpdateEmployee(int employeeId, UpdateEmployeeDto employee, IFormFile? image, string currentPassword);
        Task<(AuthenticationResult result, string message, EmployeeDto employee)> AuthenticateEmployee(string username, string password);
        Task<bool?> IsAccountLocked(string username);
        Task<Employee> GetEmployeeForClaims(string username);
        Task<Employee> GetEmployeeById(int employeeId);
        Task<EmployeeDto> GetEmployeeByUsername(string username);
        Task<IEnumerable<Employee>> GetEmployeesBySupervisorId(int supervisorId);
        Employee EmployeeQuery(Employee employee);
        Task<DataCountPages<EmployeeShowcaseDto>> GetEmployeesWorkingInTheSameCompany(string username, int page, int pageSize);
        Task<DataCountPages<EmployeeShowcaseDto>> SearchEmployeesWorkingInTheSameCompany(string search, string username, int page, int pageSize);
        Task<DataCountPages<EmployeeShowcaseDto>> GetProjectEmployees(int projectId, int page, int pageSize);
        Task<DataCountPages<EmployeeShowcaseDto>> SearchProjectEmployees(string search, int projectId, int page, int pageSize);
        Task<DataCountPages<EmployeeShowcaseDto>> GetEmployeesShowcasePaginated(int userId, int page, int pageSize);
        Task<DataCountPages<EmployeeShowcaseDto>> SearchEmployeesShowcasePaginated(int userId, string search, int page, int pageSize);
        Task<Dictionary<string, object>> GetEmployeesByCompanyPaginated(int companyId, int page, int pageSize);
        Task<DataCountPages<EmployeeShowcaseDto>> SearchEmployeesByCompanyPaginated(string search, int companyId, int page, int pageSize);
        IEnumerable<EmployeeShowcaseDto> EmployeeShowcaseQuery(IEnumerable<Employee> employees);
        Task<TierDto> GetEmployeeTier(int employeeId);
        Task<string> GetEmployeeUsernameById(int employeeId);
        Task<Dictionary<string, object>> GetAndSearchEmployeesByProjectsCreatedInClient(string? employeeIds, int clientId, int page, int pageSize);
        Task<IEnumerable<EmployeeShowcaseDto>> GetEmployeesFromAListOfEmployeeIds(string employeeIds); // * STRING SHOULD BE A LIST OF INTEGERS SEPARATED BY '-' (i.e 1-2-3-4-5)
    }
}