using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface ICompany
    {
        Task<Company> GetCompany(int companyId);
        Task<(bool created, Company)> AddCompany(int supervisorId, CompanyDto companyDto, List<IFormFile>? images);
        Task<(bool updated, Company)> UpdateCompany(int companyId, CompanyDto companyDto, List<IFormFile>? images);
        Task<Company> GetCompanyById(int companyId);
        Task<bool> DoesCompanyExist(int companyId);
        Task<IEnumerable<Image>> AddImagesToExistingCompany(int companyId, List<IFormFile>? images);
    }
}
