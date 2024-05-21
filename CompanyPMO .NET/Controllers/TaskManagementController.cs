using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces.Task_interfaces;
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
        public TaskManagementController(ITaskManagement taskManagement)
        {
            _taskManagement = taskManagement;
        }

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(OperationResult<int>))]
        [ProducesResponseType(400, Type = typeof(OperationResult<int>))]
        public async Task<IActionResult> NewTask([FromForm] TaskDto task, [FromForm] int projectId, [FromForm] List<IFormFile>? images, [FromForm] List<int> employees, [FromForm] bool shouldStartNow)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var result = await _taskManagement.CreateTask(task, employeeId, projectId, images, employees, shouldStartNow);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("{taskId}/start")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> StartTask(int taskId)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            bool taskStarted = await _taskManagement.StartingWorkingOnTask(employeeId, taskId);

            if (!taskStarted)
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
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            bool taskFinished = await _taskManagement.FinishedWorkingOnTask(employeeId, taskId);

            if (!taskFinished)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("{taskId}/employees/add")]
        [ProducesResponseType(200, Type = typeof(Dictionary<string, object>))]
        public async Task<IActionResult> AddEmployeesToTask(int taskId, [FromForm] List<int> employees)
        {
            var (response, employeesAdded) = await _taskManagement.AddEmployeesToTask(taskId, employees);

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
    }
}
