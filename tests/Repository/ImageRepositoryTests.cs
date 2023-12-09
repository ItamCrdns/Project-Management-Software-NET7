
//using CloudinaryDotNet;
//using CompanyPMO_.NET.Data;
//using CompanyPMO_.NET.Models;
//using CompanyPMO_.NET.Repository;
//using dotenv.net;
//using FakeItEasy;
//using FluentAssertions;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;

//namespace Tests.Repository
//{
//    public class ImageRepositoryTests
//    {
//        private readonly Cloudinary _cloudinary;

//        public ImageRepositoryTests()
//        {
//            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
//            Cloudinary cloudinary = new(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));

//            _cloudinary = A.Fake<Cloudinary>(x => x.WithArgumentsForConstructor(new object[] { cloudinary }));
//        }

//        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions
//        {
//            get
//            {
//                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                 .Options;

//                return options;
//            }
//        }

//        private static async Task<ApplicationDbContext> GetDatabaseContext()
//        {
//            var dbContext = new ApplicationDbContext(CreateNewContextOptions);
//            dbContext.Database.EnsureCreated();

//            await dbContext.SaveChangesAsync();

//            return dbContext;
//        }

//        [Fact]
//        public async void ImageRepository_AddImagesToExistingEntity_ReturnsAddedMessageAndImages()
//        {
//            // Arrange
//            var dbContext = await GetDatabaseContext();
//            var imageRepository = new ImageRepository(dbContext, _cloudinary);

//            var images = new List<IFormFile> { A.Fake<IFormFile>() };
//            int entityId = 1;
//            string entityType = "Type";
//            int? imagesInEntity = 5;

//            A.CallTo(() => imageRepository.AddImagesToNewEntity(images, entityId, entityType, imagesInEntity))
//                .Returns(System.Threading.Tasks.Task.FromResult(new List<Image> { new() }));

//            // Act
//            var result = await imageRepository.AddImagesToExistingEntity(entityId, images, entityType, imagesInEntity);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("1 images added.", result.status);
//            Assert.Single(result.Item2);
//        }
//    }
//}
