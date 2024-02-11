﻿using CompanyPMO_.NET.Dto;
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
        private readonly Lazy<Task<int>> _lazyUserId;

        public TaskController(ITask taskService, IUserIdentity userIdentityService)
        {
            _taskService = taskService;
            _userIdentityService = userIdentityService;
            _lazyUserId = new Lazy<Task<int>>(InitializeUserId); // Load the userId until we actually need it
        }

        private async Task<int> InitializeUserId()
        {
            return await _userIdentityService.GetUserIdFromClaims(HttpContext.User);
        }

        private async Task<int> GetUserId()
        {
            return await _lazyUserId.Value;
        }

        [HttpPost("new")]
        [ProducesResponseType(200, Type = typeof(Models.Task))]
        public async Task<IActionResult> NewTask([FromForm] TaskDto task, [FromForm] int projectId, [FromForm] List<IFormFile>? images)
        {
            var (newTask, imageCollection) = await _taskService.CreateTask(task, await GetUserId(), projectId, images);

            var taskDto = new Models.Task
            {
                TaskId = newTask.TaskId,
                Name = newTask.Name,
                Description = newTask.Description,
                Created = DateTime.UtcNow,
                TaskCreatorId = await GetUserId(),
                Images = imageCollection
            };

            return Ok(taskDto);
        }

        [HttpGet("{taskId}")]
        [ProducesResponseType(200, Type = typeof(Models.Task))]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            var task = await _taskService.GetTaskById(taskId);

            return Ok(task);
        }

        [HttpPost("{taskId}/start")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> StartTask(int taskId)
        {
            bool taskStarted = await _taskService.StartingWorkingOnTask(await GetUserId(), taskId);

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
            bool taskFinished = await _taskService.FinishedWorkingOnTask(await GetUserId(), taskId);

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
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeShowcaseDto>))]
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
        public async Task<IActionResult> GetTasksGroupedByProject([FromQuery] FilterParams filterParams, [FromQuery] int projectsPage, [FromQuery] int projectsPageSize)
        {
            var tasks = await _taskService.GetTasksGroupedByProject(filterParams, projectsPage, projectsPageSize);

            return Ok(tasks);
        }
    }
}
