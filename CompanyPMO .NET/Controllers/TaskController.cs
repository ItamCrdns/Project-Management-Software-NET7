using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITask _taskService;

        public TaskController(ITask taskService)
        {
            _taskService = taskService;
        }

        [Authorize(Roles = "supervisor, employee")]
        [HttpPost("new")]
        [ProducesResponseType(200, Type = typeof(Models.Task))]
        public async Task<IActionResult> NewTask([FromForm] Models.Task task, [FromForm] int projectId, [FromForm] List<IFormFile>? images)
        {
            // * Access control
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _ = int.TryParse(userIdClaim, out int userId);

            var (newTask, imageCollection) = await _taskService.CreateTask(task, userId, projectId, images);

            var taskDto = new Models.Task
            {
                TaskId = newTask.TaskId,
                Name = newTask.Name,
                Description = newTask.Description,
                Created = DateTimeOffset.UtcNow,
                TaskCreatorId = userId,
                Images = imageCollection
            };

            return Ok(taskDto);
        }

        [Authorize(Roles = "supervisor, employee")]
        [HttpGet("{taskId}")]
        [ProducesResponseType(200, Type = typeof(Models.Task))]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            var task = await _taskService.GetTaskById(taskId);

            return Ok(task);
        }

        [Authorize(Roles = "supervisor, employee")]
        [HttpGet("project/{projectId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Models.Task>))]
        public async Task<IActionResult> GetTasksByProjectId(int projectId)
        {
            var tasks = await _taskService.GetTasksByProjectId(projectId);

            return Ok(tasks);
        }

        [Authorize(Roles = "supervisor, employee")]
        [HttpPost("{taskId}/start")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> StartTask(int taskId)
        {
            // * Access control
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _ = int.TryParse(userIdClaim, out int userId);

            bool taskStarted = await _taskService.StartingWorkingOnTask(userId, taskId);

            if(!taskStarted)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [Authorize(Roles = "supervisor, employee")]
        [HttpPost("{taskId}/finish")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> FinishTask(int taskId)
        {
            // * Access control
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _ = int.TryParse(userIdClaim, out int userId);

            bool taskFinished = await _taskService.FinishedWorkingOnTask(userId, taskId);

            if (!taskFinished)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [Authorize(Roles = "supervisor, employee")]
        [HttpGet("{taskId}/employees")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        public async Task<IActionResult> GetEmployyesWorkingOnATask(int taskId)
        {
            var employees = await _taskService.GetEmployeesWorkingOnTask(taskId);

            return Ok(employees);
        }
    }
}
