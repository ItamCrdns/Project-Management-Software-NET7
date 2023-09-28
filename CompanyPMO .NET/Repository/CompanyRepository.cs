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
        private readonly IPatcher _patcherService;

        public CompanyRepository(ApplicationDbContext context, IImage imageService, IPatcher patcherService)
        {
            _context = context;
            _imageService = imageService;
            _patcherService = patcherService;
        }

        public async Task<(bool created, Company)> AddCompany(int supervisorId, CompanyDto companyDto, List<IFormFile>? images)
        {
            var company = new Company
            {
                Name = companyDto.Name,
                CeoUserId = companyDto.CeoUserId,
                AddressId = companyDto.AddressId,
                ContactEmail = companyDto.ContactEmail,
                ContactPhoneNumber = companyDto.ContactPhoneNumber,
                AddedById = supervisorId
            };

            _context.Add(company);
            _ = await _context.SaveChangesAsync();

            List<Image> imageCollection = new();

            if (images is not null && images.Any(i => i.Length > 0))
            {
                imageCollection = await _imageService.AddImagesToNewEntity(images, company.CompanyId, "Company");
            }

            var returnedCompany = new Company
            {
                Name = company.Name,
                CeoUserId = company.CeoUserId,
                AddressId = company.AddressId,
                ContactEmail = company.ContactEmail,
                ContactPhoneNumber = company.ContactPhoneNumber,
                Images = imageCollection
            };

            return (true, returnedCompany);
        }

        public async Task<IEnumerable<ImageDto>> AddImagesToExistingCompany(int companyId, List<IFormFile>? images)
        {
            IEnumerable<ImageDto> imageCollection = await _imageService.AddImagesToExistingEntity(companyId, images, "Company");

            return imageCollection;
        }

        public async Task<bool> DoesCompanyExist(int companyId) => await _context.Companies.AnyAsync(c => c.CompanyId.Equals(companyId));

        public async Task<Company?> GetCompany(int companyId) => await _context.Companies.FindAsync(companyId);

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
                    PublicId = i.PublicId
                }).ToList();

            company.Images = companyImages;

            return company;
        }

        public async Task<(bool updated, CompanyDto)> UpdateCompany(int employeeId, int companyId, CompanyDto companyDto, List<IFormFile>? images)
        {
            return await _patcherService.UpdateEntity(employeeId, companyId, companyDto, images, AddImagesToExistingCompany, GetCompany);
        }
    }
}
