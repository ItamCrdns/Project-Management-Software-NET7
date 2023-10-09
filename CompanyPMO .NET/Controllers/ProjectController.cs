﻿using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProject _projectService;
        private readonly IUserIdentity _userIdentityService;
        private readonly Lazy<Task<int>> _lazyUserId;

        public ProjectController(IProject projectService, IUserIdentity userIdentityService)
        {
            _projectService = projectService;
            _userIdentityService = userIdentityService;
            _lazyUserId = new Lazy<Task<int>>(InitializeUserId);
        }

        private async Task<int> InitializeUserId()
        {
            return await _userIdentityService.GetUserIdFromClaims(HttpContext.User);
        }

        private async Task<int> GetUserId()
        {
            return await _lazyUserId.Value;
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
        public async Task<IActionResult> GetAllProjects(int page, int pageSize)
        {
            var projects = await _projectService.GetAllProjects(page, pageSize);

            return Ok(projects);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("all/groupedbycompany")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
        public async Task<IActionResult> GetProjectsGroupedByCompany(int page, int pageSize)
        {
            var projects = await _projectService.GetProjectsGroupedByCompany(page, pageSize);

            return Ok(projects);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("new")]
        [ProducesResponseType(200, Type = typeof(Project))]
        public async Task<IActionResult> NewProject([FromForm] Project project, [FromForm] List<IFormFile>? images, [FromForm] int companyId, [FromForm] List<int> employees)
        {
            var (newProject, imageCollection) = await _projectService.CreateProject(project, await GetUserId(), images, companyId, employees);

            var projectDto = new Project
            {
                ProjectId = newProject.ProjectId,
                Name = newProject.Name,
                Description = newProject.Description,
                Created = newProject.Created,
                Images = imageCollection
            };

            return Ok(projectDto);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPatch("{projectId}/update")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProject(int projectId, [FromForm] ProjectDto projectDto, [FromForm] List<IFormFile>? images)
        {
            bool projectExists = await _projectService.DoesProjectExist(projectId);

            if(!projectExists)
            {
                return NotFound();
            }

            var (updated, project) = await _projectService.UpdateProject(await GetUserId(), projectId, projectDto, images);

            if (!updated)
            {
                return BadRequest();
            }

            return Ok(project);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{projectId}")]
        [ProducesResponseType(200, Type = typeof(Project))]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            var project = await _projectService.GetProjectById(projectId);

            return Ok(project);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("{projectId}/set/ended")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SetProjectFinalized(int projectId)
        {
            bool updated = await _projectService.SetProjectFinalized(projectId);

            if(!updated)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpPost("{projectId}/add/employees")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddEmployeesToProject(int projectId, [FromForm] List<int> employees)
        {
            var (response, employeesAdded) = await _projectService.AddEmployeesToProject(projectId, employees);

            if(response is null)
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
