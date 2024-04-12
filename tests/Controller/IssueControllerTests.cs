using CompanyPMO_.NET.Controllers;
using CompanyPMO_.NET.Interfaces;
using FakeItEasy;

namespace Tests.Controller
{
    public class IssueControllerTests
    {
        private readonly IssueController _issueController;
        private readonly IIssue _issueService;
        private readonly IUserIdentity _userIdentityService;
        public IssueControllerTests()
        {
            _issueService = A.Fake<IIssue>();
            _userIdentityService = A.Fake<IUserIdentity>();

            _issueController = new IssueController(_issueService, _userIdentityService);
        }
    }
}
