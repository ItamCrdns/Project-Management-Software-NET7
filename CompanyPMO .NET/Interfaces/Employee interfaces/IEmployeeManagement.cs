using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces.Employee_interfaces
{
    public interface IEmployeeManagement
    {
        // Fully implemented in Employee management controller
        Task<(string result, bool status, EmployeeShowcaseDto newEmployee)> RegisterEmployee(EmployeeRegisterDto employee, IFormFile image);
        Task<OperationResult<EmployeeShowcaseDto>> UpdateEmployee(int employeeId, UpdateEmployeeDto employee, IFormFile? image, string? currentPassword);
    }
}
