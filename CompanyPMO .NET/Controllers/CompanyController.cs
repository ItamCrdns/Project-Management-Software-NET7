using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompany _companyService;
        private readonly IUserIdentity _userIdentityService;
        private readonly Lazy<Task<int>> _lazyUserId;

        public CompanyController(ICompany companyService, IUserIdentity userIdentityService)
        {
            _companyService = companyService;
            _userIdentityService = userIdentityService;
            _lazyUserId = new Lazy<Task<int>>(InitializeUserId);
        }

        private async Task<int> InitializeUserId()
        {
            return await _userIdentityService.GetUserIdFromClaims(HttpContext.User);
        }

        private async Task<int> GetUserId()
        {
            return await _lazyUserId.Value;
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("new")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Company>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> NewCompany([FromForm] CompanyDto newCompany, [FromForm] List<IFormFile>? images)
        {
            if (images is not null && images.Count > 10)
            {
                ModelState.AddModelError("Images", "The request contains too many images (maximum allowed is 10).");
                return StatusCode(400, ModelState);
            }

            var (created, returnedCompany) = await _companyService.AddCompany(await GetUserId(), newCompany, images);

            if(!created)
            {
                return StatusCode(500, "Something went wrong");
            }

            return Ok(returnedCompany);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{companyId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Company>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCompanyById(int companyId)
        {
            var company = await _companyService.GetCompanyById(companyId);

            if(company is null)
            {
                return BadRequest();
            }

            return Ok(company);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPatch("{companyId}/update")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Company>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCompany(int companyId, [FromForm] CompanyDto companyDto, [FromForm] List<IFormFile>? images)
        {
            bool companyExists = await _companyService.DoesCompanyExist(companyId);

            if(!companyExists)
            {
                return NotFound();
            }

            if (images is not null && images.Count > 10)
            {
                ModelState.AddModelError("Images", "The request contains too many images (maximum allowed is 10).");
                return StatusCode(400, ModelState);
            }

            var (updated, company) = await _companyService.UpdateCompany(await GetUserId(), companyId, companyDto, images);

            if(!updated)
            {
                return BadRequest();
            }

            return Ok(company);
        }
    }
}
