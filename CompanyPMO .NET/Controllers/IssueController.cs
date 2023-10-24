using CompanyPMO_.NET.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    }
}
