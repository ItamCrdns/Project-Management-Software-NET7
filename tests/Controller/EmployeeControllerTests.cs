using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Controllers;
using CompanyPMO_.NET.Dto;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using FakeItEasy;
using Microsoft.AspNetCore.Http;

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


        public EmployeeControllerTests()
        {
            _employeeService = A.Fake<IEmployee>();
            _jwtService = A.Fake<IJwt>();
            _projectService = A.Fake<IProject>();
            _taskService = A.Fake<ITask>();
            _issueService = A.Fake<IIssue>();
            _userIdentityService = A.Fake<IUserIdentity>();

            _employeeController = new EmployeeController(
                _employeeService,
                _jwtService,
                _projectService,
                _taskService,
                _issueService,
                _userIdentityService
                );
        }

        [Fact]
        public async Task EmployeeController_GetEmployeesFromAListOfEmployeeIds_ReturnOk()
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
        public async Task EmployeeController_GetEmployeesFromAListOfEmployeeIds_ReturnNotFound()
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

        [Fact]
        public async Task EmployeeController_Login_ReturnOk()
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

            A.CallTo(() => _employeeService.AuthenticateEmployee(employee.Username, employee.Password)).Returns(Task.FromResult(loginResultTuple));
            _employeeController.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await _employeeController.Login(employee);

            // Assert

            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async Task EmployeeController_Login_ReturnUnauthorized()
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

            A.CallTo(() => _employeeService.AuthenticateEmployee(employee.Username, employee.Password)).Returns(Task.FromResult(loginResultTuple));
            _employeeController.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await _employeeController.Login(employee);

            // Assert
            result.Should().BeOfType(typeof(UnauthorizedObjectResult));
        }
    }
}