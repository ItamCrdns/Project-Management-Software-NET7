using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Hubs;
using CompanyPMO_.NET.Interfaces.Task_interfaces;
using CompanyPMO_.NET.Interfaces.Timeline_interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskManagementController : ControllerBase
    {
        private readonly ITaskManagement _taskManagement;
        private readonly ITimelineManagement _timelineManagement;
        public TaskManagementController(ITaskManagement taskManagement, ITimelineManagement timelineManagement)
        {
            _taskManagement = taskManagement;
            _timelineManagement = timelineManagement;
        }

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(OperationResult<int>))]
        [ProducesResponseType(400, Type = typeof(OperationResult<int>))]
        public async Task<IActionResult> CreateNewTask([FromForm] TaskDto task, [FromForm] int projectId, [FromForm] List<IFormFile>? images, [FromForm] List<int> employees, [FromForm] bool shouldStartNow)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var result = await _taskManagement.CreateTask(task, employeeId, projectId, images, employees, shouldStartNow);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            var timelineEvent = new TimelineDto
            {
                Event = "created the task",
                EmployeeId = employeeId,
                Type = TimelineType.Create,
                TaskId = result.Data
            };

            await _timelineManagement.CreateTimelineEvent(timelineEvent, UserRoles.Supervisor);

            return Ok(result);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("set/finished/bulk")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> SetTasksFinishedBulk([FromBody] int[] taskIds)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var result = await _taskManagement.SetTasksFinishedBulk(taskIds);

            if (result.Success)
            {
                foreach (var taskId in taskIds)
                {
                    var timelineEvent = new TimelineDto
                    {
                        Event = "has set the following task as finished:",
                        EmployeeId = employeeId,
                        Type = TimelineType.Finish,
                        TaskId = taskId
                    };

                    await _timelineManagement.CreateTimelineEvent(timelineEvent, UserRoles.Supervisor);
                }
            }

            return Ok(result);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("{taskId}/set/finished")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> SetTaskFinished(int taskId)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var result = await _taskManagement.SetTaskFinished(taskId);

            if (result.Success)
            {
                var timelineEvent = new TimelineDto
                {
                    Event = "has set the following task as finished:",
                    EmployeeId = employeeId,
                    Type = TimelineType.Finish,
                    TaskId = taskId
                };

                await _timelineManagement.CreateTimelineEvent(timelineEvent, UserRoles.Supervisor);
            }

            return Ok(result);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("set/start/bulk")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> SetTasksStartBulk([FromBody] int[] taskIds)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var result = await _taskManagement.SetTasksStartBulk(taskIds);

            if (result.Success)
            {
                foreach (var taskId in taskIds)
                {
                    var timelineEvent = new TimelineDto
                    {
                        Event = "has set the following task as started:",
                        EmployeeId = employeeId,
                        Type = TimelineType.Start,
                        TaskId = taskId
                    };

                    await _timelineManagement.CreateTimelineEvent(timelineEvent, UserRoles.Supervisor);
                }
            }

            return Ok(result);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("{taskId}/set/start")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> SetTaskStart(int taskId)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var result = await _taskManagement.SetTaskStart(taskId);

            if (result.Success)
            {
                var timelineEvent = new TimelineDto
                {
                    Event = "has set the following task as started:",
                    EmployeeId = employeeId,
                    Type = TimelineType.Start,
                    TaskId = taskId
                };

                await _timelineManagement.CreateTimelineEvent(timelineEvent, UserRoles.Supervisor);
            }

            return Ok(result);
        }
    }
}
