using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Hubs;
using CompanyPMO_.NET.Interfaces.Timeline_interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimelineManagementController : ControllerBase
    {
        private readonly ITimelineManagement _timelineManagement;
        public TimelineManagementController(ITimelineManagement timelineManagement)
        {
            _timelineManagement = timelineManagement;
        }
        [Authorize(Policy = "EmployeesAllowed")]
        [HttpPost("on-logout")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddTimelineEventOnUserLogout()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var timeline = new TimelineDto
            {
                Event = "logged out",
                EmployeeId = employeeId,
                Type = TimelineType.Logout
            };

            var result = await _timelineManagement.CreateTimelineEvent(timeline, UserRoles.Supervisor);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
