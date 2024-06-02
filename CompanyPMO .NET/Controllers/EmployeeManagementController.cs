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
    public class EmployeeManagementController : ControllerBase
    {
        private readonly IEmployeeManagement _employeeManagement;
        public EmployeeManagementController(IEmployeeManagement employeeManagement)
        {
            _employeeManagement = employeeManagement;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RegisterEmployee([FromForm] EmployeeRegisterDto employee, [FromForm] IFormFile? profilePicture)
        {
            var (result, status, newEmployee) = await _employeeManagement.RegisterEmployee(employee, profilePicture);

            if (!status)
            {
                return BadRequest(new { Created = status, Message = "Employee could not be created" });
            }

            return Ok(new { Created = status, Message = result, newEmployee });
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpPatch("me/update")]
        [ProducesResponseType(200, Type = typeof(OperationResult<EmployeeShowcaseDto>))]
        [ProducesErrorResponseType(typeof(OperationResult<EmployeeShowcaseDto>))]
        public async Task<IActionResult> UpdateMyEmployee([FromForm] UpdateEmployeeDto employee, [FromForm] IFormFile? profilePicture, [FromForm] string? currentPassword)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var result = await _employeeManagement.UpdateEmployee(employeeId, employee, profilePicture, currentPassword);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
