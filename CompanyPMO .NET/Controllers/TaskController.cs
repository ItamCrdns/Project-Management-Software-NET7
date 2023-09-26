using CompanyPMO_.NET.Interfaces;
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
    }
}
