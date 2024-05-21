using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IIssue _issueService;

        public IssueController(IIssue issueService)
        {
            _issueService = issueService;
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("all/showcase")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IssueShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllIssuesShowcase(int page, int pageSize)
        {
            var issues = await _issueService.GetAllIssuesShowcase(page, pageSize);

            if (issues is null || issues.Count == 0)
                return NotFound();

            return Ok(issues);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IssueDto>))]
        public async Task<IActionResult> GetAllIssues([FromQuery] FilterParams filterParams)
        {
            var issues = await _issueService.GetAllIssues(filterParams);

            return Ok(issues);
        }

        [HttpGet("{issueId}")]
        [ProducesResponseType(200, Type = typeof(EntityParticipantOrOwnerDTO<IssueDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetIssueById(int issueId, int taskId, int projectId)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var issue = await _issueService.GetIssueById(issueId, taskId, projectId, employeeId);

            if (issue is null)
            {
                return NotFound();
            }

            return Ok(issue);
        }
    }
}
