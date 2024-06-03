using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces.Company_interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyManagementController : ControllerBase
    {
        private readonly ICompanyManagement _companyManagement;
        public CompanyManagementController(ICompanyManagement companyManagement)
        {
            _companyManagement = companyManagement;
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Company>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> NewCompany([FromForm] CompanyDto newCompany, [FromForm] List<IFormFile>? images, IFormFile? logoFile)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            if (images is not null && images.Count > 10)
            {
                ModelState.AddModelError("Images", "The request contains too many images (maximum allowed is 10).");
                return StatusCode(400, ModelState);
            }

            var (created, returnedCompany) = await _companyManagement.AddCompany(employeeId, newCompany, images, logoFile);

            if (!created)
            {
                return StatusCode(500, "Something went wrong");
            }

            return Ok(returnedCompany);
        }


        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("create/nameonly")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CreateNewClientWithNameOnly([FromBody] CompanyDto newCompany)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            if (newCompany.Name is null)
            {
                ModelState.AddModelError("Name", "The name of the company is required.");
                return StatusCode(400, ModelState);
            }

            int companyId = await _companyManagement.CreateNewCompany(employeeId, newCompany.Name);

            if (companyId.Equals(0))
            {
                return StatusCode(500, "Something went wrong");
            }

            return Ok(companyId);
        }
    }
}
