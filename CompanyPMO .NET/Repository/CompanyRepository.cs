using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Repository
{
    public class CompanyRepository : ICompany
    {
        private readonly ApplicationDbContext _context;
        private readonly IImage _imageService;

        public CompanyRepository(ApplicationDbContext context, IImage imageService)
        {
            _context = context;
            _imageService = imageService;
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
                imageCollection = await _imageService.AddImagesToEntity(images, company.CompanyId, "Company");
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

        public async Task<IEnumerable<Image>> AddImagesToExistingCompany(int companyId, List<IFormFile>? images)
        {
            if(images is not null && images.Any(i => i.Length > 0))
            {
                List<Image> imageCollection = await _imageService.AddImagesToEntity(images, companyId, "Company");

                return imageCollection;
            }
            return null;
        }

        public async Task<Company> GetCompany(int companyId) => await _context.Companies.FindAsync(companyId);
    }
}
