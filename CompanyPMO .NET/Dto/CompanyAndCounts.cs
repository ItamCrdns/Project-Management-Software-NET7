using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Dto
{
    public class CompanyAndCounts
    {
        public Company Company { get; set; }
        public int EmployeeCount { get; set; }
        public int ProjectCount { get; set; }
    }
}
