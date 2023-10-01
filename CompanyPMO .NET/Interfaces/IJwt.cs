using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IJwt
    {
        string JwtTokenGenerator(Employee employee);
    }
}
