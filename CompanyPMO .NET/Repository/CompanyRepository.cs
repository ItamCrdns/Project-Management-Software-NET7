using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Interfaces.Company_interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyPMO_.NET.Repository
{
    public class CompanyRepository : ICompany, ICompanyManagement
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinary _imageService;
        private readonly IUtility _utilityService;

        public CompanyRepository(ApplicationDbContext context, ICloudinary imageService, IUtility utilityService)
        {
            _context = context;
            _imageService = imageService;
            _utilityService = utilityService;
        }

        public async Task<(bool created, Company)> AddCompany(int supervisorId, CompanyDto companyDto, List<IFormFile>? images, IFormFile? logoFile)
        {
            string logoUrl = string.Empty;
            if (logoFile is not null)
            {
                var (imageUrl, _) = await _imageService.UploadToCloudinary(logoFile, 0, 0);
                logoUrl = imageUrl;
            }

            var company = new Company
            {
                Name = companyDto.Name,
                CeoUserId = companyDto.CeoUserId,
                AddressId = companyDto.AddressId,
                ContactEmail = companyDto.ContactEmail,
                ContactPhoneNumber = companyDto.ContactPhoneNumber,
                AddedById = supervisorId,
                Logo = logoUrl ?? null
            };

            _context.Add(company);
            _ = await _context.SaveChangesAsync();

            List<CompanyPicture> imageCollection = new();

            if (images is not null && images.Any())
            {
                // Not implemented
            }

            var returnedCompany = new Company
            {
                Name = company.Name,
                CeoUserId = company.CeoUserId,
                AddressId = company.AddressId,
                ContactEmail = company.ContactEmail,
                ContactPhoneNumber = company.ContactPhoneNumber,
                Logo = company.Logo,
                Pictures = imageCollection
            };

            return (true, returnedCompany);
        }

        public async Task<int> CreateNewCompany(int supervisorId, string name)
        {
            if (string.IsNullOrEmpty(name) || supervisorId == 0)
            {
                return 0;
            }

            var company = new Company
            {
                Name = name,
                AddedById = supervisorId
            };

            _context.Add(company);
            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return company.CompanyId;
            }

            return 0;
        }

        public async Task<bool> DoesCompanyExist(int companyId) => await _context.Companies.AnyAsync(c => c.CompanyId.Equals(companyId));

        public async Task<DataCountPages<CompanyAndCounts>> GetAllCompanies(int page, int pageSize)
        {
            int toSkip = (page - 1) * pageSize;

            var companies = await _context.Companies
                .OrderByDescending(x => x.LatestProjectCreation)
                .Select(x => new CompanyAndCounts
                {
                    Company = x,
                    EmployeeCount = x.Employees.Count,
                    ProjectCount = x.Projects.Count
                })
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            int totalCompaniesCount = await _context.Companies.CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalCompaniesCount / pageSize);

            return new DataCountPages<CompanyAndCounts>
            {
                Data = companies,
                Count = totalCompaniesCount,
                Pages = totalPages
            };
        }

        public async Task<IEnumerable<CompanyShowcaseDto>> GetCompaniesThatHaveProjects()
        {
            // * List of company ids that have projects
            IEnumerable<int> projectIds = await _context.Projects
                .Select(c => c.CompanyId)
                .ToListAsync();

            // * List of companies that have projects
            List<Company> companies = await _context.Companies
                .Where(c => projectIds.Contains(c.CompanyId))
                .ToListAsync();

            List<CompanyShowcaseDto> companiesDto = new();
            foreach (var company in companies)
            {
                var companyDto = new CompanyShowcaseDto
                {
                    CompanyId = company.CompanyId,
                    Name = company.Name,
                    Logo = company.Logo
                };

                companiesDto.Add(companyDto);
            }

            return companiesDto;
        }

        public async Task<Company?> GetCompanyById(int companyId)
        {
            return await _context.Companies
                .Where(c => c.CompanyId.Equals(companyId))
                .Include(i => i.Pictures)
                .FirstOrDefaultAsync();
        }
    }
}
