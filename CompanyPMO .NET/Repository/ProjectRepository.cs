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

        public async Task<(string status, IEnumerable<EmployeeDto>)> AddEmployeesToProject(int projectId, List<int> employeeIds)
        {
            // * Adds the employees to a certain project and returns a list of the added employees
            // * Checks if the employee is already in the project and

            bool projectExists = await _context.Projects.FindAsync(projectId) is not null;

            if(!projectExists)
            {
                return ("Project does not exist", null);
            }

            if(employeeIds.Count is 0)
            {
                return ("No employees were provided.", null);
            }

            List<EmployeeProject> EmployeesToAdd = new();

            foreach(var id in employeeIds)
            {
                // * Skip iteration if the employee is already in the project
                bool employeeAlreadyInProject = await _context.EmployeeProjects
                    .AnyAsync(ep => ep.EmployeeId.Equals(id) && ep.ProjectId.Equals(projectId));

                if(employeeAlreadyInProject)
                {
                    continue;
                }

                var relation = new EmployeeProject
                {
                    EmployeeId = id,
                    ProjectId = projectId
                };

                EmployeesToAdd.Add(relation);
            }

            if(EmployeesToAdd.Count > 0)
            {
                _context.EmployeeProjects.AddRange(EmployeesToAdd);
            }

            int rowsAffected = await _context.SaveChangesAsync();

            if(rowsAffected.Equals(employeeIds.Count))
            {
                string response = "All employees were added successfully";
                IEnumerable<EmployeeDto> employeesAdded = await _context.Employees
                    .Where(e => EmployeesToAdd.Select(e => e.EmployeeId).Contains(e.EmployeeId))
                    .Select(e => new EmployeeDto
                    {
                        EmployeeId = e.EmployeeId,
                        Username = e.Username,
                        ProfilePicture = e.ProfilePicture,
                        Role = e.Role,
                    }).ToListAsync();

                return (response, employeesAdded);
            }
            else if (rowsAffected > 0) {
                string response = "Operation was completed. However, not all employees could be added. Are you trying to add employees that are already working in this project?";
                IEnumerable<EmployeeDto> employeesAdded = await _context.Employees
                    // * Explicitly checks for the intersection of IDs between the two lists.
                    .Where(e => EmployeesToAdd.Select(e => e.EmployeeId).Contains(e.EmployeeId))
                    .Select(e => new EmployeeDto
                    {
                        EmployeeId = e.EmployeeId,
                        Username = e.Username,
                        ProfilePicture = e.ProfilePicture,
                        Role = e.Role,
                    }).ToListAsync();

                return (response, employeesAdded);
            }
            else
            {
                string response = "No employees were added. Are you trying to add employees that are already working in this project?";
                return (response, null);
            }
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
