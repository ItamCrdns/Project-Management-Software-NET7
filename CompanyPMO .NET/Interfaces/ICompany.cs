using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface ICompany
    {
        Task<Company> GetCompany(int companyId);
        Task<(bool created, Company)> AddCompany(int supervisorId, CompanyDto companyDto, List<IFormFile>? images);
        Task<IEnumerable<Image>> AddImagesToExistingCompany(int companyId, List<IFormFile>? images);
    }
}
