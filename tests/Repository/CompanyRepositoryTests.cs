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
        private readonly IImage _image;
        private readonly IUtility _utility;
        public CompanyRepositoryTests()
        {
            _image = A.Fake<IImage>();
            _utility = A.Fake<IUtility>();
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
            var companyRepository = new CompanyRepository(dbContext, _image, _utility);
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
            var companyRepository = new CompanyRepository(dbContext, _image, _utility);
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
            var companyRepository = new CompanyRepository(dbContext, _image, _utility);
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

            List<Image> imageList =
            [
                new()
                {
                    ImageId = 1,
                    EntityType = "Company",
                    ImageUrl = "Dummy image",
                    PublicId = string.Empty,
                    Created = DateTime.Now
                },
                new()
                {
                    ImageId = 2,
                    EntityType = "Company",
                    ImageUrl = "Dummy image",
                    PublicId = string.Empty,
                    Created = DateTime.Now
                }
            ];

            A.CallTo(() => _image.AddImagesToNewEntity(A<List<IFormFile>>._, A<int>._, A<string>._, A<int?>._))
                .ReturnsLazily((List<IFormFile> images, int entityId, string entityType, int? imagesInEntity) =>
                {
                    // Simulate the behavior of adding images to the entity here based on inputs
                    var imageList = new List<Image>();
                    var imageUrl = "Fake Url";
                    var publicId = "Fake publicId";

                    foreach (var image in images)
                    {
                        var newImage = new Image
                        {
                            EntityType = entityType,
                            EntityId = entityId,
                            ImageUrl = imageUrl,
                            PublicId = publicId,
                            Created = DateTime.Now
                        };

                        imageList.Add(newImage);
                    }

                    return imageList;
                });


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
            company.Images.Should().NotBeNullOrEmpty();
            company.Images.Should().HaveCountGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void CompanyRepository_AddImagesToExistingCompany_ReturnsImagesDto()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var companyRepository = new CompanyRepository(dbContext, _image, _utility);

            List<IFormFile> fakeImages = [
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.webp"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file 2")), 0, 0, "Data 2", "dummy2.webp")
            ];

            List<ImageDto> imageDtos = [
                new()
                {
                    ImageId = 1,
                    EntityType = "Company",
                    EntityId = 1,
                    ImageUrl = "Dummy image",
                    PublicId = string.Empty,
                    Created = DateTime.Now
                },
                new()
                {
                    ImageId = 2,
                    EntityType = "Company",
                    EntityId = 1,
                    ImageUrl = "Dummy image",
                    PublicId = string.Empty,
                    Created = DateTime.Now
                }
            ];

            var tupleReturn = ("Sucess", imageDtos);

            A.CallTo(() => _image.AddImagesToExistingEntity(A<int>._, A<List<IFormFile>>._, A<string>._, A<int>._)).Returns(tupleReturn);

            // Act
            var (status, images) = await companyRepository.AddImagesToExistingCompany(1, fakeImages);

            // Assert
            status.Should().NotBeNullOrEmpty();
            images.Should().NotBeNullOrEmpty();
            images.Should().HaveCountGreaterThanOrEqualTo(1);
            images.Should().BeEquivalentTo(imageDtos);
            images.Should().BeOfType(typeof(List<ImageDto>));
        }

        [Fact]
        public async void CompanyRepository_CreateNewCompany_ReturnsCompanyId()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var companyRepository = new CompanyRepository(dbContext, _image, _utility);

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
            var companyRepository = new CompanyRepository(dbContext, _image, _utility);

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
            var companyRepository = new CompanyRepository(dbContext, _image, _utility);


            // Act
            var companies = await companyRepository.GetAllCompanies(page, pageSize);

            // Assert
            companies.Should().NotBeNull();
            companies.Should().BeOfType(typeof(DataCountPages<CompanyShowcaseDto>));
            companies.Data.Should().NotBeNullOrEmpty();
            companies.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            companies.Count.Should().BeGreaterThan(0);
            companies.Pages.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void CompanyRepository_GetCompaniesThatHaveProjects_ReturnsCompanies()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var companyRepository = new CompanyRepository(dbContext, _image, _utility);

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

            var companyRepository = new CompanyRepository(dbContext, _image, _utility);

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

            var companyRepository = new CompanyRepository(dbContext, _image, _utility);

            // Act
            var company = await companyRepository.GetCompanyById(companyId);

            // Assert
            company.Should().BeNull();
        }

        //    [Fact]
        //    public async void CompanyRepository_UpdateCompany_ReturnsUpdatedTrue()
        //    {
        //        // Not implemented yet.
        //    }
    }
}
