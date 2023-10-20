﻿using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employeeService;
        private readonly IJwt _jwtService;
        private readonly IProject _projectService;
        private readonly ITask _taskService;

        public EmployeeController(IEmployee employeeService, IJwt jwtService, IProject projectService, ITask taskService)
        {
            _employeeService = employeeService;
            _jwtService = jwtService;
            _projectService = projectService;
            _taskService = taskService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(EmployeeDto))]
        public async Task<IActionResult> Login([FromForm] EmployeeLoginDto employee)
        {
            var employeeAuthentication = await _employeeService.AuthenticateEmployee(employee.Username, employee.Password);

            if(employeeAuthentication.result.Authenticated)
            {
                var loggedEmployee = await _employeeService.GetEmployeeForClaims(employee.Username);

                var token = _jwtService.JwtTokenGenerator(loggedEmployee);

                HttpContext.Response.Cookies.Append("JwtToken", token, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true, // No HTTPS
                    IsEssential = true
                });

                var loginResponse = new LoginResponseDto
                {
                    Result = employeeAuthentication.result,
                    Message = employeeAuthentication.message,
                    Employee = employeeAuthentication.employee
                };

                return Ok(loginResponse);
            }
            else if(!employeeAuthentication.result.Authenticated)
            {
                var loginResponse = new LoginResponseDto
                {
                    Result = employeeAuthentication.result,
                    Message = employeeAuthentication.message,
                    Employee = employeeAuthentication.employee
                };

                return Unauthorized(loginResponse);
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RegisterEmployee([FromForm] EmployeeRegisterDto employee, [FromForm] IFormFile? profilePicture)
        {
            var (result, status) = await _employeeService.RegisterEmployee(employee, profilePicture);

            return Ok(new { Created = status, Message = result });
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("{employeeId}")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            Employee employee = await _employeeService.GetEmployeeById(employeeId);

            return Ok(employee);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("supervisor/{supervisorId}/employees")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        public async Task<IActionResult> GetEmployeesBySupervisorId(int supervisorId)
        {
            IEnumerable<Employee> employees = await _employeeService.GetEmployeeBySupervisorId(supervisorId);

            return Ok(employees);
        }

        [HttpPost("logout")]
        [ProducesResponseType(204)]
        public IActionResult EmployeeLogout()
        {
            Response.Cookies.Delete("JwtToken");

            return NoContent();
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("username/{username}")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        public async Task<IActionResult> GetEmployeeByUsername(string username)
        {
            EmployeeDto employee = await _employeeService.GetEmployeeByUsername(username);

            return Ok(employee);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("username/{username}/colleagues")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        public async Task<IActionResult> GetEmployeesWorkingInTheSameCompany(string username, int page, int pageSize)
        {
            IEnumerable<EmployeeDto> employees = await _employeeService.GetEmployeesWorkingInTheSameCompany(username, page, pageSize);

            return Ok(employees);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeShowcaseDto>))]
        public async Task<IActionResult> GetEmployeesShowcasePaginated(int page, int pageSize)
        {
            IEnumerable<EmployeeShowcaseDto> employees = await _employeeService.GetEmployeesShowcasePaginated(page, pageSize);

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/projects")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectDto>))]
        public async Task<IActionResult> GetProjectsByEmployeeUsername(string username, int page, int pageSize)
        {
            var projects = await _projectService.GetProjectsGroupedByUsername(username, page, pageSize);

            return Ok(projects);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/tasks")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TaskShowcaseDto>))]
        public async Task<IActionResult> GetTasksByEmployeeUsername(string username, int page, int pageSize)
        {
            var tasks = await _taskService.GetTasksByEmployeeUsername(username, page, pageSize);

            return Ok(tasks);
        }
    }
}
