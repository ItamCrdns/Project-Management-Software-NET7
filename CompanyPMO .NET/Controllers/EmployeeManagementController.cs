using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Hubs;
using CompanyPMO_.NET.Interfaces.Employee_interfaces;
using CompanyPMO_.NET.Interfaces.Timeline_interfaces;
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
        private readonly ITimelineManagement _timelineManagement;
        public EmployeeManagementController(IEmployeeManagement employeeManagement, ITimelineManagement timelineManagement)
        {
            _employeeManagement = employeeManagement;
            _timelineManagement = timelineManagement;
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

            var timelineEvent = new TimelineDto
            {
                Event = "registered",
                EmployeeId = newEmployee.EmployeeId,
                Type = TimelineType.Register
            };

            await _timelineManagement.CreateTimelineEvent(timelineEvent, UserRoles.Supervisor);

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

            var timelineEvent = new TimelineDto
            {
                Event = "updated their profile",
                EmployeeId = employeeId,
                Type = TimelineType.Update
            };

            await _timelineManagement.CreateTimelineEvent(timelineEvent, UserRoles.Supervisor);

            return Ok(result);
        }
    }
}
