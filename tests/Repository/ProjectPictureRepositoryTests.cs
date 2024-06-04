using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Project = CompanyPMO_.NET.Models.Project;

namespace Tests.Repository
{
    public class ProjectPictureRepositoryTests
    {
        private readonly ICloudinary _cloudinary;
        public ProjectPictureRepositoryTests()
        {
            _cloudinary = A.Fake<ICloudinary>();
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


            if (!await dbContext.Employees.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    dbContext.Employees.Add(
                        new Employee
                        {
                            Username = $"test{i}",
                            Email = $"test{i}",
                            PhoneNumber = $"test{i}",
                            Password = BCrypt.Net.BCrypt.HashPassword($"test{i}"),
                            FirstName = $"test{i}",
                            LastName = $"test{i}",
                            Gender = $"test{i}",
                            Created = DateTime.UtcNow,
                            ProfilePicture = $"test{i}",
                            LastLogin = DateTime.UtcNow,
                            CompanyId = (i % 2) + 1, // Only company Ids one and two
                            TierId = (i % 2) + 1,
                            LockedEnabled = false,
                            LoginAttempts = i,
                            LockedUntil = i == 8 ? DateTime.UtcNow : null,
                            SupervisorId = i == 8 ? null : i,
                            PasswordVerified = i == 8 ? null : DateTime.UtcNow.AddMinutes(-i)
                        });
                };
            }

            if (!await dbContext.Tiers.AnyAsync())
            {
                for (int j = 1; j < 3; j++)
                {
                    dbContext.Tiers.Add(
                        new Tier
                        {
                            Name = $"test{j}",
                            Duty = $"test{j}",
                            Created = DateTime.UtcNow
                        });
                }
            }

            if (!await dbContext.Companies.AnyAsync())
            {
                for (int j = 1; j < 3; j++)
                {
                    dbContext.Companies.Add(
                        new Company
                        {
                            Name = $"test{j}",
                            CeoUserId = j,
                            AddressId = j,
                            ContactEmail = $"test{j}",
                            ContactPhoneNumber = $"test{j}",
                            AddedById = j,
                            Logo = $"test{j}"
                        });
                }
            }

            if (!await dbContext.Projects.AnyAsync())
            {
                for (int j = 1; j < 3; j++)
                {
                    dbContext.Projects.Add(
                        new Project
                        {
                            Name = $"test{j}",
                            Description = $"test{j}",
                            Created = DateTime.UtcNow,
                            Finished = DateTime.UtcNow,
                            ProjectCreatorId = j,
                            CompanyId = j,
                            Priority = j,
                            ExpectedDeliveryDate = DateTime.UtcNow,
                            Lifecycle = $"test{j}"
                        });
                }
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async void AddPicturesToProject_ShouldReturnSuccess()
        {
            var dbContext = await GetDatabaseContext();

            var projectPictureRepository = new ProjectPictureRepository(dbContext, _cloudinary);

            List<IFormFile> fakeIFormFileList =
                [
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test")), 0, 0, "Data", "test.jpg"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 2")), 0, 0, "Data 2", "test2.jpg"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 3")), 0, 0, "Data 3", "test3.jpg")
                ];

            A.CallTo(() => _cloudinary.UploadToCloudinary(A<IFormFile>._, A<int>._, A<int>._)).Returns(("https://dummyurl.com", "dummypublicid"));

            var result = await projectPictureRepository.AddPicturesToProject(1, 1, fakeIFormFileList);

            result.Message.Should().Be("Pictures uploaded successfully");
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async void AddPicturesToProject_ShouldReturnFailureNoPictures()
        {
            var dbContext = await GetDatabaseContext();

            var projectPictureRepository = new ProjectPictureRepository(dbContext, _cloudinary);

            List<IFormFile> fakeIFormFileList = [];

            var result = await projectPictureRepository.AddPicturesToProject(1, 1, fakeIFormFileList);

            result.Message.Should().Be("No pictures uploaded");
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void AddPicturesToProject_ShouldReturnFailureProjectNotFound()
        {
            var dbContext = await GetDatabaseContext();

            var projectPictureRepository = new ProjectPictureRepository(dbContext, _cloudinary);

            List<IFormFile> fakeIFormFileList =
                [
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test")), 0, 0, "Data", "test.jpg"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 2")), 0, 0, "Data 2", "test2.jpg"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a test 3")), 0, 0, "Data 3", "test3.jpg")
                ];

            var result = await projectPictureRepository.AddPicturesToProject(100, 1, fakeIFormFileList);

            result.Message.Should().Be("Project not found");
            result.Success.Should().BeFalse();
        }
    }
}
