using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces.Employee_interfaces
{
    public interface IEmployeeAuthentication
    {
        // Fully implemented in Auth Controller
        Task<LoginResponseDto> AuthenticateEmployee(string username, string password);
        Task<OperationResult<bool>> ConfirmPassword(int employeeId, string password);
        Task<OperationResult<DateTime>> PasswordLastVerification(int employeeId);
        Task<Employee> GetEmployeeForClaims(string username);
    }
}
