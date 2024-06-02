using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces.Issue_interfaces;
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
        public IssueManagementController(IIssueManagement issueManagement)
        {
            _issueManagement = issueManagement;
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(OperationResult<int>))]
        [ProducesResponseType(400, Type = typeof(OperationResult<int>))]
        public async Task<IActionResult> CreateIssue([FromBody] IssueDto issue, [FromQuery] int taskId, [FromQuery] bool shouldStartNow)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var result = await _issueManagement.CreateIssue(issue, employeeId, taskId, shouldStartNow);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
