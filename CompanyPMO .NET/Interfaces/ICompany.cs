using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface ICompany
    {
        Task<Company> GetCompany(int companyId);
    }
}
