using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces.Company_interfaces
{
    public interface ICompanyManagement
    {
        // TODO: Implement AddImagesToExistingCompany in Company Management Controller
        Task<(bool created, Company)> AddCompany(int supervisorId, CompanyDto companyDto, List<IFormFile>? images, IFormFile? logoFile);
        Task<int> CreateNewCompany(int supervisorId, string name);
    }
}
