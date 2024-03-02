﻿using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface ICompany
    {
        Task<(bool created, Company)> AddCompany(int supervisorId, CompanyDto companyDto, List<IFormFile>? images, IFormFile? logoFile);
        Task<int> CreateNewCompany(int supervisorId, string name);
        Task<(bool updated, CompanyDto)> UpdateCompany(int employeeId, int companyId, CompanyDto companyDto, List<IFormFile>? images);
        Task<Company> GetCompanyById(int companyId);
        Task<bool> DoesCompanyExist(int companyId);
        Task<(string status, IEnumerable<ImageDto>)> AddImagesToExistingCompany(int companyId, List<IFormFile>? images);
        Task<IEnumerable<CompanyShowcaseDto>> GetCompaniesThatHaveProjects();
        Task<DataCountPages<CompanyShowcaseDto>> GetAllCompanies(int page, int pageSize);
    }
}
