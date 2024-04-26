using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Repository;
using CompanyPMO_.NET.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tests;

namespace tests.Repository
{
    public class EmployeeRepositoryTests
    {
        private readonly IImage _image;
        private readonly IUtility _utility;
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
                            Role = $"test{i}",
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
                            LockedEnabled = true,
                            LoginAttempts = i,
                            LockedUntil = DateTime.UtcNow,
                            SupervisorId = i,
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

            if (!await dbContext.EmployeeProjects.AnyAsync())
            {
                for (int j = 1; j < 3; j++)
                {
                    dbContext.EmployeeProjects.Add(
                        new EmployeeProject
                        {
                            EmployeeId = j,
                            ProjectId = j
                        });
                }
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeeById_ReturnsListOfEmployees()
        {
            int id = 7;
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetEmployeeById(id);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Employee));
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeeById_ReturnsNull()
        {
            int id = 100;
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetEmployeeById(id);

            result.Should().BeNull();
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeeBySupervisorId_ReturnsListOfEmployees()
        {
            int id = 1;
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetEmployeesBySupervisorId(id);

            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Should().BeOfType(typeof(List<Employee>));
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeeByUsername_ReturnsEmployeeDto()
        {
            string username = "test1";
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetEmployeeByUsername(username);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(EmployeeDto));
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeeByUsername_ReturnsNull()
        {
            string username = "Test100";
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetEmployeeByUsername(username);

            result.Should().BeNull();
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeeForClaims_ReturnsEmployee()
        {
            string username = "test1";
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetEmployeeForClaims(username);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Employee));
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeeForClaims_ReturnsNull()
        {
            string username = "Test100";
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetEmployeeForClaims(username);

            result.Should().BeNull();
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeesShowcasePaginated_ReturnsListOfEmployees()
        {
            int page = 1;
            int pageSize = 10;
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetEmployeesShowcasePaginated(1, page, pageSize);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(DataCountPages<EmployeeShowcaseDto>));
            result.Data.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
            result.Data.Should().NotContain(x => x.EmployeeId == 1);
        }

        [Fact]
        public async void EmployeeRepository_GetProjectEmployees_ReturnsEmployees()
        {
            int projectId = 1;
            int page = 1;
            int pageSize = 10;
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetProjectEmployees(projectId, page, pageSize);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(DataCountPages<EmployeeShowcaseDto>));
            result.Data.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            foreach (var employee in result.Data)
            {
                employee.Username.Should().Contain("test");
            }
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeesWorkingInTheSameCompany_ReturnsDictionaryOrEmptyDictionary()
        {
            string username = "test1";
            int page = 1;
            int pageSize = 10;
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            IEnumerable<int> fakeEmployeeIds = new List<int> { 1, 2, 3, 4, 5, 6, 7 };

            A.CallTo(() => _utility.GetEntitiesByEntityId<Employee>(A<int>._, A<string>._, A<string>._, null, null))
                .Returns(System.Threading.Tasks.Task.FromResult((fakeEmployeeIds, 0, 0)));

            var result = await employeeRepository.GetEmployeesWorkingInTheSameCompany(username, page, pageSize);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(DataCountPages<EmployeeShowcaseDto>));
            result.Data.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void EmployeeRepository_IsAccountLocked_ReturnsFalse()
        {
            string username = "test1";
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.IsAccountLocked(username);

            result.Should().BeFalse();
            result.Should().NotBeNull();
        }

        [Fact]
        public async void EmployeeRepository_IsAccountLocked_ReturnsTrue()
        {
            string username = "test9";
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.IsAccountLocked(username);

            result.Should().BeTrue();
            result.Should().NotBeNull();
        }

        [Fact]
        public async void EmployeeRepository_IsAccountLocked_ReturnsNull()
        {
            string username = "Test100";
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.IsAccountLocked(username);

            result.Should().BeNull();
        }

        [Fact]
        public async void EmployeeRepository_RegisterEmployee_ReturnsCreatedTrue()
        {
            var fakeEmployee = A.Fake<EmployeeRegisterDto>();
            fakeEmployee.Username = "notExistingUsername";
            fakeEmployee.Password = "FakePassword";
            fakeEmployee.Email = "FakeEmail";
            fakeEmployee.FirstName = "FakeFirstName";
            fakeEmployee.Gender = "FakeGender";
            fakeEmployee.LastName = "FakeLastName";
            fakeEmployee.PhoneNumber = "FakePhoneNumber";
            fakeEmployee.Role = "FakeRole";

            var fakeImage = A.Fake<IFormFile>();

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.RegisterEmployee(fakeEmployee, fakeImage);

            result.Should().NotBeNull();
            result.result.Should().Be("Employee created");
            result.status.Should().BeTrue();
        }

        [Fact]
        public async void EmployeeRepository_RegisterEmployee_ReturnsCreatedFalseAndSomeFieldsNotProvided()
        {
            var fakeEmployee = A.Fake<EmployeeRegisterDto>();
            fakeEmployee.Username = "notExistingUsername";
            fakeEmployee.Password = "FakePassword";

            var fakeImage = A.Fake<IFormFile>();

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.RegisterEmployee(fakeEmployee, fakeImage);

            result.Should().NotBeNull();
            result.result.Should().Be("Email, first name, last name, gender, phone number and role cannot be null");
            result.status.Should().BeFalse();
        }

        [Fact]
        public async void EmployeeRepository_RegisterEmployee_ReturnsUsernameAlreadyRegistered()
        {
            string username = "test1";
            var fakeEmployee = A.Fake<EmployeeRegisterDto>();
            fakeEmployee.Username = username;

            var fakeImage = A.Fake<IFormFile>();

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.RegisterEmployee(fakeEmployee, fakeImage);

            result.Should().NotBeNull();
            result.result.Should().Be("Username already registered");
            result.status.Should().BeFalse();
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeesByCompanyPaginated_ReturnsDictionaryOrEmptyDictionary()
        {
            int companyId = 1;
            int page = 1;
            int pageSize = 10;
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result1 = await employeeRepository.GetEmployeesByCompanyPaginated(companyId, page, pageSize);

            result1.Should().NotBeNull();
            result1.Should().BeOfType(typeof(Dictionary<string, object>));
        }

        [Fact]
        public async void EmployeeRepository_SearchProjectEmployees_ReturnsEmployees()
        {
            string search = "test";
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.SearchProjectEmployees(search, 1, 1, 10);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(DataCountPages<EmployeeShowcaseDto>));
            result.Data.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            foreach (var employee in result.Data)
            {
                employee.Username.Should().Contain(search);
            }
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void EmployeeRepository_EmployeeShowcaseQuery_ReturnsEmployeeShowcaseDto()
        {
            var fakeEmployees = A.Fake<IEnumerable<Employee>>();

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result1 = employeeRepository.EmployeeShowcaseQuery(fakeEmployees);

            result1.Should().NotBeNull();
            result1.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
        }

        [Fact]
        public async void EmployeeRepository_SearchEmployeesByCompanyPaginated_ReturnsEmployeesData()
        {
            string search = "test";
            int companyId = 1;
            int page = 1;
            int pageSize = 10;
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.SearchEmployeesByCompanyPaginated(search, companyId, page, pageSize);

            result.Should().NotBeNull();
            result.Data.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
            result.Should().BeOfType(typeof(DataCountPages<EmployeeShowcaseDto>));
        }

        [Fact]
        public async void EmployeeRepository_SearchEmployeesByCompanyPaginated_ReturnsEmptyEmployeesData()
        {
            string search = "test100";
            int companyId = 1;
            int page = 1;
            int pageSize = 10;
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.SearchEmployeesByCompanyPaginated(search, companyId, page, pageSize);

            result.Should().NotBeNull();
            result.Data.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            result.Data.Should().HaveCount(0);
            result.Count.Should().BeGreaterThanOrEqualTo(0);
            result.Pages.Should().BeGreaterThanOrEqualTo(0);
            result.Should().BeOfType(typeof(DataCountPages<EmployeeShowcaseDto>));
        }

        [Fact]
        public async void EmployeeRepository_SearchEmployeesWorkingInTheSameCompany_ReturnsEmployeesData()
        {
            string search = "test";
            string username = "test1";
            int page = 1;
            int pageSize = 10;
            var dbContext = await GetDatabaseContext();

            IEnumerable<int> fakeEmployeeIds = new List<int> { 1, 2, 3, 4, 5, 6, 7 };

            A.CallTo(() => _utility.GetEntitiesByEntityId<Employee>(A<int>._, A<string>._, A<string>._, null, null))
                .Returns(System.Threading.Tasks.Task.FromResult((fakeEmployeeIds, 7, 1)));

            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.SearchEmployeesWorkingInTheSameCompany(search, username, page, pageSize);

            result.Should().NotBeNull();
            result.Data.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
            result.Should().BeOfType(typeof(DataCountPages<EmployeeShowcaseDto>));
        }

        [Fact]
        public async void EmployeeRepository_SearchEmployeesWorkingInTheSameCompany_ReturnsNoEmployees()
        {
            string search = "test100";
            string username = "test1";
            int page = 1;
            int pageSize = 10;
            var dbContext = await GetDatabaseContext();

            IEnumerable<int> fakeEmployeeIds = Enumerable.Empty<int>();

            A.CallTo(() => _utility.GetEntitiesByEntityId<Employee>(A<int>._, A<string>._, A<string>._, null, null))
                .Returns(System.Threading.Tasks.Task.FromResult((fakeEmployeeIds, 0, 0)));

            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.SearchEmployeesWorkingInTheSameCompany(search, username, page, pageSize);

            result.Should().NotBeNull();
            result.Data.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            result.Data.Should().HaveCount(0);
            result.Count.Should().BeGreaterThanOrEqualTo(0);
            result.Pages.Should().BeGreaterThanOrEqualTo(0);
            result.Should().BeOfType(typeof(DataCountPages<EmployeeShowcaseDto>));
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeeTier_ReturnsTierDto()
        {
            int employeeId = 1;
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetEmployeeTier(employeeId);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(TierDto));
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeeTier_ReturnsNull()
        {
            int employeeId = 100;
            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetEmployeeTier(employeeId);

            result.Should().BeNull();
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeeUsernameById_ReturnsUsernameString()
        {
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result1 = await employeeRepository.GetEmployeeUsernameById(employeeId);

            result1.Should().NotBeNull();
            result1.Should().BeOfType(typeof(string));
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeeUsernameById_ReturnsNull()
        {
            int employeeId = 100;

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result1 = await employeeRepository.GetEmployeeUsernameById(employeeId);

            result1.Should().BeNull();
        }

        [Fact]
        public async void EmployeeRepository_GetAndSearchEmployeesByProjectsCreatedInClient_ReturnsListOfEmployees()
        {
            int clientId = 1;
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            string employeeIds = "1-2-3-4-5-6-7";

            var result = await employeeRepository.GetAndSearchEmployeesByProjectsCreatedInClient(employeeIds, clientId, page, pageSize);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Dictionary<string, object>));

            var selectedEmployees = result["selectedEmployees"] as IEnumerable<EmployeeShowcaseDto>;
            var allEmployees = result["allEmployees"] as DataCountPages<EmployeeShowcaseDto>;

            selectedEmployees.Should().NotBeNull();
            selectedEmployees.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            selectedEmployees.Should().HaveCountGreaterThanOrEqualTo(1);
            selectedEmployees.Should().OnlyHaveUniqueItems(x => x.EmployeeId);

            allEmployees.Should().NotBeNull();
            allEmployees?.Data.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            allEmployees?.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            allEmployees?.Data.Should().OnlyHaveUniqueItems(x => x.EmployeeId);
        }

        [Fact]
        public async void EmployeeRepository_GetAndSearchEmployeesByProjectsCreatedInClient_ReturnsEmptyListOfEmployees()
        {
            int clientId = 100;
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            string employeeIds = "100-200-300-400-500-600-700";

            var result = await employeeRepository.GetAndSearchEmployeesByProjectsCreatedInClient(employeeIds, clientId, page, pageSize);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Dictionary<string, object>));

            var selectedEmployees = result["selectedEmployees"] as IEnumerable<EmployeeShowcaseDto>;
            var allEmployees = result["allEmployees"] as DataCountPages<EmployeeShowcaseDto>;

            selectedEmployees.Should().NotBeNull();
            selectedEmployees.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            selectedEmployees.Should().HaveCount(0);

            allEmployees.Should().NotBeNull();
            allEmployees?.Data.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            allEmployees?.Data.Should().HaveCount(0);

        }

        [Fact]
        public async void EmployeeRepository_GetEmployeesFromAListOfEmployeeIds_ReturnsListOfEmployees()
        {
            string employeeIds = "1-2-3-4-5-6-7";

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetEmployeesFromAListOfEmployeeIds(employeeIds);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            result.Should().HaveCountGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async void EmployeeRepository_GetEmployeesFromAListOfEmployeeIds_ReturnsEmptyListOfEmployees()
        {
            string employeeIds = "100-200-300-400-500-600-700";

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.GetEmployeesFromAListOfEmployeeIds(employeeIds);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            result.Should().HaveCount(0);
        }

        [Fact]
        public async void EmployeeRepository_SearchEmployeesShowcasePaginated_ReturnsListOfEmployees()
        {
            string search = "test";
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.SearchEmployeesShowcasePaginated(1, search, page, pageSize);

            result.Should().NotBeNull();
            result.Data.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            result.Data.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Pages.Should().BeGreaterThanOrEqualTo(1);
            result.Should().BeOfType(typeof(DataCountPages<EmployeeShowcaseDto>));
            result.Data.Should().NotContain(x => x.EmployeeId == 1);
        }

        [Fact]
        public async void EmployeeRepository_SearchEmployeesShowcasePaginated_ReturnsEmptyListOfEmployees()
        {
            string search = "test100";
            int page = 1;
            int pageSize = 10;

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.SearchEmployeesShowcasePaginated(1, search, page, pageSize);

            result.Should().NotBeNull();
            result.Data.Should().BeOfType(typeof(List<EmployeeShowcaseDto>));
            result.Data.Should().HaveCount(0);
            result.Count.Should().BeGreaterThanOrEqualTo(0);
            result.Pages.Should().BeGreaterThanOrEqualTo(0);
            result.Should().BeOfType(typeof(DataCountPages<EmployeeShowcaseDto>));
        }

        [Fact]
        public async void EmployeeRepository_UpdateEmployee_ReturnsSuccess()
        {
            int employeeId = 1;
            var fakeEmployee = A.Fake<UpdateEmployeeDto>();
            fakeEmployee.Email = "update";
            var fakeImage = A.Fake<IFormFile>();
            string currentPassword = "test0";

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.UpdateEmployee(employeeId, fakeEmployee, fakeImage, currentPassword);

            result.Should().NotBeNull();
            result.Message.Should().Be("Employee updated");
            result.Success.Should().BeTrue();
            result.Data.Should().BeOfType(typeof(EmployeeShowcaseDto));
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async void EmployeeRepository_UpdateEmployee_ReturnsFailure()
        {
            int employeeId = 1;
            var fakeEmployee = A.Fake<UpdateEmployeeDto>();
            fakeEmployee.Email = "update";
            var fakeImage = A.Fake<IFormFile>();
            string currentPassword = "test99999999999999";

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.UpdateEmployee(employeeId, fakeEmployee, fakeImage, currentPassword);

            result.Should().NotBeNull();
            result.Message.Should().Be("Wrong password");
            result.Success.Should().BeFalse();
            result.Data.Should().BeOfType(typeof(EmployeeShowcaseDto));
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async void EmployeeRepository_UpdateEmployee_ReturnsFailurePasswordNotProvidedAndNotVerifiedInTheLast5Mins()
        {
            int employeeId = 8;
            var fakeEmployee = A.Fake<UpdateEmployeeDto>();
            fakeEmployee.Email = "update";
            var fakeImage = A.Fake<IFormFile>();

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.UpdateEmployee(employeeId, fakeEmployee, fakeImage, null);

            result.Should().NotBeNull();
            result.Message.Should().Be("Password not verified in the last 5 minutes");
            result.Success.Should().BeFalse();
            result.Data.Should().BeOfType(typeof(EmployeeShowcaseDto));
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async void EmployeeRepository_ConfirmPassword_ReturnsSuccess()
        {
            int employeeId = 1;
            string password = "test0";

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.ConfirmPassword(employeeId, password);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Password confirmed");
        }

        [Fact]
        public async void EmployeeRepository_ConfirmPassword_ReturnsFailure()
        {
            int employeeId = 1;
            string password = "test99999999999999";

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.ConfirmPassword(employeeId, password);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Password does not match");
        }

        [Fact]
        public async void EmployeeRepository_PasswordLastVerification_ReturnsSuccess()
        {
            int employeeId = 1;

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.PasswordLastVerification(employeeId);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OperationResult<DateTime>));
            result.Message.Should().Be("Password has been verified");
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async void EmployeeRepository_PasswordLastVerification_ReturnsFailure()
        {
            int employeeId = 7;

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.PasswordLastVerification(employeeId);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OperationResult<DateTime>));
            result.Message.Should().Be("Password not verified in the last 5 minutes");
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async void EmployeeRepository_PasswordLastVerification_ReturnsNull()
        {
            int employeeId = 9;

            var dbContext = await GetDatabaseContext();
            var employeeRepository = new EmployeeRepository(dbContext, _image, _utility);

            var result = await employeeRepository.PasswordLastVerification(employeeId);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OperationResult<DateTime>));
            result.Message.Should().Be("Password has never been verified");
            result.Success.Should().BeFalse();
        }
    }
}
