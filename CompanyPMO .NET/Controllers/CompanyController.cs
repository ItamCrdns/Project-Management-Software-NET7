using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Interfaces.Employee_interfaces;
using CompanyPMO_.NET.Interfaces.Project_interfaces;
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
        private readonly IEmployeeCompanyQueries _employeeCompanyQueriesService;
        private readonly IProjectCompanyQueries _projectCompanyQueries;

        public CompanyController(ICompany companyService, IEmployeeCompanyQueries employeeCompanyQueriesService, IProjectCompanyQueries projectCompanyQueries)
        {
            _companyService = companyService;
            _employeeCompanyQueriesService = employeeCompanyQueriesService;
            _projectCompanyQueries = projectCompanyQueries;
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
            var employees = await _employeeCompanyQueriesService.GetEmployeesByCompanyPaginated(companyId, page, pageSize);

            return Ok(employees);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("{companyId}/employees/search/{employeeToSearch}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeShowcaseDto>))]
        public async Task<IActionResult> SearchEmployeesByCompany(int companyId, string employeeToSearch, int page, int pageSize)
        {
            var employees = await _employeeCompanyQueriesService.SearchEmployeesByCompanyPaginated(employeeToSearch, companyId, page, pageSize);

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{clientId}/employees/by-projects-created")] // Returns all employees that have created projects in {clientId}
        public async Task<IActionResult> GetAndSearchEmployeesByProjectsCreatedInClient(string? employeeIds, int clientId, int page, int pageSize)
        {
            var employees = await _employeeCompanyQueriesService.GetAndSearchEmployeesByProjectsCreatedInClient(employeeIds, clientId, page, pageSize);

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{clientId}/projects/ongoing")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<ProjectDto>))]
        public async Task<IActionResult> GetOngoingProjectsByClient(int clientId, [FromQuery] FilterParams filterParams)
        {
            string filterBy = filterParams.FilterBy == null ? "startedWorking_finished_expectedDeliveryDate" : filterParams.FilterBy + "_startedWorking_finished_expectedDeliveryDate";
            string filterValue = filterParams.FilterValue == null ? "NotNull_NoValue_Ongoing" : filterParams.FilterValue + "_NotNull_NoValue_Ongoing";

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

            var projects = await _projectCompanyQueries.GetProjectsByCompanyName(clientId, interalFilterParams);

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

            var projects = await _projectCompanyQueries.GetProjectsByCompanyName(clientId, interalFilterParams);

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

            var projects = await _projectCompanyQueries.GetProjectsByCompanyName(clientId, interalFilterParams);

            return Ok(projects);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{clientId}/projects/not-started")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<ProjectDto>))]
        public async Task<IActionResult> GetNotStartedProjectsByClient(int clientId, [FromQuery] FilterParams filterParams)
        {
            string filterBy = filterParams.FilterBy == null ? "startedWorking_finished" : filterParams.FilterBy + "_startedWorking_finished";
            string filterValue = filterParams.FilterValue == null ? "NoValue_NoValue" : filterParams.FilterValue + "_NoValue_NoValue";

            FilterParams interalFilterParams = new()
            {
                Page = filterParams.Page,
                PageSize = filterParams.PageSize,
                OrderBy = filterParams.OrderBy,
                Sort = filterParams.Sort,
                SearchBy = filterParams.SearchBy,
                SearchValue = filterParams.SearchValue,
                FilterBy = filterBy,
                FilterValue = filterValue, //  x => x.StartedWorking == null && x.Finished == null
                FilterWhere = filterParams.FilterWhere,
                FilterWhereValue = filterParams.FilterWhereValue
            };

            var projects = await _projectCompanyQueries.GetProjectsByCompanyName(clientId, interalFilterParams);

            return Ok(projects);
        }
    }
}
