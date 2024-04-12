﻿using CompanyPMO_.NET.Dto;
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
        private readonly IEmployee _employeeService;
        private readonly Lazy<int> _lazyUserId;

        public CompanyController(ICompany companyService, IUserIdentity userIdentityService, IEmployee employeeService)
        {
            _companyService = companyService;
            _userIdentityService = userIdentityService;
            _employeeService = employeeService;
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
        [HttpPost("new")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Company>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> NewCompany([FromForm] CompanyDto newCompany, [FromForm] List<IFormFile>? images, IFormFile? logoFile)
        {
            if (images is not null && images.Count > 10)
            {
                ModelState.AddModelError("Images", "The request contains too many images (maximum allowed is 10).");
                return StatusCode(400, ModelState);
            }

            var (created, returnedCompany) = await _companyService.AddCompany(GetUserId(), newCompany, images, logoFile);

            if (!created)
            {
                return StatusCode(500, "Something went wrong");
            }

            return Ok(returnedCompany);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("new/nameonly")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CreateNewClientWithNameOnly([FromBody] CompanyDto newCompany)
        {
            if (newCompany.Name is null)
            {
                ModelState.AddModelError("Name", "The name of the company is required.");
                return StatusCode(400, ModelState);
            }

            int companyId = await _companyService.CreateNewCompany(GetUserId(), newCompany.Name);

            if (companyId.Equals(0))
            {
                return StatusCode(500, "Something went wrong");
            }

            return Ok(companyId);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{companyId}")]
        [ProducesResponseType(200, Type = typeof(Company))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCompanyById(int companyId)
        {
            var company = await _companyService.GetCompanyById(companyId);

            if (company is null)
            {
                return NotFound();
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

            if (!companyExists)
            {
                return NotFound();
            }

            // To do: handle the case when the entity has for example 7 values and you pass 5, only the first 3 will be uploaded and the other 2 will not, no error will be given (and it should be an error!)
            if (images is not null && images.Count > 10)
            {
                ModelState.AddModelError("Images", "The request contains too many images (maximum allowed is 10).");
                return StatusCode(400, ModelState);
            }

            var (updated, company) = await _companyService.UpdateCompany(GetUserId(), companyId, companyDto, images);

            if (!updated)
            {
                return BadRequest();
            }

            return Ok(company);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("all/withprojects")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CompanyShowcaseDto>))]
        public async Task<IActionResult> GetCompaniesThatHaveProjects()
        {
            var companies = await _companyService.GetCompaniesThatHaveProjects();

            return Ok(companies);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CompanyShowcaseDto>))]
        public async Task<IActionResult> GetAllCompanies(int page, int pageSize)
        {
            var companies = await _companyService.GetAllCompanies(page, pageSize);

            return Ok(companies);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("{companyId}/employees")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeShowcaseDto>))]
        public async Task<IActionResult> GetEmployeesByCompanyId(int companyId, int page, int pageSize)
        {
            var employees = await _employeeService.GetEmployeesByCompanyPaginated(companyId, page, pageSize);

            return Ok(employees);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("{companyId}/employees/search/{employeeToSearch}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeShowcaseDto>))]
        public async Task<IActionResult> SearchEmployeesByCompany(int companyId, string employeeToSearch, int page, int pageSize)
        {
            var employees = await _employeeService.SearchEmployeesByCompanyPaginated(employeeToSearch, companyId, page, pageSize);

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{clientId}/employees/by-projects-created")] // Returns all employees that have created projects in {clientId}
        public async Task<IActionResult> GetAndSearchEmployeesByProjectsCreatedInClient(string? employeeIds, int clientId, int page, int pageSize)
        {
            var employees = await _employeeService.GetAndSearchEmployeesByProjectsCreatedInClient(employeeIds, clientId, page, pageSize);

            return Ok(employees);
        }
    }
}
