using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces.Project_interfaces;
using CompanyPMO_.NET.Interfaces.Timeline_interfaces;
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
        private readonly ITimelineManagement _timelineManagement;
        public ProjectManagementController(IProjectManagement projectManagement, ITimelineManagement timelineManagement)
        {
            _projectManagement = projectManagement;
            _timelineManagement = timelineManagement;
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(OperationResult<int>))]
        [ProducesResponseType(400, Type = typeof(OperationResult<int>))]
        public async Task<IActionResult> CreateNewProject([FromForm] Project project, [FromForm] List<IFormFile>? images, [FromForm] int companyId, [FromForm] List<int> employees, [FromForm] bool shouldStartNow)
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

            var timelineEvent = new TimelineDto
            {
                Event = "created the project",
                EmployeeId = employeeId,
                Type = TimelineType.Create,
                ProjectId = result.Data
            };

            await _timelineManagement.CreateTimelineEvent(timelineEvent);

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

            var timelineEvent = new TimelineDto
            {
                Event = "updated the project #",
                EmployeeId = employeeId,
                Type = TimelineType.Update,
                ProjectId = projectId
            };

            return Ok(project);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("set/finished/bulk")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> SetProjectsFininishedBulk([FromBody] int[] projectIds)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var result = await _projectManagement.SetProjectsFininishedBulk(projectIds);

            if (result.Success)
            {
                foreach (var projectId in projectIds)
                {
                    var timelineEvent = new TimelineDto
                    {
                        Event = "has set the following project as finished:",
                        EmployeeId = employeeId,
                        Type = TimelineType.Finish,
                        ProjectId = projectId
                    };

                    await _timelineManagement.CreateTimelineEvent(timelineEvent);
                }
            }

            return Ok(result);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("{projectId}/set/finished")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> SetProjectFinished(int projectId)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var result = await _projectManagement.SetProjectFinished(projectId);

            var timelineEvent = new TimelineDto
            {
                Event = "has set the following project as finished:",
                EmployeeId = employeeId,
                Type = TimelineType.Finish,
                ProjectId = projectId
            };

            await _timelineManagement.CreateTimelineEvent(timelineEvent);

            return Ok(result);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("set/start/bulk")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> SetProjectsStartBulk([FromBody] int[] projectIds)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var result = await _projectManagement.SetProjectsStartBulk(projectIds);

            if (result.Success)
            {
                foreach (var projectId in projectIds)
                {
                    var timelineEvent = new TimelineDto
                    {
                        Event = "has set the following project as started:",
                        EmployeeId = employeeId,
                        Type = TimelineType.Start,
                        ProjectId = projectId
                    };

                    await _timelineManagement.CreateTimelineEvent(timelineEvent);
                }
            }

            return Ok(result);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("{projectId}/set/start")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> SetProjectStart(int projectId)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User ID claim is missing");
            }

            int employeeId = int.Parse(claim.Value);

            var result = await _projectManagement.SetProjectStart(projectId);

            if (result.Success)
            {
                var timelineEvent = new TimelineDto
                {
                    Event = "has set the following project as started:",
                    EmployeeId = employeeId,
                    Type = TimelineType.Start,
                    ProjectId = projectId
                };

                await _timelineManagement.CreateTimelineEvent(timelineEvent);
            }

            return Ok(result);
        }
    }
}
