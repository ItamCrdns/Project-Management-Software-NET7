using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyPMO_.NET.Repository
{
    public class CompanyRepository : ICompany
    {
        private readonly ApplicationDbContext _context;
        private readonly IImage _imageService;
        private readonly IUtility _utilityService;

        public CompanyRepository(ApplicationDbContext context, IImage imageService, IUtility utilityService)
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

            List<Image> imageCollection = new();

            if (images is not null && images.Any(i => i.Length > 0))
            {
                imageCollection = await _imageService.AddImagesToNewEntity(images, company.CompanyId, "Company", null);
            }

            var returnedCompany = new Company
            {
                Name = company.Name,
                CeoUserId = company.CeoUserId,
                AddressId = company.AddressId,
                ContactEmail = company.ContactEmail,
                ContactPhoneNumber = company.ContactPhoneNumber,
                Logo = company.Logo,
                Images = imageCollection
            };

            return (true, returnedCompany);
        }

        public async Task<(string status, IEnumerable<ImageDto>)> AddImagesToExistingCompany(int companyId, List<IFormFile>? images)
        {
            var company = await GetCompanyById(companyId);

            int imageCountInCompanyEntity = company.Images.Count;

            return await _imageService.AddImagesToExistingEntity(companyId, images, "Company", imageCountInCompanyEntity);
        }

        public async Task<bool> DoesCompanyExist(int companyId) => await _context.Companies.AnyAsync(c => c.CompanyId.Equals(companyId));

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

        public async Task<Company> GetCompanyById(int companyId)
        {
            var company = await _context.Companies
                .Where(c => c.CompanyId.Equals(companyId))
                .Include(i => i.Images)
                .FirstOrDefaultAsync();

            var companyImages = company.Images
                .Where(et => et.EntityType.Equals("Company"))
                .Select(i => new Image
                {
                    ImageId = i.ImageId,
                    EntityType = i.EntityType,
                    EntityId = i.EntityId,
                    ImageUrl = i.ImageUrl,
                    PublicId = i.PublicId,
                    Created = i.Created,
                }).ToList();

            company.Images = companyImages;

            return company;
        }

        public async Task<(bool updated, CompanyDto)> UpdateCompany(int employeeId, int companyId, CompanyDto companyDto, List<IFormFile>? images)
        {
            return await _utilityService.UpdateEntity(employeeId, companyId, companyDto, images, AddImagesToExistingCompany, GetCompanyById);
        }
    }
}
