using CompanyPMO_.NET.Common;
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
        private readonly IEmployee _employeeService;
        private readonly IProject _projectService;
        private readonly Lazy<int> _lazyUserId;

        public CompanyController(ICompany companyService, IUserIdentity userIdentityService, IEmployee employeeService, IProject projectService)
        {
            _companyService = companyService;
            _userIdentityService = userIdentityService;
            _employeeService = employeeService;
            _projectService = projectService;
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

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{clientId}/projects/ongoing")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<ProjectDto>))]
        public async Task<IActionResult> GetOngoingProjectsByClient(int clientId, [FromQuery] FilterParams filterParams)
        {
            string filterBy = filterParams.FilterBy == null ? "finished_expectedDeliveryDate" : filterParams.FilterBy + "_finished_expectedDeliveryDate";
            string filterValue = filterParams.FilterValue == null ? "NoValue_Ongoing" : filterParams.FilterValue + "_NoValue_Ongoing";

            // New instance of FilterParams to ensure that the FilterBy and FilterValue are set correctly and not overwritten by the client
            FilterParams interalFilterParams = new()
            {
                Page = filterParams.Page,
                PageSize = filterParams.PageSize,
                OrderBy = filterParams.OrderBy,
                Sort = filterParams.Sort,
                SearchBy = filterParams.SearchBy,
                SearchValue = filterParams.SearchValue,
                FilterBy = filterBy,
                FilterValue = filterValue,
                FilterWhere = filterParams.FilterWhere,
                FilterWhereValue = filterParams.FilterWhereValue
            };

            var projects = await _projectService.GetProjectsByCompanyName(clientId, interalFilterParams);

            return Ok(projects);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{clientId}/projects/finished")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<ProjectDto>))]
        public async Task<IActionResult> GetFinishedProjectsByClient(int clientId, [FromQuery] FilterParams filterParams)
        {
            string filterBy = filterParams.FilterBy == null ? "finished" : filterParams.FilterBy + "_finished";
            string filterValue = filterParams.FilterValue == null ? "NotNull" : filterParams.FilterValue + "_NotNull";

            // New instance of FilterParams to ensure that the FilterBy and FilterValue are set correctly and not overwritten by the client
            FilterParams interalFilterParams = new()
            {
                Page = filterParams.Page,
                PageSize = filterParams.PageSize,
                OrderBy = filterParams.OrderBy,
                Sort = filterParams.Sort,
                SearchBy = filterParams.SearchBy,
                SearchValue = filterParams.SearchValue,
                FilterBy = filterBy,
                FilterValue = filterValue, // x => x.Finished != null
                FilterWhere = filterParams.FilterWhere,
                FilterWhereValue = filterParams.FilterWhereValue
            };

            var projects = await _projectService.GetProjectsByCompanyName(clientId, interalFilterParams);

            return Ok(projects);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{clientId}/projects/overdue")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<ProjectDto>))]
        public async Task<IActionResult> GetOverdueProjectsByClient(int clientId, [FromQuery] FilterParams filterParams)
        {
            string filterBy = filterParams.FilterBy == null ? "expectedDeliveryDate" : filterParams.FilterBy + "_expectedDeliveryDate";
            string filterValue = filterParams.FilterValue == null ? "RightNowDate" : filterParams.FilterValue + "_RightNowDate";

            // New instance of FilterParams to ensure that the FilterBy and FilterValue are set correctly and not overwritten by the client
            FilterParams interalFilterParams = new()
            {
                Page = filterParams.Page,
                PageSize = filterParams.PageSize,
                OrderBy = filterParams.OrderBy,
                Sort = filterParams.Sort,
                SearchBy = filterParams.SearchBy,
                SearchValue = filterParams.SearchValue,
                FilterBy = filterBy,
                FilterValue = filterValue, // x => x.ExpectedDeliveryDate < DateTime.Now
                FilterWhere = filterParams.FilterWhere,
                FilterWhereValue = filterParams.FilterWhereValue
            };

            var projects = await _projectService.GetProjectsByCompanyName(clientId, interalFilterParams);

            return Ok(projects);
        }
    }
}
