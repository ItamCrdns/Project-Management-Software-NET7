using CompanyPMO_.NET.Controllers;
using CompanyPMO_.NET.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Controller
{
    public class IssueControllerTests
    {
        private readonly IssueController _issueController;
        private readonly IIssue _issueService;
        public IssueControllerTests()
        {
            _issueService = A.Fake<IIssue>();

            _issueController = new IssueController(_issueService);
        }

        //[Fact]
        //public async void IssueController_GetAllIssuesShowcase_ReturnsOk()
        //{
        //    // Arrange
        //    var page = 1;
        //    var pageSize = 10;

        //    // Act
        //    var result = await _issueController.GetAllIssuesShowcase(page, pageSize);

        //    // Assert
        //    result.Should().NotBeNull().And.BeAssignableTo<IActionResult>();
        //    result.Should().BeOfType(typeof(OkObjectResult));
        //}
    }
}
