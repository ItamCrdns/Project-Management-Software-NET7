using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IEmployee
    {
        Task<bool> RegisterEmployee(EmployeeRegisterDto employee, IFormFile image);
        Task<(bool updated, Employee)> UpdateEmployee(int employeeId, EmployeeDto employeeDto, IFormFile image);
        Task<(AuthenticationResult result, string message, EmployeeDto employee)> AuthenticateEmployee(string username, string password);
        Task<bool?> IsAccountLocked(string username);
        Task<Employee> GetEmployeeForClaims(string username);
        Task<Employee> GetEmployeeById(int employeeId);
        Task<EmployeeDto> GetEmployeeByUsername(string username);
        Task<IEnumerable<Employee>> GetEmployeeBySupervisorId(int supervisorId);
        Employee EmployeeQuery(Employee employee);
        Task<IEnumerable<EmployeeDto>> GetEmployeesWorkingInTheSameCompany(string username);
        Task<IEnumerable<EmployeeShowcaseDto>> GetEmployeesWorkingInACertainProject(int projectId);
        Task<IEnumerable<ProjectDto>> GetProjectsByEmployeeUsername(string username);
        Task<IEnumerable<EmployeeShowcaseDto>> GetEmployeesShowcasePaginated(int page, int pageSize);
    }
}