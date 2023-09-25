using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IEmployee
    {
        Task<bool> RegisterEmployee(EmployeeRegisterDto employee, IFormFile image);
        Task<(bool authenticated, string result, EmployeeDto employee)> AuthenticateEmployee(string username, string password);
        Task<bool?> IsAccountLocked(string username);
        Task<Employee> GetEmployeeForClaims(string username);
        Task<Employee> GetEmployeeById(int employeeId);
    }
}
