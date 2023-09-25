using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employeeService;

        public EmployeeController(IEmployee employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("login")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(EmployeeDto))]
        public async Task<IActionResult> Login([FromForm] EmployeeLoginDto employee)
        {
            var employeeAuthentication = await _employeeService.AuthenticateEmployee(employee.Username, employee.Password);

            if(employeeAuthentication.authenticated)
            {
                var loggedEmployee = await _employeeService.GetEmployeeForClaims(employee.Username);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, loggedEmployee.EmployeeId.ToString()),
                    new Claim(ClaimTypes.Name, loggedEmployee.Username),
                    new Claim(ClaimTypes.Role, loggedEmployee.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                var loginResponse = new LoginResponseDto
                {
                    Authenticated = employeeAuthentication.authenticated,
                    Message = employeeAuthentication.result,
                    Employee = employeeAuthentication.employee
                };

                return Ok(loginResponse);
            }
            else if(!employeeAuthentication.authenticated)
            {
                return Unauthorized();
            }

            return NoContent();
        }

        [HttpPost("register")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RegisterEmployee([FromForm] EmployeeRegisterDto employee, [FromForm] IFormFile? profilePicture)
        {
            bool newEmployee = await _employeeService.RegisterEmployee(employee, profilePicture);

            return Ok(new { Created = newEmployee });
        }

        [HttpGet("{employeeId}")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            Employee employee = await _employeeService.GetEmployeeById(employeeId);

            return Ok(employee);
        }
    }
}
