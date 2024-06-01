using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces.Employee_interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IEmployeeAuthentication _employeeAuthentication;
        public AuthController(IEmployeeAuthentication employeeAuthentication)
        {
            _employeeAuthentication = employeeAuthentication;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(LoginResponseDto))]
        public async Task<IActionResult> Login(EmployeeLoginDto employee)
        {
            var authRequest = await _employeeAuthentication.AuthenticateEmployee(employee.Username, employee.Password);

            if (!authRequest.Result.Authenticated)
                return Ok(authRequest);
            else
                return Unauthorized(authRequest);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpPost("me/confirm-password")]
        [ProducesResponseType(200, Type = typeof(OperationResult<bool>))]
        public async Task<IActionResult> ConfirmPassword([FromBody] EmployeePasswordDto employee)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var isPasswordCorrect = await _employeeAuthentication.ConfirmPassword(employeeId, employee.Password);

            return Ok(isPasswordCorrect);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("me/password-last-verification")]
        [ProducesResponseType(200, Type = typeof(OperationResult<DateTime>))]
        public async Task<IActionResult> PasswordLastVerification()
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var lastVerification = await _employeeAuthentication.PasswordLastVerification(employeeId);

            return Ok(lastVerification);
        }
    }
}
