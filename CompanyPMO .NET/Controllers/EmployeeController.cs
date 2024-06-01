using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces.Employee_interfaces;
using CompanyPMO_.NET.Interfaces.Issue_interfaces;
using CompanyPMO_.NET.Interfaces.Project_interfaces;
using CompanyPMO_.NET.Interfaces.Task_interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeQueries _employeeQueries;
        private readonly IProjectEmployeeQueries _projectEmployeeQueries;
        private readonly ITaskEmployeeQueries _taskEmployeeQueries;
        private readonly IIssueEmployeeQueries _issueEmployeeQueries;
        public EmployeeController(IEmployeeQueries employeeQueries, IProjectEmployeeQueries projectEmployeeQueries, ITaskEmployeeQueries taskEmployeeQueries, IIssueEmployeeQueries issueEmployeeQueries)
        {
            _taskEmployeeQueries = taskEmployeeQueries;
            _employeeQueries = employeeQueries;
            _projectEmployeeQueries = projectEmployeeQueries;
            _issueEmployeeQueries = issueEmployeeQueries;
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("{employeeId}")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            Employee employee = await _employeeQueries.GetEmployeeById(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("my-team")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<EmployeeShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployeesBySupervisorId([FromQuery] FilterParams filterParams)
        {
            var supervisorId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var employees = await _employeeQueries.GetEmployeesBySupervisorId(supervisorId, filterParams);

            if (employees == null || employees.Count == 0)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("username/{username}")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployeeByUsername(string username)
        {
            EmployeeDto employee = await _employeeQueries.GetEmployeeByUsername(username);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/colleagues")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeShowcaseDto>))]
        public async Task<IActionResult> GetEmployeesWorkingInTheSameCompany(string username, int page, int pageSize)
        {
            var employees = await _employeeQueries.GetEmployeesWorkingInTheSameCompany(username, page, pageSize);

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/colleagues/search/{employeeToSearch}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeShowcaseDto>))]
        public async Task<IActionResult> SearchEmployeesWorkingInTheSameCompany(string username, string employeeToSearch, int page, int pageSize)
        {
            var employees = await _employeeQueries.SearchEmployeesWorkingInTheSameCompany(employeeToSearch, username, page, pageSize);

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<EmployeeShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployeesShowcasePaginated(int page, int pageSize)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var employees = await _employeeQueries.GetEmployeesShowcasePaginated(employeeId, page, pageSize);

            if (employees.Count == 0)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("all/search/{employee}")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<EmployeeShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SearchEmployeesShowcasePaginated(string employee, int page, int pageSize)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var employees = await _employeeQueries.SearchEmployeesShowcasePaginated(employeeId, employee, page, pageSize);

            if (employees.Count == 0)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/projects/all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProjectsByEmployeeUsername(string username, [FromQuery] FilterParams filterParams)
        {
            var projects = await _projectEmployeeQueries.GetProjectsByEmployeeUsername(username, filterParams);

            if (projects == null || projects.Data == null || !projects.Data.Any())
            {
                return NotFound();
            }

            return Ok(projects);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/projects/showcase")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProjectsShowcaseByEmployeeUsername(string username, int page, int pageSize)
        {
            var projects = await _projectEmployeeQueries.GetProjectsShowcaseByEmployeeUsername(username, page, pageSize);

            if (projects == null || projects.Data == null || !projects.Data.Any())
            {
                return NotFound();
            }

            return Ok(projects);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/tasks/showcase")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TaskShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTasksShowcaseByEmployeeUsername(string username, int page, int pageSize)
        {
            var tasks = await _taskEmployeeQueries.GetTasksShowcaseByEmployeeUsername(username, page, pageSize);

            if (tasks == null || tasks.Data == null || !tasks.Data.Any())
            {
                return NotFound();
            }

            return Ok(tasks);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/tasks/all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TaskShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTasksByEmployeeUsername(string username, int page, int pageSize)
        {
            var tasks = await _taskEmployeeQueries.GetTasksByEmployeeUsername(username, page, pageSize);

            if (tasks == null || tasks.Data == null || !tasks.Data.Any())
            {
                return NotFound();
            }

            return Ok(tasks);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/issues/showcase")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IssueShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetIssuesByEmployeeUsername(string username, int page, int pageSize)
        {
            var issues = await _issueEmployeeQueries.GetIssuesShowcaseByEmployeeUsername(username, page, pageSize);

            if (issues == null || issues.Data == null || !issues.Data.Any())
            {
                return NotFound();
            }

            return Ok(issues);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("me/tier")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        public async Task<IActionResult> GetEmployeeByIdForClaims()
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            TierDto tier = await _employeeQueries.GetEmployeeTier(employeeId);

            return Ok(tier);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("me")]
        [ProducesResponseType(200, Type = typeof(EmployeeDto))]
        public async Task<IActionResult> GetMyEmployee()
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var employee = await _employeeQueries.GetEmployeeById(employeeId);

            return Ok(employee);
        }
    }
}
