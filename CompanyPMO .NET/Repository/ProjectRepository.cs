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
        private readonly IUtility _utilityService;

        public ProjectRepository(ApplicationDbContext context, IImage imageService, IUtility utilityService)
        {
            _context = context;
            _imageService = imageService;
            _utilityService = utilityService;
        }

        public async Task<(string status, IEnumerable<EmployeeDto>)> AddEmployeesToProject(int projectId, List<int> employees)
        {
            // * Adds the employees to a certain project and returns a list of the added employees

            // * employees = list of integers with employee ids
            // * "ProjectId", projectId = identifier of what entity we are updating
            // * IsEmployeeAlreadyInProject return whether or not the employee its already in the project
            return await _utilityService.AddEmployeesToEntity<EmployeeProject, Project>(employees, "ProjectId", projectId, IsEmployeeAlreadyInProject);
        }

        public async Task<(string status, IEnumerable<ImageDto>)> AddImagesToExistingProject(int projectId, List<IFormFile>? images)
        {
            var project = await GetProjectEntityById(projectId);

            int imageCountInProjectEntity = project.Images.Count;

            return await _imageService.AddImagesToExistingEntity(projectId, images, "Project", imageCountInProjectEntity);
        }

        public async Task<int> CreateProject(Project project, int employeeSupervisorId, List<IFormFile>? images, int companyId, List<int>? employees)
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

            if (employees.Count > 0)
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

            return newProject.ProjectId;
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

            var projectDtos = ProjectSelectQuery(projects);

            return projectDtos;
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsByCompanyName(int companyId, int page, int pageSize)
        {
            int toSkip = (page - 1) * pageSize;

            //int companyId = await _context.Companies
            //    //.Where(n => EF.Functions.Like(n.Name, $"%{companyName}%"))
            //    .Where(n => n.Name.Contains(companyName)) // perform a simple substring search
            //    .Select(i => i.CompanyId)
            //    .FirstOrDefaultAsync();

            var projects = await _context.Projects
                .Where(i => i.CompanyId.Equals(companyId))
                .OrderByDescending(c => c.Created)
                .Include(c => c.Company)
                .Include(e => e.Employees)
                .Include(p => p.ProjectCreator)
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            var projectDtos = ProjectSelectQuery(projects);

            return projectDtos;
        }

        public async Task<ProjectDto> GetProjectById(int projectId)
        {
            var project = await _context.Projects
                .Where(p => p.ProjectId.Equals(projectId))
                .Include(t => t.Images)
                .Include(p => p.ProjectCreator)
                .Include(c => c.Company)
                .Include(e => e.Employees)
                .FirstOrDefaultAsync();

            var images = project.Images = SelectImages(project.Images);

            int totalEmployeesCount = await _context.Projects
                .Where(p => p.ProjectId.Equals(projectId))
                .SelectMany(e => e.Employees)
                .CountAsync();

            int tasksCount = await _context.Tasks
                .Where(t => t.ProjectId.Equals(projectId))
                .CountAsync();

            ProjectDto projectDto = new()
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                Description = project.Description,
                ImagesCollection = images.Select(i => new ImageDto
                {
                    ImageId = i.ImageId,
                    ImageUrl = i.ImageUrl,
                    PublicId = i.PublicId,
                    Created = i.Created
                }).ToList(),
                Created = project.Created,
                Finalized = project.Finalized,
                Priority = project.Priority,
                ProjectCreator = new EmployeeShowcaseDto
                {
                    EmployeeId = project.ProjectCreator.EmployeeId,
                    Username = project.ProjectCreator.Username,
                    ProfilePicture = project.ProjectCreator.ProfilePicture
                },
                Company = new CompanyShowcaseDto
                {
                    CompanyId = project.Company.CompanyId,
                    Name = project.Company.Name,
                    Logo = project.Company.Logo
                },
                EmployeeCount = totalEmployeesCount,
                TasksCount = tasksCount,
                Employees = project.Employees.Select(p => new EmployeeShowcaseDto
                {
                    EmployeeId = p.EmployeeId,
                    Username = p.Username,
                    ProfilePicture = p.ProfilePicture
                }).Take(5).ToList(),
            };

            return projectDto;
        }

        public async Task<Project> GetProjectEntityById(int projectId)
        {
            return await _context.Projects
                .Where(p => p.ProjectId.Equals(projectId))
                .Include(t => t.Images)
                .FirstOrDefaultAsync();
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

        public ICollection<ProjectDto> ProjectSelectQuery(ICollection<Project> projects)
        {
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
                    EmployeeId = p.EmployeeId,
                    Username = p.Username,
                    ProfilePicture = p.ProfilePicture
                }).ToList(),
                ProjectCreator = new EmployeeShowcaseDto
                {
                    EmployeeId = project.ProjectCreator.EmployeeId,
                    Username = project.ProjectCreator.Username,
                    ProfilePicture = project.ProjectCreator.ProfilePicture
                }
            }).ToList();

            return projectDtos;
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
            return await _utilityService.UpdateEntity(employeeId, projectId, projectDto, images, AddImagesToExistingProject, GetProjectById);
        }

        public async Task<Dictionary<string, object>> GetProjectsByEmployeeUsername(string username, int page, int pageSize)
        {
            // Returns a list of actual projects. containing a lot of information
            var (projectIds, totalProjectsCount, totalPages) = await _utilityService.GetEntitiesByEmployeeUsername<EmployeeProject>(username, "ProjectId", page, pageSize);

            ICollection<Project> projects = await _context.Projects
                .Where(p => projectIds.Contains(p.ProjectId))
                .Include(c => c.Company)
                .Include(e => e.Employees)
                .Include(p => p.ProjectCreator)
                .ToListAsync();

            var projectDtos = ProjectSelectQuery(projects);

            var result = new Dictionary<string, object>
            {
                { "data", projectDtos },
                { "count", totalProjectsCount },
                { "pages", totalPages }
            };

            return result;
        }

        public async Task<Dictionary<string, object>> GetProjectsShowcaseByEmployeeUsername(string username, int page, int pageSize)
        {
            // Returns a simple list just to showcase the project
            var (projectIds, totalProjectsCount, totalPages) = await _utilityService.GetEntitiesByEmployeeUsername<EmployeeProject>(username, "ProjectId", page, pageSize);

            var projects = await _context.Projects
                .Where(p => projectIds.Contains(p.ProjectId))
                .Select(p => new ProjectShowcaseDto
                {
                    ProjectId = p.ProjectId,
                    Name = p.Name,
                    Priority = p.Priority
                })
                .ToListAsync();

            var result = new Dictionary<string, object>
            {
                { "data", projects },
                { "count", totalProjectsCount },
                { "pages", totalPages }
            };

            return result;
        }
    }
}
