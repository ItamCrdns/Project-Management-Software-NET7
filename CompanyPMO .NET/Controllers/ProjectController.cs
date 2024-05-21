using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces.Employee_interfaces;
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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectQueries _projectQueries;
        private readonly ITaskProjectQueries _taskProjectQueries;
        private readonly IEmployeeProjectQueries _employeeProjectQueriesService;

        public ProjectController(IProjectQueries projectQueries, ITaskProjectQueries taskProjectQueries, IEmployeeProjectQueries employeeProjectQueriesService)
        {
            _projectQueries = projectQueries;
            _taskProjectQueries = taskProjectQueries;
            _employeeProjectQueriesService = employeeProjectQueriesService;
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectDto>))]
        public async Task<IActionResult> GetAllProjects([FromQuery] FilterParams filterParams)
        {
            var projects = await _projectQueries.GetAllProjects(filterParams);

            return Ok(projects);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("all/showcase")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectShowcaseDto>))]
        public async Task<IActionResult> GetAllProjectsShowcase(int page, int pageSize)
        {
            var projects = await _projectQueries.GetAllProjectsShowcase(page, pageSize);

            return Ok(projects);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("all/groupedbycompany")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<CompanyProjectGroup>))]
        public async Task<IActionResult> GetProjectsGroupedByCompany([FromQuery] FilterParams filterParams, [FromQuery] int projectsPage = 1, [FromQuery] int projectsPageSize = 5)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var projects = await _projectQueries.GetProjectsGroupedByCompany(filterParams, projectsPage, projectsPageSize, employeeId);

            return Ok(projects);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{projectId}")]
        [ProducesResponseType(200, Type = typeof(EntityParticipantOrOwnerDTO<ProjectDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var project = await _projectQueries.GetProjectById(projectId, employeeId);

            if (project is null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{projectId}/tasks/{taskId}")]
        [ProducesResponseType(200, Type = typeof(EntityParticipantOrOwnerDTO<TaskDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTaskById(int projectId, int taskId)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var task = await _taskProjectQueries.GetTaskById(taskId, projectId, employeeId);

            if (task is null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{projectId}/limited")]
        [ProducesResponseType(200, Type = typeof(EntityParticipantOrOwnerDTO<ProjectSomeInfoDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProjectNameCreatorAndTeam(int projectId)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var project = await _projectQueries.GetProjectNameCreatorLifecyclePriorityAndTeam(projectId);

            if (project is null)
            {
                return NotFound();
            }

            bool isParticipantOfProject = await _projectQueries.IsParticipant(projectId, employeeId);

            bool isOwner = false;
            if (!isParticipantOfProject)
            {
                isOwner = await _projectQueries.IsOwner(projectId, employeeId);
            }

            var result = new EntityParticipantOrOwnerDTO<ProjectSomeInfoDto>
            {
                Entity = project,
                IsParticipant = isParticipantOfProject,
                IsOwner = isOwner
            };

            return Ok(result);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{projectId}/employees")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<EmployeeShowcaseDto>))]
        public async Task<IActionResult> GetProjectEmployees(int projectId, int page, int pageSize)
        {
            var employees = await _employeeProjectQueriesService.GetProjectEmployees(projectId, page, pageSize);

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{projectId}/employees/search/{employeeToSearch}")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<EmployeeShowcaseDto>))]
        public async Task<IActionResult> SearchProjectEmployees(int projectId, string employeeToSearch, int page, int pageSize)
        {
            var employees = await _employeeProjectQueriesService.SearchProjectEmployees(employeeToSearch, projectId, page, pageSize);

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{projectId}/tasks/all")]
        [ProducesResponseType(200, Type = typeof(Dictionary<string, object>))]
        public async Task<IActionResult> GetTasksByProjectId(int projectId, [FromQuery] FilterParams filterParams)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var tasks = await _taskProjectQueries.GetTasksByProjectId(projectId, filterParams);

            bool isParticipantOfProject = await _projectQueries.IsParticipant(projectId, employeeId);

            bool isOwner = false;
            if (!isParticipantOfProject)
            {
                isOwner = await _projectQueries.IsOwner(projectId, employeeId);
            }

            var result = new Dictionary<string, object>
            {
                { "entity", tasks },
                { "isProjectParticipant", isParticipantOfProject },
                { "isProjectOwner", isOwner }
            };
 
            return Ok(result);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{projectId}/tasks/all/showcase")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<TaskShowcaseDto>))]
        public async Task<IActionResult> GetTasksShowcaseByProjectId(int projectId, int page, int pageSize)
        {
            var tasks = await _taskProjectQueries.GetTasksShowcaseByProjectId(projectId, page, pageSize);

            return Ok(tasks);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{projectId}/showcase")]
        [ProducesResponseType(200, Type = typeof(ProjectShowcaseDto))]
        public async Task<IActionResult> GetProjectShowcase(int projectId)
        {
            var project = await _projectQueries.GetProjectShowcase(projectId);

            return Ok(project);
        }
    }
}
