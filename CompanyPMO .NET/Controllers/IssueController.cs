using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Common;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IIssue _issueService;
        private readonly IUserIdentity _userIdentityService;
        private readonly Lazy<int> _lazyUserId;

        public IssueController(IIssue issueService, IUserIdentity userIdentityService)
        {
            _issueService = issueService;
            _userIdentityService = userIdentityService;
            _lazyUserId = new Lazy<int>(InitializeUserId);
        }

        private int InitializeUserId()
        {
            return _userIdentityService.GetUserIdFromClaims(HttpContext.User);
        }

        private int GetUserId()
        {
            return _lazyUserId.Value;
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

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("new")]
        [ProducesResponseType(200, Type = typeof(OperationResult<int>))]
        [ProducesResponseType(400, Type = typeof(OperationResult<int>))]
        public async Task<IActionResult> NewIssue([FromBody] IssueDto issue, [FromQuery] int taskId, [FromQuery] bool shouldStartNow)
        {
            var result = await _issueService.CreateIssue(issue, GetUserId(), taskId, shouldStartNow);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
