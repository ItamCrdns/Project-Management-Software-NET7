using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Interfaces.Issue_interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "EmployeesAllowed")]
    public class TaskController : ControllerBase
    {
        private readonly ITask _taskService;
        private readonly IIssueTaskQueries _issueTaskQueries;

        public TaskController(ITask taskService, IIssueTaskQueries issueTaskQueries)
        {
            _taskService = taskService;
            _issueTaskQueries = issueTaskQueries;
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

        [HttpGet("grouped")]
        public async Task<IActionResult> GetTasksGroupedByProject([FromQuery] FilterParams filterParams, [FromQuery] int tasksPage = 1, [FromQuery] int tasksPageSize = 5)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var tasks = await _taskService.GetTasksGroupedByProject(filterParams, tasksPage, tasksPageSize, employeeId);

            return Ok(tasks);
        }

        [HttpGet("{taskId}/issues/all")]
        [ProducesResponseType(200, Type = typeof(Dictionary<string, object>))]
        public async Task<IActionResult> GetIssuesByTaskId(int taskId, [FromQuery] FilterParams filterParams)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var issues = await _issueTaskQueries.GetIssuesByTaskId(taskId, filterParams);

            bool isTaskParticipant = await _taskService.IsParticipant(taskId, employeeId);

            bool isOwner = await _taskService.IsOwner(taskId, employeeId);

            var result = new Dictionary<string, object>
            {
                { "entity", issues },
                { "isTaskParticipant", isTaskParticipant },
                { "isTaskOwner", isOwner }
            };

            return Ok(result);
        }
    }
}
