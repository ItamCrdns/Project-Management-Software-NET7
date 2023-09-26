using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProject _projectService;

        public ProjectController(IProject projectService)
        {
            _projectService = projectService;
        }

        [Authorize(Roles = "supervisor")]
        [HttpPost("new")]
        [ProducesResponseType(200, Type = typeof(Project))]
        public async Task<IActionResult> NewProject([FromForm] Project project, [FromForm] List<IFormFile>? images)
        {
            // * Access control
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _ = int.TryParse(userIdClaim, out int supervisorId);

            var (newProject, imageCollection) = await _projectService.CreateProject(project, supervisorId, images);

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

        [Authorize(Roles = "employee, supervisor")]
        [HttpGet("{projectId}")]
        [ProducesResponseType(200, Type = typeof(Project))]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            var project = await _projectService.GetProjectById(projectId);

            return Ok(project);
        }
    }
}
