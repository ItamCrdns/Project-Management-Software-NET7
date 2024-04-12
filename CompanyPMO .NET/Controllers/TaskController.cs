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
    [Authorize(Policy = "EmployeesAllowed")]
    public class TaskController : ControllerBase
    {
        private readonly ITask _taskService;
        private readonly IUserIdentity _userIdentityService;
        private readonly Lazy<int> _lazyUserId;
        private readonly IIssue _issueService;

        public TaskController(ITask taskService, IUserIdentity userIdentityService, IIssue issueService)
        {
            _taskService = taskService;
            _userIdentityService = userIdentityService;
            _lazyUserId = new Lazy<int>(InitializeUserId); // Load the userId until we actually need it
            _issueService = issueService;
        }

        private int InitializeUserId()
        {
            return _userIdentityService.GetUserIdFromClaims(HttpContext.User);
        }

        private int GetUserId()
        {
            return _lazyUserId.Value;
        }

        [HttpPost("new")]
        [ProducesResponseType(200, Type = typeof(OperationResult<int>))]
        [ProducesResponseType(400, Type = typeof(OperationResult<int>))]
        public async Task<IActionResult> NewTask([FromForm] TaskDto task, [FromForm] int projectId, [FromForm] List<IFormFile>? images, [FromForm] List<int> employees, [FromForm] bool shouldStartNow)
        {
            var result = await _taskService.CreateTask(task, GetUserId(), projectId, images, employees, shouldStartNow);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{taskId}")]
        [ProducesResponseType(200, Type = typeof(EntityParticipantOrOwnerDTO<TaskDto>))]
        public async Task<IActionResult> GetTaskById(int taskId, int projectId)
        {
            var task = await _taskService.GetTaskById(taskId, projectId, GetUserId());

            return Ok(task);
        }

        [HttpPost("{taskId}/start")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> StartTask(int taskId)
        {
            bool taskStarted = await _taskService.StartingWorkingOnTask(GetUserId(), taskId);

            if(!taskStarted)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPost("{taskId}/finish")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> FinishTask(int taskId)
        {
            bool taskFinished = await _taskService.FinishedWorkingOnTask(GetUserId(), taskId);

            if (!taskFinished)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpGet("{taskId}/employees")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        public async Task<IActionResult> GetEmployeesWorkingOnATask(int taskId)
        {
            var employees = await _taskService.GetEmployeesWorkingOnTask(taskId);

            return Ok(employees);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Models.Task>))]
        public async Task<IActionResult> GetAllTasks([FromQuery] FilterParams filterParams)
        {
            var tasks = await _taskService.GetAllTasks(filterParams);

            return Ok(tasks);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("all/showcase")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TaskShowcaseDto>))]
        public async Task<IActionResult> GetAllTasksShowcase(int page, int pageSize)
        {
            var tasks = await _taskService.GetAllTasksShowcase(page, pageSize);

            return Ok(tasks);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("{taskId}/employees/add")]
        [ProducesResponseType(200, Type = typeof(Dictionary<string, object>))]
        public async Task<IActionResult> AddEmployeesToTask(int taskId, [FromForm] List<int> employees)
        {
            var (response, employeesAdded) = await _taskService.AddEmployeesToTask(taskId, employees);

            if (response is null)
            {
                return BadRequest();
            }

            var toReturn = new
            {
                Status = response,
                EmployeesAdded = employeesAdded
            };

            return Ok(toReturn);
        }

        [HttpGet("grouped")]
        public async Task<IActionResult> GetTasksGroupedByProject([FromQuery] FilterParams filterParams, [FromQuery] int tasksPage = 1, [FromQuery] int tasksPageSize = 5)
        {
            var tasks = await _taskService.GetTasksGroupedByProject(filterParams, tasksPage, tasksPageSize, GetUserId());

            return Ok(tasks);
        }

        [HttpGet("{taskId}/issues/all")]
        [ProducesResponseType(200, Type = typeof(Dictionary<string, object>))]
        public async Task<IActionResult> GetIssuesByTaskId(int taskId, [FromQuery] FilterParams filterParams)
        {
            var issues = await _issueService.GetIssuesByTaskId(taskId, filterParams);

            return Ok(issues);
        }
    }
}
