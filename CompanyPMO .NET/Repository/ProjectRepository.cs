using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyPMO_.NET.Repository
{
    public class ProjectRepository : IProject
    {
        private readonly ApplicationDbContext _context;
        private readonly IImage _imageService;

        public ProjectRepository(ApplicationDbContext context, IImage imageService)
        {
            _context = context;
            _imageService = imageService;
        }
        public async Task<(Project, List<Image>)> CreateProject(Project project, int employeeSupervisorId, List<IFormFile>? images)
        {
            var newProject = new Project
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                Description = project.Description,
                Created = DateTimeOffset.UtcNow,
                ProjectCreatorId = employeeSupervisorId,
            };

            // Save changed because we will need to access the projectId later when adding images
            _context.Add(newProject);
            _ = await _context.SaveChangesAsync();

            List<Image> imageCollection = new();

            if(images is not null && images.Any(i => i.Length > 0))
            {
                imageCollection = await _imageService.AddImagesToEntity(images, newProject.ProjectId, "Project");
            }

            return (newProject, imageCollection);
        }

        public async Task<Project?> GetProjectById(int projectId)
        {
            var project = await _context.Projects
                .Where(p => p.ProjectId.Equals(projectId))
                .Include(t => t.Images)
                .FirstOrDefaultAsync();

            var projectImages = project.Images.Select(i => new Image
            {
                ImageId = i.ImageId,
                EntityType = i.EntityType,
                EntityId = i.EntityId,
                ImageUrl = i.ImageUrl,
                PublicId = i.PublicId
            }).ToList(); // Materialize the project

            project.Images = projectImages;

            return project;
        }
    }
}
