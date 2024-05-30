using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Hubs;
using CompanyPMO_.NET.Interfaces.Issue_interfaces;
using CompanyPMO_.NET.Interfaces.Timeline_interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueManagementController : ControllerBase
    {
        private readonly IIssueManagement _issueManagement;
        private readonly ITimelineManagement _timelineManagement;
        public IssueManagementController(IIssueManagement issueManagement, ITimelineManagement timelineManagement)
        {
            _issueManagement = issueManagement;
            _timelineManagement = timelineManagement;
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(OperationResult<int>))]
        [ProducesResponseType(400, Type = typeof(OperationResult<int>))]
        public async Task<IActionResult> CreateIssue([FromBody] IssueDto issue, [FromQuery] int taskId, [FromQuery] bool shouldStartNow)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var result = await _issueManagement.CreateIssue(issue, employeeId, taskId, shouldStartNow);

            if (!result.Success)
                return BadRequest(result);

            var timelineEvent = new TimelineDto
            {
                Event = "reported the issue",
                EmployeeId = employeeId,
                Type = TimelineType.Report,
                IssueId = result.Data
            };

            await _timelineManagement.CreateTimelineEvent(timelineEvent, UserRoles.Supervisor);

            return Ok(result);
        }
    }
}
