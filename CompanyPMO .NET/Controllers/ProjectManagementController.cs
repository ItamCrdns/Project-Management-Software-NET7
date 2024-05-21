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
        public async Task<IActionResult> NewProject([FromForm] Project project, [FromForm] List<IFormFile>? images, [FromForm] int companyId, [FromForm] List<int> employees, [FromForm] bool shouldStartNow)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var result = await _projectManagement.CreateProject(project, employeeId, images, companyId, employees, shouldStartNow);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPatch("{projectId}/update")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProject(int projectId, [FromForm] ProjectDto projectDto, [FromForm] List<IFormFile>? images)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var (updated, project) = await _projectManagement.UpdateProject(employeeId, projectId, projectDto, images);

            if (!updated)
            {
                return BadRequest();
            }

            return Ok(project);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("{projectId}/add/employees")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeShowcaseDto>))]
        public async Task<IActionResult> AddEmployeesToProject(int projectId, [FromForm] List<int> employees)
        {
            var (response, employeesAdded) = await _projectManagement.AddEmployeesToProject(projectId, employees);

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

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("set/finished/bulk")]
        [ProducesResponseType(200, Type = typeof(OperationResult<int[]>))]
        public async Task<IActionResult> SetProjectsFininishedBulk([FromBody] int[] projectIds)
        {
            var result = await _projectManagement.SetProjectsFininishedBulk(projectIds);

            return Ok(result);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("{projectId}/set/finished")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SetProjectFinalized(int projectId)
        {
            bool updated = await _projectManagement.SetProjectFinalized(projectId);

            if (!updated)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
