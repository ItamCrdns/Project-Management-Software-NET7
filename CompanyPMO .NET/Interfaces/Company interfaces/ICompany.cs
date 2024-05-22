using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface ICompany
    {
        // Fully implemented in Company Controller
        Task<Company> GetCompanyById(int companyId);
        Task<IEnumerable<CompanyShowcaseDto>> GetCompaniesThatHaveProjects();
        Task<DataCountPages<CompanyShowcaseDto>> GetAllCompanies(int page, int pageSize);
    }
}
