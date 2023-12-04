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
        Task<IEnumerable<Employee>> GetEmployeesBySupervisorId(int supervisorId);
        Employee EmployeeQuery(Employee employee);
        Task<DataCountAndPagesizeDto<IEnumerable<EmployeeShowcaseDto>>> GetEmployeesWorkingInTheSameCompany(string username, int page, int pageSize);
        Task<DataCountAndPagesizeDto<IEnumerable<EmployeeShowcaseDto>>> SearchEmployeesWorkingInTheSameCompany(string search, string username, int page, int pageSize);
        Task<Dictionary<string, object>> GetProjectEmployees(int projectId, int page, int pageSize);
        Task<Dictionary<string, object>> SearchProjectEmployees(string search, int projectId, int page, int pageSize);
        Task<IEnumerable<EmployeeShowcaseDto>> GetEmployeesShowcasePaginated(int page, int pageSize);
        Task<Dictionary<string, object>> GetEmployeesByCompanyPaginated(int companyId, int page, int pageSize);
        Task<DataCountAndPagesizeDto<List<EmployeeShowcaseDto>>> SearchEmployeesByCompanyPaginated(string search, int companyId, int page, int pageSize);
        IEnumerable<EmployeeShowcaseDto> EmployeeShowcaseQuery(IEnumerable<Employee> employees);
        Task<TierDto> GetEmployeeTier(int employeeId);
        Task<string> GetEmployeeUsernameById(int employeeId);
        Task<DataCountAndPagesizeDto<IEnumerable<EmployeeShowcaseDto>>> GetEmployeesThatHaveCreatedProjectsInACertainClient(int clientId, int page, int pageSize);
        // * Will take a list of integers (employeeIds) and will return a list of employees, will remove non existing Ids
        Task<IEnumerable<EmployeeShowcaseDto>> GetEmployeesFromAListOfEmployeeIds(string employeeIds); // * STRING SHOULD BE A LIST OF INTEGERS SEPARATED BY '-' (i.e 1-2-3-4-5)
    }
}