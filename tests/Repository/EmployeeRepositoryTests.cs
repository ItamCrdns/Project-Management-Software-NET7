using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace tests.Repository
{
    public class EmployeeRepositoryTests
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly IImage _image;
        private readonly IUtility _utility;
        private readonly DbContextOptions<ApplicationDbContext> dbContextOptions;
        public EmployeeRepositoryTests()
        {
            _image = A.Fake<IImage>();
            _utility = A.Fake<IUtility>();
        }

        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions
        {
            get
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "CompanyPMO")
                    .Options;

                return options;
            }
        }

        private static async Task<ApplicationDbContext> GetDatabaseContext()
        {
            var dbContext = new ApplicationDbContext(CreateNewContextOptions);
            dbContext.Database.EnsureCreated();

            if (!await dbContext.Employees.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Create 10 employee instances in the in-memory database
                    dbContext.Employees.Add(new Employee
                    {
                        //EmployeeId = i,
                        Username = $"Test{i}",
                        FirstName = $"Test{i}",
                        LastName = $"Test{i}",
                        Gender = $"Test{i}",
                        Password = $"Test{i}",
                        ProfilePicture = $"Test{i}",
                        PhoneNumber = $"Test{i}",
                        Role = $"Test{i}",
                        Email = $""
                    });
                }
                await dbContext.SaveChangesAsync();
            }
            return dbContext;
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeeById_ReturnsListOfEmployees()
        {
            int id = 2;
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetEmployeeById(id);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Employee));
        }
    }
}
