using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces.Project_interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectManagementController : ControllerBase
    {
        private readonly IProjectManagement _projectManagement;
        public ProjectManagementController(IProjectManagement projectManagement)
        {
            _projectManagement = projectManagement;
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(OperationResult<int>))]
        [ProducesResponseType(400, Type = typeof(OperationResult<int>))]
        public async Task<IActionResult> CreateNewProject([FromForm] Project project, [FromForm] List<IFormFile>? images, [FromForm] int companyId, [FromForm] List<int> employees, [FromForm] bool shouldStartNow)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var result = await _projectManagement.CreateProject(project, new EmployeeDto { EmployeeId = employeeId, Username = username }, images, companyId, employees, shouldStartNow);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPatch("{projectId}/update")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProject(int projectId, [FromForm] ProjectDto projectDto, [FromForm] List<IFormFile>? images)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var (updated, project) = await _projectManagement.UpdateProject(employeeId, projectId, projectDto, images);

            if (!updated)
            {
                return BadRequest();
            }

            return Ok(project);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("set/finished/bulk")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> SetProjectsFininishedBulk([FromBody] int[] projectIds)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var result = await _projectManagement.SetProjectsFininishedBulk(projectIds, employeeId);

            return Ok(result);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("{projectId}/set/finished")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> SetProjectFinished(int projectId)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var result = await _projectManagement.SetProjectFinished(projectId, employeeId);

            return Ok(result);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("set/start/bulk")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> SetProjectsStartBulk([FromBody] int[] projectIds)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var result = await _projectManagement.SetProjectsStartBulk(projectIds, employeeId);

            return Ok(result);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("{projectId}/set/start")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> SetProjectStart(int projectId)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var result = await _projectManagement.SetProjectStart(projectId, employeeId);

            return Ok(result);
        }
    }
}
