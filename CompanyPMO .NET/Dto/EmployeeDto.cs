using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Dto
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string? ProfilePicture { get; set; }
        public Tier Tier { get; set; }
        public EmployeeDto? Supervisor { get; set; }
    }
}
