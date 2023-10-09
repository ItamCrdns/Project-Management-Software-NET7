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

        public async Task<(string status, IEnumerable<EmployeeDto>)> AddEmployeesToProject(int projectId, List<int> employees)
        {
            // * Adds the employees to a certain project and returns a list of the added employees

            // * employees = list of integers with employee ids
            // * "ProjectId", projectId = identifier of what entity we are updating
            // * IsEmployeeAlreadyInProject return whether or not the employee its already in the project
            return await _patcherService.AddEmployeesToEntity<EmployeeProject, Project>(employees, "ProjectId", projectId, IsEmployeeAlreadyInProject);
        }

        public async Task<(string status, IEnumerable<ImageDto>)> AddImagesToExistingProject(int projectId, List<IFormFile>? images)
        {
            var project = await GetProjectById(projectId);

            int imageCountInProjectEntity = project.Images.Count;

            return await _imageService.AddImagesToExistingEntity(projectId, images, "Project", imageCountInProjectEntity);
        }

        public async Task<(Project, List<Image>)> CreateProject(Project project, int employeeSupervisorId, List<IFormFile>? images, int companyId, List<int>? employees)
        {
            var newProject = new Project
            {
                Name = project.Name,
                Description = project.Description,
                Created = DateTimeOffset.UtcNow,
                ProjectCreatorId = employeeSupervisorId,
                Priority = project.Priority,
                CompanyId = companyId
            };

            // Save changed because we will need to access the projectId later when adding images
            _context.Add(newProject);
            _ = await _context.SaveChangesAsync();

            List<EmployeeProject> employeesToAdd = new();

            if(employees.Count > 0)
            {
                foreach (var employee in employees)
                {
                    var newRelation = new EmployeeProject
                    {
                        EmployeeId = employee,
                        ProjectId = newProject.ProjectId
                    };

                    employeesToAdd.Add(newRelation);
                }

                _context.AddRange(employeesToAdd);
                _ = await _context.SaveChangesAsync();
            }

            List<Image> imageCollection = new();

            if(images is not null && images.Any(i => i.Length > 0))
            {
                imageCollection = await _imageService.AddImagesToNewEntity(images, newProject.ProjectId, "Project", null);
            }

            return (newProject, imageCollection);
        }

        public async Task<bool> DoesProjectExist(int projectId) => await _context.Projects.AnyAsync(i => i.ProjectId == projectId);

        public async Task<IEnumerable<ProjectDto>> GetAllProjects(int page, int pageSize)
        {
            int postsToSkip = (page - 1) * pageSize;

            var projects = await _context.Projects
                .OrderByDescending(p => p.Created)
                .Include(t => t.Images)
                .Include(c => c.Company)
                .Include(e => e.Employees)
                .Include(p => p.ProjectCreator)
                .Skip(postsToSkip)
                .Take(pageSize)
                .ToListAsync();

            foreach(var project in projects)
            { 
                project.Images = SelectImages(project.Images);
            }

            var projectDtos = projects.Select(project => new ProjectDto
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                Description = project.Description,
                Created = project.Created,
                Finalized = project.Finalized,
                Priority = project.Priority,
                Company = new CompanyShowcaseDto
                {
                    CompanyId = project.Company.CompanyId,
                    Name = project.Company.Name,
                    Logo = project.Company.Logo
                },
                Employees = project.Employees.Select(p => new EmployeeShowcaseDto
                {
                    Username = p.Username,
                    ProfilePicture = p.ProfilePicture
                }).ToList(),
                ProjectCreator = new EmployeeShowcaseDto
                {
                    Username = project.ProjectCreator.Username,
                    ProfilePicture = project.ProjectCreator.ProfilePicture
                }
            }).ToList();

            return projectDtos;
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

        public async Task<Dictionary<string, List<ProjectDto>>> GetProjectsGroupedByCompany(int page, int pageSize)
        {
            int entitiesToSkip = (page - 1) * pageSize;

            // * This will load all of the projects to memory
            var groupedProjects = await _context.Projects
                .Include(c => c.Company)
                .Include(e => e.Employees)
                .Include(p => p.ProjectCreator)
                .GroupBy(p => p.Company.Name)
                .ToListAsync();
            
            // Create a dictionary to store the grouped projects by their Company Name
            var result = new Dictionary<string, List<ProjectDto>>();

            foreach (var group in groupedProjects)
            {
                var companyName = group.Key;
                var projects = group.ToList();

                var projectDtos = projects.Select(project => new ProjectDto
                {
                    ProjectId = project.ProjectId,
                    Name = project.Name,
                    Description = project.Description,
                    Created = project.Created,
                    Finalized = project.Finalized,
                    Priority = project.Priority,
                    Company = new CompanyShowcaseDto
                    {
                        CompanyId = project.Company.CompanyId,
                        Name = project.Company.Name,
                        Logo = project.Company.Logo
                    },
                    Employees = project.Employees.Select(p => new EmployeeShowcaseDto
                    {
                        Username = p.Username,
                        ProfilePicture = p.ProfilePicture
                    }).ToList(),
                    ProjectCreator = new EmployeeShowcaseDto
                    {
                        Username = project.ProjectCreator.Username,
                        ProfilePicture = project.ProjectCreator.ProfilePicture
                    }
                }).Skip(entitiesToSkip).Take(pageSize).ToList(); // Skip and take to get only a certain amount of projects by company

                result.Add(companyName, projectDtos);
            }

            return result;
        }

        public async Task<bool> IsEmployeeAlreadyInProject(int employeeId, int projectId)
        {
            return await _context.EmployeeProjects
                .AnyAsync(ep => ep.EmployeeId.Equals(employeeId) && ep.ProjectId.Equals(projectId));
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
