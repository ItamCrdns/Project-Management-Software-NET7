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
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

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
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

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

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPatch("{companyId}/update")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Company>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCompany(int companyId, [FromForm] CompanyDto companyDto, [FromForm] List<IFormFile>? images)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            // To do: handle the case when the entity has for example 7 values and you pass 5, only the first 3 will be uploaded and the other 2 will not, no error will be given (and it should be an error!)
            if (images is not null && images.Count > 10)
            {
                ModelState.AddModelError("Images", "The request contains too many images (maximum allowed is 10).");
                return StatusCode(400, ModelState);
            }

            var (updated, company) = await _companyManagement.UpdateCompany(employeeId, companyId, companyDto, images);

            if (!updated)
            {
                return BadRequest();
            }

            return Ok(company);
        }
    }
}
