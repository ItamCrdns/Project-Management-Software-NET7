using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Controllers;
using CompanyPMO_.NET.Dto;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using CompanyPMO_.NET.Models;
using System.Security.Claims;
using CompanyPMO_.NET.Common;

namespace Tests.EmployeeControllerTests
{
    public class EmployeeControllerTests
    {
        // If we need an actual instance of IEmployee, we can use the Mock.Of<T> method:
        private readonly EmployeeController _employeeController;
        private readonly IEmployee _employeeService;
        private readonly IJwt _jwtService;
        private readonly IProject _projectService;
        private readonly ITask _taskService;
        private readonly IIssue _issueService;
        private readonly IUserIdentity _userIdentityService;
        private readonly Lazy<Task<int>> _lazyUserId;
        public EmployeeControllerTests()
        {
            _employeeService = A.Fake<IEmployee>();
            _jwtService = A.Fake<IJwt>();
            _projectService = A.Fake<IProject>();
            _taskService = A.Fake<ITask>();
            _issueService = A.Fake<IIssue>();
            _userIdentityService = A.Fake<IUserIdentity>();
            _lazyUserId = new Lazy<Task<int>>(System.Threading.Tasks.Task.FromResult(1));

            var fakeClaims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, _lazyUserId.Value.ToString())
            };

            var fakeIdentity = new ClaimsIdentity(fakeClaims, "TextAuthType");

            var fakePrincipal = new ClaimsPrincipal(fakeIdentity);

            var fakeHttpContext = new DefaultHttpContext
            {
                User = fakePrincipal
            };

            _employeeController = new EmployeeController(
                _employeeService,
                _jwtService,
                _projectService,
                _taskService,
                _issueService,
                _userIdentityService
                )
            {
                ControllerContext = new ControllerContext { HttpContext = fakeHttpContext }
            };
        }

        [Fact]
        public async void EmployeeController_Login_ReturnOk()
        {
            var httpContext = A.Fake<HttpContext>();
            var response = A.Fake<HttpResponse>();

            A.CallTo(() => httpContext.Response).Returns(response);

            var employee = A.Fake<EmployeeLoginDto>();
            var loginResultTuple = (
                new AuthenticationResult { Authenticated = true },
                "Fake Message",
                new EmployeeDto()
                );

            A.CallTo(() => _employeeService.AuthenticateEmployee(employee.Username, employee.Password)).Returns(loginResultTuple);
            _employeeController.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await _employeeController.Login(employee);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_Login_ReturnUnauthorized()
        {
            var httpContext = A.Fake<HttpContext>();
            var response = A.Fake<HttpResponse>();

            A.CallTo(() => httpContext.Response).Returns(response);

            var employee = A.Fake<EmployeeLoginDto>();
            var loginResultTuple = (
                new AuthenticationResult { Authenticated = false },
                "Fake Message",
                new EmployeeDto()
                );

            A.CallTo(() => _employeeService.AuthenticateEmployee(employee.Username, employee.Password))
                .Returns(loginResultTuple);

            _employeeController.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await _employeeController.Login(employee);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(UnauthorizedObjectResult));
        }

        [Fact]
        public async void EmployeeController_RegisterEmployee_ReturnOk()
        {
            // Arrange
            var employee = A.Fake<EmployeeRegisterDto>();
            var formFile = A.Fake<IFormFile>();

            var registerResultTuple = (
                "Fake Message",
                true
                );

            A.CallTo(() => _employeeService.RegisterEmployee(employee, formFile)).Returns(registerResultTuple);

            // Act
            var result = await _employeeController.RegisterEmployee(employee, formFile);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_RegisterEmployee_ReturnBadRequest()
        {
            // Arrange
            var employee = A.Fake<EmployeeRegisterDto>();
            var formFile = A.Fake<IFormFile>();

            var registerResultTuple = (
                "Fake Message",
                false
                );

            A.CallTo(() => _employeeService.RegisterEmployee(employee, formFile))
                .Returns(registerResultTuple);

            // Act
            var result = await _employeeController.RegisterEmployee(employee, formFile);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async void EmployeeController_GetEmployeeById_ReturnOk()
        {
            // Arrange
            int supervisorId = 1;

            var employees = A.Fake<Employee>();
            var fakeImage = A.Fake<IFormFile>();

            A.CallTo(() => _employeeService.GetEmployeeById(supervisorId)).Returns(employees);

            // Act
            var result = await _employeeController.GetEmployeeById(supervisorId);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_GetEmployeeById_ReturnNotFound()
        {
            // Arrange
            int employeeId = 100;
            Employee nullEmployee = null;

            A.CallTo(() => _employeeService.GetEmployeeById(employeeId)).Returns(nullEmployee);

            // Act
            var result = await _employeeController.GetEmployeeById(employeeId);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async void EmplyeeController_GetEmployeesBySupervisorId_ReturnOk()
        {
            // Arrange
            int supervisorId = 1;
            FilterParams filterParams = new()
            {
                Page = 1,
                PageSize = 5
            };
            var employees = A.Fake<DataCountPages<EmployeeShowcaseDto>>();

            employees.Count = 3;

            A.CallTo(() => _userIdentityService.GetUserIdFromClaims(A<ClaimsPrincipal>.Ignored)).Returns(supervisorId);
            A.CallTo(() => _employeeService.GetEmployeesBySupervisorId(supervisorId, filterParams)).Returns(employees);

            // Act
            var result = await _employeeController.GetEmployeesBySupervisorId(filterParams);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmplyeeController_GetEmployeesBySupervisorId_ReturnNotFound()
        {
            // Arrange
            int supervisorId = 999;
            int page = 1;
            int pageSize = 5;
            var employees = A.Fake<DataCountPages<EmployeeShowcaseDto>>();

            var filterParams = new FilterParams();

            A.CallTo(() => _userIdentityService.GetUserIdFromClaims(A<ClaimsPrincipal>.Ignored)).Returns(supervisorId);
            A.CallTo(() => _employeeService.GetEmployeesBySupervisorId(supervisorId, filterParams)).Returns(employees);

            // Act
            var result = await _employeeController.GetEmployeesBySupervisorId(filterParams);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public void EmployeeController_EmployeeLogout_ReturnsNoContent()
        {
            // Arrange
            var httpContext = A.Fake<HttpContext>();
            var response = A.Fake<HttpResponse>();

            A.CallTo(() => httpContext.Response).Returns(response);

            _employeeController.ControllerContext.HttpContext = httpContext;

            // Act
            var result = _employeeController.EmployeeLogout();

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Fact]
        public async void EmployeeController_GetEmployeeByUsername_ReturnOk()
        {
            // Arrange
            string username = "Test";
            var employee = A.Fake<EmployeeDto>();

            A.CallTo(() => _employeeService.GetEmployeeByUsername(username)).Returns(employee);

            // Act
            var result = await _employeeController.GetEmployeeByUsername(username);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_GetEmployeeByUsername_ReturnNotFound()
        {
            // Arrange
            string username = "Test";
            EmployeeDto nullEmployee = null;

            A.CallTo(() => _employeeService.GetEmployeeByUsername(username)).Returns(nullEmployee);

            // Act
            var result = await _employeeController.GetEmployeeByUsername(username);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async void EmployeeController_GetEmployeesWorkingInTheSameCompany_ReturnOk()
        {
            // Arrange
            string username = "Test";
            int page = 1;
            int pageSize = 10;

            var dataCountAndPagesizeDto = A.Fake<DataCountPages<EmployeeShowcaseDto>>();
            dataCountAndPagesizeDto.Data = new List<EmployeeShowcaseDto>
            {
                new()
                {
                    EmployeeId = 1,
                    Username = "Test",
                    ProfilePicture = "User picture"
                },
                new()
                {
                    EmployeeId = 2,
                    Username = "Test2",
                    ProfilePicture = "User picture2"
                },
                new()
                {
                    EmployeeId = 3,
                    Username = "Test3",
                    ProfilePicture = "User picture3"
                },
            };
            dataCountAndPagesizeDto.Count = 3;
            dataCountAndPagesizeDto.Pages = 10;

            A.CallTo(() => _employeeService.GetEmployeesWorkingInTheSameCompany(username, page, pageSize))
                .Returns(dataCountAndPagesizeDto);

            // Act
            var result = await _employeeController.GetEmployeesWorkingInTheSameCompany(username, page, pageSize);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_SearchEmployeesWorkingInTheSameCompany_ReturnOk()
        {
            // Arrange
            string search = "Test";
            string username = "Test";
            int page = 1;
            int pageSize = 10;

            var dataCountAndPagesizeDto = A.Fake<DataCountPages<EmployeeShowcaseDto>>();
            dataCountAndPagesizeDto.Data = new List<EmployeeShowcaseDto>
            {
                new()
                {
                    EmployeeId = 1,
                    Username = "Test",
                    ProfilePicture = "User picture"
                },
                new()
                {
                    EmployeeId = 2,
                    Username = "Test2",
                    ProfilePicture = "User picture2"
                },
                new()
                {
                    EmployeeId = 3,
                    Username = "Test3",
                    ProfilePicture = "User picture3"
                },
            };
            dataCountAndPagesizeDto.Count = 3;
            dataCountAndPagesizeDto.Pages = 10;

            A.CallTo(() => _employeeService.SearchEmployeesWorkingInTheSameCompany(search, username, page, pageSize))
                .Returns(dataCountAndPagesizeDto);

            // Act
            var result = await _employeeController.SearchEmployeesWorkingInTheSameCompany(search, username, page, pageSize);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_GetEmployeesShowcasePaginated_ReturnOk()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;

            var fakeEmployees = new List<EmployeeShowcaseDto>
            {
                new() {
                    EmployeeId = 1,
                    Username = "Test",
                    ProfilePicture = "User picture"
                },
                new() {
                    EmployeeId = 2,
                    Username = "Test2",
                    ProfilePicture = "User picture2"
                },
                new() {
                    EmployeeId = 3,
                    Username = "Test3",
                    ProfilePicture = "User picture3"
                },
            };

            var fakeEmployeesList = A.Fake<DataCountPages<EmployeeShowcaseDto>>();
            fakeEmployeesList.Data = fakeEmployees;
            fakeEmployeesList.Count = fakeEmployees.Count;
            fakeEmployeesList.Pages = 1;

            A.CallTo(() => _userIdentityService.GetUserIdFromClaims(A<ClaimsPrincipal>.Ignored))
                .Returns(1);

            A.CallTo(() => _employeeService.GetEmployeesShowcasePaginated(1, page, pageSize))
                .Returns(fakeEmployeesList);

            // Act
            var result = await _employeeController.GetEmployeesShowcasePaginated(page, pageSize);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_GetEmployeesShowcasePaginated_ReturnNotFound()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;

            var fakeEmployeesList = A.Fake<DataCountPages<EmployeeShowcaseDto>>();

            A.CallTo(() => _employeeService.GetEmployeesShowcasePaginated(1, page, pageSize))
                .Returns(fakeEmployeesList);

            // Act
            var result = await _employeeController.GetEmployeesShowcasePaginated(page, pageSize);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async void EmployeeController_GetProjectsByEmployeeUsername_ReturnOk()
        {
            // Arrange
            string username = "Test";
            var filterParams = A.Fake<FilterParams>();
            var fakeProjectsList = A.Fake<DataCountPages<ProjectDto>>();

            fakeProjectsList.Data = new List<ProjectDto>()
            {
                new() { ProjectId = 1, Name = "Test" },
                new() { ProjectId = 2, Name = "Test2" },
                new() { ProjectId = 3, Name = "Test3" },
            };

            fakeProjectsList.Count = 3;
            fakeProjectsList.Pages = 10;

            A.CallTo(() => _projectService.GetProjectsByEmployeeUsername(username, filterParams))
                .Returns(fakeProjectsList);

            // Act
            var result = await _employeeController.GetProjectsByEmployeeUsername(username, filterParams);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_GetProjectsByEmployeeUsername_ReturnNotFound()
        {
            // Arrange
            string username = "Test";
            var filterParams = A.Fake<FilterParams>();
            DataCountPages<ProjectDto> nullProjectsList = null;

            A.CallTo(() => _projectService.GetProjectsByEmployeeUsername(username, filterParams))
                .Returns(nullProjectsList);

            // Act
            var result = await _employeeController.GetProjectsByEmployeeUsername(username, filterParams);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async void EmployeeController_GetProjectsShowcaseByEmployeeUsername_ReturnOk()
        {
            // Arrange
            var username = "Test";
            int page = 1;
            int pageSize = 10;

            var fakeProjectsList = A.Fake<DataCountPages<ProjectShowcaseDto>>();

            fakeProjectsList.Data = new List<ProjectShowcaseDto>()
            {
                new() { ProjectId = 1, Name = "Test" },
                new() { ProjectId = 2, Name = "Test2" },
                new() { ProjectId = 3, Name = "Test3" },
            };

            fakeProjectsList.Count = 3;
            fakeProjectsList.Pages = 10;

            A.CallTo(() => _projectService.GetProjectsShowcaseByEmployeeUsername(username, page, pageSize))
                .Returns(fakeProjectsList);

            // Act
            var result = await _employeeController.GetProjectsShowcaseByEmployeeUsername(username, page, pageSize);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_GetProjectsShowcaseByEmployeeUsername_ReturnNotFound()
        {
            // Arrange
            var username = "Test";
            int page = 1;
            int pageSize = 10;

            DataCountPages<ProjectShowcaseDto> nullProjectsList = null;

            A.CallTo(() => _projectService.GetProjectsShowcaseByEmployeeUsername(username, page, pageSize))
                .Returns(nullProjectsList);

            // Act
            var result = await _employeeController.GetProjectsShowcaseByEmployeeUsername(username, page, pageSize);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async void EmployeeController_GetTasksShowcaseByEmployeeUsername_ReturnOk()
        {
            // Arrange
            var username = "Test";
            int page = 1;
            int pageSize = 10;

            var fakeTasksList = A.Fake<DataCountPages<TaskShowcaseDto>>();

            fakeTasksList.Data = new List<TaskShowcaseDto>()
            {
                new() { TaskId = 1, Name = "Test" },
                new() { TaskId = 2, Name = "Test2" },
                new() { TaskId = 3, Name = "Test3" },
            };

            fakeTasksList.Count = 3;
            fakeTasksList.Pages = 10;

            A.CallTo(() => _taskService.GetTasksShowcaseByEmployeeUsername(username, page, pageSize))
                .Returns(fakeTasksList);

            // Act
            var result = await _employeeController.GetTasksShowcaseByEmployeeUsername(username, page, pageSize);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_GetTasksShowcaseByEmployeeUsername_ReturnNotFound()
        {
            // Arrange
            var username = "Test";
            int page = 1;
            int pageSize = 10;

            DataCountPages<TaskShowcaseDto> nullTasksList = null;

            A.CallTo(() => _taskService.GetTasksShowcaseByEmployeeUsername(username, page, pageSize))
                .Returns(nullTasksList);

            // Act
            var result = await _employeeController.GetTasksShowcaseByEmployeeUsername(username, page, pageSize);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async void EmployeeController_GetTasksByEmployeeUsername_ReturnOk()
        {
            // Arrange
            var username = "Test";
            int page = 1;
            int pageSize = 10;
            var fakeTasksList = A.Fake<DataCountPages<TaskDto>>();

            fakeTasksList.Data = new List<TaskDto>()
            {
                new() { TaskId = 1, Name = "Test" },
                new() { TaskId = 2, Name = "Test2" },
                new() { TaskId = 3, Name = "Test3" },
            };

            fakeTasksList.Count = 3;
            fakeTasksList.Pages = 10;

            A.CallTo(() => _taskService.GetTasksByEmployeeUsername(username, page, pageSize))
                .Returns(fakeTasksList);

            // Act
            var result = await _employeeController.GetTasksByEmployeeUsername(username, page, pageSize);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_GetTasksByEmployeeUsername_ReturnNotFound()
        {
            // Arrange
            var username = "Test";
            int page = 1;
            int pageSize = 10;

            DataCountPages<TaskDto> nullTasksList = null;

            A.CallTo(() => _taskService.GetTasksByEmployeeUsername(username, page, pageSize))
                .Returns(nullTasksList);

            // Act
            var result = await _employeeController.GetTasksByEmployeeUsername(username, page, pageSize);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async void EmployeeController_GetIssuesByEmployeeUsername_ReturnOk()
        {
            // Arrange
            var username = "Test";
            int page = 1;
            int pageSize = 10;

            var fakeIssuesList = A.Fake<DataCountPages<IssueShowcaseDto>>();
            fakeIssuesList.Data = new List<IssueShowcaseDto>()
            {
                new() { IssueId = 1, Name = "Test" },
                new() { IssueId = 2, Name = "Test2" },
                new() { IssueId = 3, Name = "Test3" },
            };

            fakeIssuesList.Count = 3;
            fakeIssuesList.Pages = 10;

            A.CallTo(() => _issueService.GetIssuesShowcaseByEmployeeUsername(username, page, pageSize))
                .Returns(fakeIssuesList);

            // Act
            var result = await _employeeController.GetIssuesByEmployeeUsername(username, page, pageSize);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_GetIssuesByEmployeeUsername_ReturnNotFound()
        {
            // Arrange
            var username = "Test";
            int page = 1;
            int pageSize = 10;

            DataCountPages<IssueShowcaseDto> nullIssuesList = null;

            A.CallTo(() => _issueService.GetIssuesShowcaseByEmployeeUsername(username, page, pageSize))
                .Returns(nullIssuesList);

            // Act
            var result = await _employeeController.GetIssuesByEmployeeUsername(username, page, pageSize);

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async void EmployeeController_GetEmployeeByIdForClaims_ReturnOk()
        {
            // Arrange
            int employeeId = 1;
            var fakeTier = A.Fake<TierDto>();

            A.CallTo(() => _userIdentityService.GetUserIdFromClaims(A<ClaimsPrincipal>._)).Returns(employeeId);

            A.CallTo(() => _employeeService.GetEmployeeTier(employeeId)).Returns(fakeTier);

            // Act
            var result = await _employeeController.GetEmployeeByIdForClaims();

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_GetMyEmployee_ReturnOk()
        {
            // Arrange
            int employeeId = 1;
            string username = "Test";
            var fakeEmployee = A.Fake<EmployeeDto>();

            A.CallTo(() => _userIdentityService.GetUserIdFromClaims(A<ClaimsPrincipal>._)).Returns(employeeId);

            A.CallTo(() => _employeeService.GetEmployeeUsernameById(employeeId)).Returns(username);

            A.CallTo(() => _employeeService.GetEmployeeByUsername(username)).Returns(fakeEmployee);

            // Act
            var result = await _employeeController.GetMyEmployee();

            // Assert
            result.Should().BeAssignableTo<IActionResult>();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        //[Fact]
        //public async void EmployeeController_GetAndSearchEmployeesByProjectsCreatedInClient_ReturnOk()
        //{
        //    // Arrange
        //    int clientId = 1;
        //    int page = 1;
        //    int pageSize = 10;

        //    var fakeReturn = A.Fake<Dictionary<string, object>>();

        //    string employeeIds = "1-2-3-4-5";

        //    A.CallTo(() => _employeeService.GetAndSearchEmployeesByProjectsCreatedInClient(employeeIds, clientId, page, pageSize))
        //        .Returns(fakeReturn);

        //    // Act
        //    var result = await _employeeController.GetAndSearchEmployeesByProjectsCreatedInClient(employeeIds, clientId, page, pageSize);

        //    // Assert
        //    result.Should().BeAssignableTo<IActionResult>();
        //    result.Should().BeOfType(typeof(OkObjectResult));
        //}

        [Fact]
        public async void EmployeeController_GetEmployeesFromAListOfEmployeeIds_ReturnOk()
        {
            // Arrange
            string employeeIds = "1-2-3-4-5";

            var employees = new List<EmployeeShowcaseDto>
            {
                new()
                {
                    EmployeeId = 1,
                    Username = "Test",
                    ProfilePicture = "User picture"
                },
                new()
                {
                    EmployeeId = 2,
                    Username = "Test2",
                    ProfilePicture = "User picture2"
                },
                new()
                {
                    EmployeeId = 3,
                    Username = "Test3",
                    ProfilePicture = "User picture3"
                },
            };

            A.CallTo(() => _employeeService.GetEmployeesFromAListOfEmployeeIds(employeeIds)).Returns(employees);

            // Act
            var result = await _employeeController.GetEmployeesFromAListOfEmployeeIds(employeeIds);

            // Assert
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void EmployeeController_GetEmployeesFromAListOfEmployeeIds_ReturnNotFound()
        {
            // Arrange
            string employeeIds = "1-2-3-4-5";
            var employees = new List<EmployeeShowcaseDto>();

            A.CallTo(() => _employeeService.GetEmployeesFromAListOfEmployeeIds(employeeIds)).Returns(employees);

            // Act
            var result = await _employeeController.GetEmployeesFromAListOfEmployeeIds(employeeIds);

            // Assert
            result.Should().BeOfType(typeof(NotFoundResult));
        }
    }
}