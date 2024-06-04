using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Tests.Repository
{
    public class CompanyRepositoryTests
    {
        private readonly ICloudinary _image;
        public CompanyRepositoryTests()
        {
            _image = A.Fake<ICloudinary>();
        }

        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions
        {
            get
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                 .Options;

                return options;
            }
        }

        private static async Task<ApplicationDbContext> GetDatabaseContext()
        {
            var dbContext = new ApplicationDbContext(CreateNewContextOptions);
            dbContext.Database.EnsureCreated();

            await ResetDb.Reset(dbContext);

            if (!await dbContext.Companies.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    dbContext.Companies.Add(
                        new Company
                        {
                            Name = $"Company {i}",
                            CeoUserId = i,
                            AddressId = i,
                            ContactEmail = $"Email {i}",
                            ContactPhoneNumber = $"Phone {i}",
                            AddedById = i,
                            Logo = $"Logo {i}"
                        });
                }
            }

            if (!await dbContext.Projects.AnyAsync())
            {
                for (int i = 1; i < 6; i++)
                {
                    dbContext.Projects.Add(
                        new Project
                        {
                            Name = $"Project {i}",
                            Description = $"Description {i}",
                            Created = DateTime.Now,
                            Finished = DateTime.Now,
                            ProjectCreatorId = i,
                            CompanyId = i,
                            Priority = i,
                            ExpectedDeliveryDate = DateTime.Now,
                            Lifecycle = $"Lifecycle {i}"
                        });
                }
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async void CompanyRepository_AddCompany_ReturnsCompany()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var companyRepository = new CompanyRepository(dbContext, _image);
            var companyDto = new CompanyDto
            {
                Name = "Company 1",
                CeoUserId = 1,
                AddressId = 1,
                ContactEmail = "Email 1",
                ContactPhoneNumber = "Phone 1"
            };

            // Act
            var (created, company) = await companyRepository.AddCompany(1, companyDto, null, null);

            // Assert
            created.Should().BeTrue();
            company.Should().NotBeNull();

            company.Name.Should().Be(companyDto.Name);
            company.CeoUserId.Should().Be(companyDto.CeoUserId);
            company.AddressId.Should().Be(companyDto.AddressId);
            company.ContactEmail.Should().Be(companyDto.ContactEmail);
            company.ContactPhoneNumber.Should().Be(companyDto.ContactPhoneNumber);
        }

        [Fact]
        public async void CompanyRepository_AddCompany_ReturnsCompanyWithLogo()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var companyRepository = new CompanyRepository(dbContext, _image);
            var companyDto = new CompanyDto
            {
                Name = "Company 1",
                CeoUserId = 1,
                AddressId = 1,
                ContactEmail = "Email 1",
                ContactPhoneNumber = "Phone 1"
            };

            IFormFile fakeLogo = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.webp");

            var tupleReturn = ("Dummy logo", string.Empty);

            A.CallTo(() => _image.UploadToCloudinary(fakeLogo, 0, 0)).Returns(tupleReturn);

            // Act
            var (created, company) = await companyRepository.AddCompany(1, companyDto, null, fakeLogo);

            // Assert
            created.Should().BeTrue();
            company.Should().NotBeNull();

            company.Name.Should().Be(companyDto.Name);
            company.CeoUserId.Should().Be(companyDto.CeoUserId);
            company.AddressId.Should().Be(companyDto.AddressId);
            company.ContactEmail.Should().Be(companyDto.ContactEmail);
            company.ContactPhoneNumber.Should().Be(companyDto.ContactPhoneNumber);
            company.Logo.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async void CompanyRepository_AddCompany_ReturnsCompanyWithImages()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var companyRepository = new CompanyRepository(dbContext, _image);
            var companyDto = new CompanyDto
            {
                Name = "Company 1",
                CeoUserId = 1,
                AddressId = 1,
                ContactEmail = "Email 1",
                ContactPhoneNumber = "Phone 1"
            };

            List<IFormFile> fakeImages = [
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.webp"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file 2")), 0, 0, "Data 2", "dummy2.webp")
            ];

            // Act
            var (created, company) = await companyRepository.AddCompany(1, companyDto, fakeImages, null);

            // Assert
            created.Should().BeTrue();
            company.Should().NotBeNull();

            company.Name.Should().Be(companyDto.Name);
            company.CeoUserId.Should().Be(companyDto.CeoUserId);
            company.AddressId.Should().Be(companyDto.AddressId);
            company.ContactEmail.Should().Be(companyDto.ContactEmail);
            company.ContactPhoneNumber.Should().Be(companyDto.ContactPhoneNumber);
        }

        [Fact]
        public async void CompanyRepository_CreateNewCompany_ReturnsCompanyId()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var companyRepository = new CompanyRepository(dbContext, _image);

            // Act
            var companyId = await companyRepository.CreateNewCompany(1, "Company 1");

            // Assert
            companyId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void CompanyRepository_CreateNewCompany_Returns0()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var companyRepository = new CompanyRepository(dbContext, _image);

            // Act
            var companyId = await companyRepository.CreateNewCompany(0, string.Empty);

            // Assert
            companyId.Should().Be(0);
        }

        [Fact]
        public async void CompanyRepository_GetAllCompanies_ReturnsCompanies()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            var dbContext = await GetDatabaseContext();
            var companyRepository = new CompanyRepository(dbContext, _image);


            // Act
            var result = await companyRepository.GetAllCompanies(page, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(DataCountPages<CompanyAndCounts>));
            result.Data.Should().NotBeNullOrEmpty();
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            foreach (var each in result.Data)
            {
                each.Company.Should().NotBeNull();
            }
            result.Count.Should().BeGreaterThan(0);
            result.Pages.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void CompanyRepository_GetCompaniesThatHaveProjects_ReturnsCompanies()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var companyRepository = new CompanyRepository(dbContext, _image);

            // Act
            var companies = await companyRepository.GetCompaniesThatHaveProjects();

            // Assert
            companies.Should().NotBeNullOrEmpty();
            companies.Should().BeOfType(typeof(List<CompanyShowcaseDto>));
            companies.Should().HaveCountGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void CompanyRepository_GetCompanyById_ReturnsCompany()
        {
            // Arrange
            int companyId = 1;
            var dbContext = await GetDatabaseContext();

            var companyRepository = new CompanyRepository(dbContext, _image);

            // Act
            var company = await companyRepository.GetCompanyById(companyId);

            // Assert
            company.Should().NotBeNull();
            company.Should().BeOfType(typeof(Company));
            company.CompanyId.Should().Be(companyId);
        }

        [Fact]
        public async void CompanyRepository_GetCompanyById_ReturnsNull()
        {
            // Arrange
            int companyId = 1000;
            var dbContext = await GetDatabaseContext();

            var companyRepository = new CompanyRepository(dbContext, _image);

            // Act
            var company = await companyRepository.GetCompanyById(companyId);

            // Assert
            company.Should().BeNull();
        }
    }
}
