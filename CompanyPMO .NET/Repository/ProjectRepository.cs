using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

        public async Task<(string status, IEnumerable<EmployeeShowcaseDto>)> AddEmployeesToProject(int projectId, List<int> employees)
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

        public async Task<OperationResult<int>> CreateProject(Project project, int employeeSupervisorId, List<IFormFile>? images, int companyId, List<int>? employees, bool shouldStartNow)
        {
            if (string.IsNullOrWhiteSpace(project.Name) || string.IsNullOrWhiteSpace(project.Description))
            {
                return new OperationResult<int>
                {
                    Success = false,
                    Message = "Project name and description are required",
                    Data = 0
                };
            }

            var company = await _context.Companies.FindAsync(companyId);

            Project newProject = new()
            {
                Name = project.Name,
                Description = project.Description,
                Created = DateTime.UtcNow,
                ProjectCreatorId = employeeSupervisorId,
                Priority = project.Priority,
                CompanyId = companyId,
                ExpectedDeliveryDate = project.ExpectedDeliveryDate,
                StartedWorking = shouldStartNow ? DateTime.UtcNow : null,
            };

            // Save changed because we will need to access the projectId later when adding images
            _context.Add(newProject);

            company.LatestProjectCreation = DateTime.UtcNow;
            _context.Update(company);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected is 0)
            {
                return new OperationResult<int>
                {
                    Success = false,
                    Message = "Failed to create the project",
                    Data = 0
                };
            }

            List<string> errors = new();

            if (employees is not null && employees.Count > 0)
            {
                var employeesToAdd = employees.Where(employee => employee != employeeSupervisorId).Select(employee => new EmployeeProject
                {
                    EmployeeId = employee,
                    ProjectId = newProject.ProjectId
                });

                if (employees.Any(x => x == employeeSupervisorId))
                {
                    errors.Add("You can't add yourself");
                }

                await _context.EmployeeProjects.AddRangeAsync(employeesToAdd);
                int employeeRowsAffected = await _context.SaveChangesAsync(); // Returns the number of added employees

                // Based on this we can execute the stored procedure to update each employees workload
                foreach (var employeeId in employees)
                {
                    var parameter = new NpgsqlParameter("@EmployeeId", employeeId);
                    
                    await _context.Database.ExecuteSqlRawAsync("CALL sp_update_employee_workload(@EmployeeId)", parameter);
                }

                if (employeeRowsAffected is 0)
                {
                    errors.Add("Failed to add employees to the project");
                }
            }

            if (images is not null && images.Any(i => i.Length > 0))
            {
                var newImages = await _imageService.AddImagesToNewEntity(images, newProject.ProjectId, "Project", null);

                if (newImages.Count is 0)
                {
                    errors.Add("Failed to add images to the project");
                }
            }

            return new OperationResult<int>
            {
                Success = true,
                Message = "Project created successfully",
                Data = newProject.ProjectId,
                Errors = errors
            };
        }

        public async Task<bool> DoesProjectExist(int projectId) => await _context.Projects.AnyAsync(i => i.ProjectId == projectId);

        public async Task<DataCountPages<ProjectDto>> GetAllProjects(FilterParams filterParams)
        {
            List<string> navProperties = new() { "Company", "Employees", "ProjectCreator" };

            var (projects, totalProjectsCount, totalPages) = await _utilityService.GetAllEntities<Project>(filterParams, navProperties);

            var projectDtos = ProjectSelectQuery(projects);

            return new DataCountPages<ProjectDto>
            {
                Data = projectDtos,
                Count = totalProjectsCount,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<ProjectDto>> GetProjectsByCompanyName(int companyId, FilterParams filterParams)
        {
            var (whereExpression, orderByExpression) = _utilityService.BuildWhereAndOrderByExpressions<Project>(companyId, null, null, "CompanyId", "Created", filterParams);

            bool ShallOrderAscending = filterParams.Sort is not null && filterParams.Sort.Equals("ascending");
            bool ShallOrderDescending = filterParams.Sort is not null && filterParams.Sort.Equals("descending");

            List<Project> projects = new();

            int totalProjectsCount = await _context.Projects
                .Where(whereExpression)
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalProjectsCount / filterParams.PageSize);

            if (filterParams.Page > totalPages)
            {
                filterParams.Page = totalPages; // If trying to reach a page that does not exist fallback to the last page
            }

            if (filterParams.PageSize > totalProjectsCount)
            {
                filterParams.PageSize = totalProjectsCount; // If the page size is bigger than the total count fallback to the total count
            }

            int toSkip = (filterParams.Page - 1) * filterParams.PageSize;

            if (ShallOrderAscending)
            {
                projects = await _context.Projects
                    .Where(whereExpression)
                    .OrderBy(orderByExpression)
                    .Include(c => c.Company)
                    .Include(e => e.Employees)
                    .Include(p => p.ProjectCreator)
                    .Skip(toSkip)
                    .Take(filterParams.PageSize)
                    .ToListAsync();
            }
            else if (ShallOrderDescending || (!ShallOrderAscending && !ShallOrderDescending))
            {
                projects = await _context.Projects
                    .Where(whereExpression)
                    .OrderByDescending(orderByExpression)
                    .Include(c => c.Company)
                    .Include(e => e.Employees)
                    .Include(p => p.ProjectCreator)
                    .Skip(toSkip)
                    .Take(filterParams.PageSize)
                    .ToListAsync();
            }

            var projectDtos = ProjectSelectQuery(projects);

            return new DataCountPages<ProjectDto>
            {
                Data = projectDtos,
                Count = totalProjectsCount,
                Pages = totalPages
            };
        }

        public async Task<EntityParticipantOrOwnerDTO<ProjectDto>?> GetProjectById(int projectId, int userId)
        {
            return await _context.Projects
                .Where(p => p.ProjectId.Equals(projectId))
                .Select(x => new EntityParticipantOrOwnerDTO<ProjectDto>
                {
                    Entity = new ProjectDto
                    {
                        ProjectId = x.ProjectId,
                        Name = x.Name,
                        Description = x.Description,
                        Created = x.Created,
                        Finished = x.Finished,
                        StartedWorking = x.StartedWorking,
                        ExpectedDeliveryDate = x.ExpectedDeliveryDate,
                        Priority = x.Priority,
                        Creator = new EmployeeShowcaseDto
                        {
                            EmployeeId = x.ProjectCreator.EmployeeId,
                            Username = x.ProjectCreator.Username,
                            ProfilePicture = x.ProjectCreator.ProfilePicture
                        },
                        Company = new CompanyShowcaseDto
                        {
                            CompanyId = x.Company.CompanyId,
                            Name = x.Company.Name,
                            Logo = x.Company.Logo
                        },
                        EmployeeCount = x.Employees.Count,
                        TasksCount = x.Tasks.Count,
                        Team = x.Employees.Select(p => new EmployeeShowcaseDto
                        {
                            EmployeeId = p.EmployeeId,
                            Username = p.Username,
                            ProfilePicture = p.ProfilePicture
                        }).Take(5).ToList(),
                    },
                    IsOwner = x.ProjectCreatorId == userId,
                    IsParticipant = x.Employees.Any(e => e.EmployeeId == userId)
                    })
                .FirstOrDefaultAsync();
        }

        public async Task<Project> GetProjectEntityById(int projectId)
        {
            return await _context.Projects
                .Where(p => p.ProjectId.Equals(projectId))
                .Include(t => t.Images)
                .FirstOrDefaultAsync();
        }

        public async Task<DataCountPages<CompanyProjectGroup>> GetProjectsGroupedByCompany(FilterParams filterParams, int projectsPage, int projectsPageSize, int employeeId)
        {
            List<CompanyProjectGroup> projects = await _context.Projects
                .GroupBy(x => x.Company)
                .Select(x => new CompanyProjectGroup
                {
                    CompanyName = x.Key.Name,
                    CompanyId = x.Key.CompanyId,
                    IsCurrentUserInTeam = x.SelectMany(e => e.Employees).Any(e => e.EmployeeId == employeeId), // This will return true if the current user is in the team of any of the projects
                    Projects = x.Select(project => new ProjectDto
                    {
                        ProjectId = project.ProjectId,
                        Name = project.Name,
                        Description = project.Description,
                        Created = project.Created,
                        Finished = project.Finished,
                        Priority = project.Priority,
                        Company = new CompanyShowcaseDto
                        {
                            CompanyId = project.Company.CompanyId,
                            Name = project.Company.Name,
                            Logo = project.Company.Logo
                        },
                        Team = project.Employees.Select(p => new EmployeeShowcaseDto
                        {
                            Username = p.Username,
                            ProfilePicture = p.ProfilePicture
                        }).ToList(),
                        Creator = new EmployeeShowcaseDto
                        {
                            Username = project.ProjectCreator.Username,
                            ProfilePicture = project.ProjectCreator.ProfilePicture
                        }
                    }).OrderBy(x => x.Created).Skip((projectsPage - 1) * projectsPageSize).Take(projectsPageSize),
                    Count = x.Count(),
                    Pages = (int)Math.Ceiling((double)x.Count() / projectsPageSize),
                    LatestProjectCreation = x.Key.LatestProjectCreation
                })
                .Skip((filterParams.Page - 1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .OrderByDescending(t => t.LatestProjectCreation) // This doesnt even exist yet. But it will be added later
                .ToListAsync();

            int totalCompaniesWithProjects = await _context.Companies.CountAsync(
                c => _context.Projects.Any(p => p.CompanyId == c.CompanyId)
                );

            int totalPages = (int)Math.Ceiling((double)totalCompaniesWithProjects / filterParams.PageSize);

            return new DataCountPages<CompanyProjectGroup>
            {
                Data = projects,
                Count = totalCompaniesWithProjects,
                Pages = totalPages
            };
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
                Finished = project.Finished,
                Priority = project.Priority,
                Company = new CompanyShowcaseDto
                {
                    CompanyId = project.Company.CompanyId,
                    Name = project.Company.Name,
                    Logo = project.Company.Logo
                },
                Team = project.Employees.Select(p => new EmployeeShowcaseDto
                {
                    EmployeeId = p.EmployeeId,
                    Username = p.Username,
                    ProfilePicture = p.ProfilePicture
                }).ToList(),
                Creator = new EmployeeShowcaseDto
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

            bool isProjectNullOrNotFinalized = project?.Finished is null || project?.ExpectedDeliveryDate > DateTime.UtcNow;

            if(project is not null && isProjectNullOrNotFinalized)
            {
                project.Finished = DateTime.UtcNow;
                _context.Update(project);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<(bool updated, ProjectDto)> UpdateProject(int employeeId, int projectId, ProjectDto projectDto, List<IFormFile>? images)
        {
            // TODO: Test it
            return await _utilityService.UpdateEntity(employeeId, projectId, projectDto, images, AddImagesToExistingProject, GetProjectEntityById);
        }

        public async Task<DataCountPages<ProjectDto>> GetProjectsByEmployeeUsername(string username, FilterParams filterParams)
        {
            var (projectIds, totalProjectsCount, totalPages) = await _utilityService.GetEntitiesEmployeeCreatedOrParticipates<EmployeeProject, Project>(username, "ProjectCreatorId", "ProjectId", filterParams.Page, filterParams.PageSize);

            var (whereExpression, orderByExpression) = _utilityService.BuildWhereAndOrderByExpressions<Project>(null, projectIds, "ProjectId", "ProjectId", "Created", filterParams);

            bool ShallOrderAscending = filterParams.Sort is not null && filterParams.Sort.Equals("ascending");
            bool ShallOrderDescending = filterParams.Sort is not null && filterParams.Sort.Equals("descending");

            List<Project> projects = new();

            if (ShallOrderAscending)
            {
                projects = await _context.Projects
                    .Where(whereExpression)
                    .OrderBy(orderByExpression)
                    .Include(c => c.Company)
                    .Include(e => e.Employees)
                    .Include(p => p.ProjectCreator)
                    .ToListAsync();
            } else if (ShallOrderDescending || (!ShallOrderAscending && !ShallOrderDescending))
            {
                projects = await _context.Projects
                    .Where(whereExpression)
                    .OrderByDescending(orderByExpression)
                    .Include(c => c.Company)
                    .Include(e => e.Employees)
                    .Include(p => p.ProjectCreator)
                    .ToListAsync();
            }
            var projectDtos = ProjectSelectQuery(projects);

            return new DataCountPages<ProjectDto>
            {
                Data = projectDtos,
                Count = totalProjectsCount,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<ProjectShowcaseDto>> GetProjectsShowcaseByEmployeeUsername(string username, int page, int pageSize)
        {
            var (projectIds, totalProjectsCount, totalPages) = await _utilityService.GetEntitiesEmployeeCreatedOrParticipates<EmployeeProject, Project>(username, "ProjectCreatorId", "ProjectId", page, pageSize);

            ICollection<ProjectShowcaseDto> projects = await _context.Projects
                .Where(p => projectIds.Contains(p.ProjectId))
                .Select(p => new ProjectShowcaseDto
                {
                    ProjectId = p.ProjectId,
                    Name = p.Name,
                    Priority = p.Priority
                })
                .ToListAsync();

            return new DataCountPages<ProjectShowcaseDto>
            {
                Data = projects,
                Count = totalProjectsCount,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<ProjectShowcaseDto>> GetAllProjectsShowcase(int page, int pageSize)
        {
            // Admin only endpoint. Get all projects without any additional information (showcase only)

            int toSkip = (page - 1) * pageSize;
            IEnumerable<ProjectShowcaseDto> projects = await _context.Projects
                .OrderByDescending(p => p.Created)
                .Select(project => new ProjectShowcaseDto
                {
                    ProjectId = project.ProjectId,
                    Name = project.Name,
                    Priority = project.Priority
                })
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            int totalProjectsCount = await _context.Projects.CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalProjectsCount / pageSize);

            return new DataCountPages<ProjectShowcaseDto>
            {
                Data = projects,
                Count = totalProjectsCount,
                Pages = totalPages
            };
        }

        public async Task<bool> IsParticipant(int projectId, int employeeId) => await _context.EmployeeProjects
                .AnyAsync(ep => ep.ProjectId.Equals(projectId) && ep.EmployeeId.Equals(employeeId));

        public async Task<bool> IsOwner(int projectId, int employeeId) => await _context.Projects
                .AnyAsync(p => p.ProjectId.Equals(projectId) && p.ProjectCreatorId.Equals(employeeId));

        public async Task<ProjectSomeInfoDto?> GetProjectNameCreatorLifecyclePriorityAndTeam(int projectId)
        {
            return await _context.Projects
                .Where(p => p.ProjectId.Equals(projectId))
                .Select(p => new ProjectSomeInfoDto
                {
                    ProjectId = p.ProjectId,
                    Name = p.Name,
                    Created = p.Created,
                    StartedWorking = p.StartedWorking,
                    Finished = p.Finished,
                    ExpectedDeliveryDate = p.ExpectedDeliveryDate,
                    Lifecycle = p.Lifecycle,
                    Priority = p.Priority,
                    Creator = new EmployeeShowcaseDto
                    {
                        EmployeeId = p.ProjectCreator.EmployeeId,
                        Username = p.ProjectCreator.Username,
                        ProfilePicture = p.ProjectCreator.ProfilePicture
                    },
                    Team = p.Employees.Select(p => new EmployeeShowcaseDto
                    {
                        EmployeeId = p.EmployeeId,
                        Username = p.Username,
                        ProfilePicture = p.ProfilePicture
                    }).OrderByDescending(x => x.Username).Take(5).ToList(),
                    EmployeeCount = p.Employees.Count
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ProjectShowcaseDto> GetProjectShowcase(int projectId)
        {
            return await _context.Projects.Where(x => x.ProjectId == projectId)
                .Select(p => new ProjectShowcaseDto
                {
                    ProjectId = p.ProjectId,
                    Name = p.Name,
                    Priority = p.Priority
                })
                .FirstOrDefaultAsync();
        }
    }
}
