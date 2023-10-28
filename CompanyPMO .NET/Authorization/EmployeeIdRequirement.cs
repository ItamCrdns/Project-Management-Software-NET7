using Microsoft.AspNetCore.Authorization;

namespace CompanyPMO_.NET.Authorization
{
    public class EmployeeIdRequirement : IAuthorizationRequirement
    {
        public string EmployeeId { get; set; }
        public EmployeeIdRequirement(string employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
