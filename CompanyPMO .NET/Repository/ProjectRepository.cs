using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyPMO_.NET.Repository
{
    public class ProjectRepository : IProject
    {
        private readonly ApplicationDbContext _context;
        private readonly IImage _imageService;
        private readonly IPatcher _patcherService;

        public ProjectRepository(ApplicationDbContext context, IImage imageService, IPatcher patcherService)
        {
            _context = context;
            _imageService = imageService;
            _patcherService = patcherService;
        }

        public async Task<(string status, IEnumerable<ImageDto>)> AddImagesToExistingProject(int projectId, List<IFormFile>? images)
        {
            var project = await GetProjectById(projectId);

            int imageCountInProjectEntity = project.Images.Count;

            return await _imageService.AddImagesToExistingEntity(projectId, images, "Project", imageCountInProjectEntity);
        }

        public async Task<(Project, List<Image>)> CreateProject(Project project, int employeeSupervisorId, List<IFormFile>? images)
        {
            var newProject = new Project
            {
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
                imageCollection = await _imageService.AddImagesToNewEntity(images, newProject.ProjectId, "Project", null);
            }

            return (newProject, imageCollection);
        }

        public async Task<bool> DoesProjectExist(int projectId) => await _context.Projects.AnyAsync(i => i.ProjectId == projectId);

        public async Task<List<Project>> GetAllProjects(int page, int pageSize)
        {
            int postsToSkip = (page - 1) * pageSize;

            var projects = await _context.Projects
                .OrderByDescending(p => p.Created)
                .Include(t => t.Images)
                .Skip(postsToSkip)
                .Take(pageSize)
                .ToListAsync();

            foreach(var project in projects)
            {
                project.Images = SelectImages(project.Images);
            }

            return projects;
        }

        public async Task<Project> GetProjectById(int projectId)
        {
            var project = await _context.Projects
                .Where(p => p.ProjectId.Equals(projectId))
                .Include(t => t.Images)
                .FirstOrDefaultAsync();

            project.Images = SelectImages(project.Images);

            return project;
        }

        public ICollection<Image> SelectImages(ICollection<Image> images)
        {
            var projectImages = images
                .Where(et => et.EntityType.Equals("Project"))
                .Select(i => new Image
                {
                    ImageId = i.ImageId,
                    EntityType = i.EntityType,
                    EntityId = i.EntityId,
                    ImageUrl = i.ImageUrl,
                    PublicId = i.PublicId,
                    Created = i.Created
                }).ToList();

            return projectImages;
        }


        public async Task<bool> SetProjectFinalized(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);

            if(project is not null && project.Finalized is null)
            {
                project.Finalized = DateTimeOffset.UtcNow;
                _context.Update(project);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<(bool updated, ProjectDto)> UpdateProject(int employeeId, int projectId, ProjectDto projectDto, List<IFormFile>? images)
        {
            return await _patcherService.UpdateEntity(employeeId, projectId, projectDto, images, AddImagesToExistingProject, GetProjectById);
        }
    }
}
