using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
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
        private readonly IJwt _jwt;
        public AuthController(IEmployeeAuthentication employeeAuthentication, IJwt jwt)
        {
            _employeeAuthentication = employeeAuthentication;
            _jwt = jwt;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(LoginResponseDto))]
        public async Task<IActionResult> Login(EmployeeLoginDto employee)
        {
            var employeeAuthentication = await _employeeAuthentication.AuthenticateEmployee(employee.Username, employee.Password);

            if (employeeAuthentication.result.Authenticated)
            {
                var loggedEmployee = await _employeeAuthentication.GetEmployeeForClaims(employee.Username);

                var token = _jwt.JwtTokenGenerator(loggedEmployee);

                var loginResponse = new LoginResponseDto
                {
                    Result = employeeAuthentication.result,
                    Message = employeeAuthentication.message,
                    Employee = employeeAuthentication.employee,
                    Token = token
                };

                return Ok(loginResponse);
            }
            else if (!employeeAuthentication.result.Authenticated)
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


        [Authorize(Policy = "EmployeesAllowed")]
        [HttpPost("me/confirm-password")]
        [ProducesResponseType(200, Type = typeof(OperationResult<bool>))]
        public async Task<IActionResult> ConfirmPassword([FromBody] EmployeePasswordDto employee)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var isPasswordCorrect = await _employeeAuthentication.ConfirmPassword(employeeId, employee.Password);

            if (isPasswordCorrect == null)
            {
                return NotFound();
            }

            return Ok(isPasswordCorrect);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("me/password-last-verification")]
        [ProducesResponseType(200, Type = typeof(OperationResult<DateTime>))]
        public async Task<IActionResult> PasswordLastVerification()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var lastVerification = await _employeeAuthentication.PasswordLastVerification(employeeId);

            return Ok(lastVerification);
        }
    }
}
