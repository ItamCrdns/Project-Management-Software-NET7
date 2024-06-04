using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Hubs;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Interfaces.Notification_interfaces;
using CompanyPMO_.NET.Interfaces.Project_interfaces;
using CompanyPMO_.NET.Interfaces.Timeline_interfaces;
using CompanyPMO_.NET.Interfaces.Workload_interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CompanyPMO_.NET.Repository
{
    public class ProjectRepository : IProjectCompanyQueries, IProjectEmployeeQueries, IProjectManagement, IProjectQueries
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtility _utilityService;
        private readonly IWorkloadProject _workloadService;
        private readonly ITimelineManagement _timelineManagement;
        private readonly INotificationManagement _notificationManagement;
        private readonly IProjectPicture _projectPicture;

        public ProjectRepository(ApplicationDbContext context, IUtility utilityService, IWorkloadProject workloadService, ITimelineManagement timelineManagement, INotificationManagement notificationManagement, IProjectPicture projectPicture)
        {
            _context = context;
            _utilityService = utilityService;
            _workloadService = workloadService;
            _timelineManagement = timelineManagement;
            _notificationManagement = notificationManagement;
            _projectPicture = projectPicture;
        }

        public async Task<OperationResult<int>> CreateProject(Project project, EmployeeDto supervisor, List<IFormFile>? images, int companyId, List<int>? employeeIds, bool shouldStartNow)
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
                ProjectCreatorId = supervisor.EmployeeId,
                Priority = project.Priority,
                CompanyId = companyId,
                ExpectedDeliveryDate = project.ExpectedDeliveryDate,
                StartedWorking = shouldStartNow ? DateTime.UtcNow : null,
            };

            // Save changed because we will need to access the projectId later when adding images
            _context.Add(newProject);

            company.LatestProjectCreation = DateTime.UtcNow;

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

            var workloadCreatedProjectsResult = await _workloadService.UpdateEmployeeCreatedProjects(supervisor.EmployeeId);

            if (!workloadCreatedProjectsResult.Success)
            {
                errors.Add($"Failed to update the workload of the project creator. Error = {workloadCreatedProjectsResult.Message}");
            }

            IEnumerable<EmployeeProject> employeesToAdd = new List<EmployeeProject>();

            if (employeeIds is not null && employeeIds.Count > 0)
            {
                employeesToAdd = employeeIds.Where(employee => employee != supervisor.EmployeeId).Select(employee => new EmployeeProject
                {
                    EmployeeId = employee,
                    ProjectId = newProject.ProjectId
                });

                if (employeeIds.Any(x => x == supervisor.EmployeeId))
                {
                    errors.Add("You can't add yourself");
                }

                await _context.EmployeeProjects.AddRangeAsync(employeesToAdd);
                int employeeRowsAffected = await _context.SaveChangesAsync(); // Returns the number of added employees

                // Based on this we can execute the method to update assigned_projects on the workload table for the employee
                var workloadUpdateResult = await _workloadService.UpdateEmployeeAssignedProjects(employeesToAdd.Select(x => x.EmployeeId).ToArray());

                if (!workloadUpdateResult.Success)
                {
                    errors.Add($"Failed to update the workload of the employees. Error = {workloadUpdateResult.Message}");
                }

                if (employeeRowsAffected is 0)
                {
                    errors.Add("Failed to add employees to the project");
                }
            }

            if (images is not null && images.Any(i => i.Length > 0))
            {
                var picUploadResult = await _projectPicture.AddPicturesToProject(newProject.ProjectId, supervisor.EmployeeId, images);

                if (!picUploadResult.Success)
                {
                    errors.Add(picUploadResult.Message);
                }
            }

            await _timelineManagement.CreateTimelineEvent(new TimelineDto
            {
                Event = "created the project",
                EmployeeId = supervisor.EmployeeId,
                Type = TimelineType.Create,
                ProjectId = newProject.ProjectId
            }, UserRoles.Supervisor);

            int[] addedEmployees = employeesToAdd.Select(x => x.EmployeeId).ToArray();

            await _notificationManagement.SendNotificationsBulk("You got assigned a new project", $"{supervisor.Username} created a new project #{newProject.ProjectId} and added you and other {addedEmployees.Length - 1} employees ", supervisor.EmployeeId, addedEmployees);

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
            var (projects, totalProjectsCount, totalPages) = await _utilityService.GetAllEntities(filterParams, GetProjectPredicate());

            return new DataCountPages<ProjectDto>
            {
                Data = projects,
                Count = totalProjectsCount,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<ProjectDto>> GetProjectsByCompanyName(int companyId, FilterParams filterParams)
        {
            var (whereExpression, orderByExpression) = _utilityService.BuildWhereAndOrderByExpressions<Project>(companyId, null, "CompanyId", "Created", filterParams);

            bool ShallOrderAscending = filterParams.Sort is not null && filterParams.Sort.Equals("ascending");
            bool ShallOrderDescending = filterParams.Sort is not null && filterParams.Sort.Equals("descending");

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

            List<ProjectDto> projects = new();

            if (ShallOrderAscending)
            {
                projects = await _context.Projects
                    .Where(whereExpression)
                    .OrderBy(orderByExpression)
                    .Select(GetProjectPredicate())
                    .Skip(toSkip)
                    .Take(filterParams.PageSize)
                    .ToListAsync();
            }
            else if (ShallOrderDescending || (!ShallOrderAscending && !ShallOrderDescending))
            {
                projects = await _context.Projects
                    .Where(whereExpression)
                    .OrderByDescending(orderByExpression)
                    .Select(GetProjectPredicate())
                    .Skip(toSkip)
                    .Take(filterParams.PageSize)
                    .ToListAsync();
            }

            return new DataCountPages<ProjectDto>
            {
                Data = projects,
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
                        Lifecycle = x.Lifecycle,
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
                .Include(t => t.Pictures)
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
                    })
                    .OrderBy(x => x.Created).Skip((projectsPage - 1) * projectsPageSize).Take(projectsPageSize),
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

        public async Task<DataCountPages<ProjectDto>> GetProjectsByEmployeeUsername(string username, FilterParams filterParams)
        {
            var (whereExpression, orderByExpression) = _utilityService.BuildWhereAndOrderByExpressions<Project>(null, username, "ProjectCreatorUsername", "Created", filterParams);

            bool ShallOrderAscending = filterParams.Sort is not null && filterParams.Sort.Equals("ascending");
            bool ShallOrderDescending = filterParams.Sort is not null && filterParams.Sort.Equals("descending");

            int totalProjectsCount = await _context.Projects
                .Where(whereExpression)
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalProjectsCount / filterParams.PageSize);

            List<ProjectDto> projects = new();

            int toSkip = (filterParams.Page - 1) * filterParams.PageSize;

            if (ShallOrderAscending)
            {
                projects = await _context.Projects
                    .Where(whereExpression)
                    .OrderBy(orderByExpression)
                    .Select(GetProjectPredicate())
                    .Skip(toSkip)
                    .Take(filterParams.PageSize)
                    .ToListAsync();
            }
            else if (ShallOrderDescending || (!ShallOrderAscending && !ShallOrderDescending))
            {
                projects = await _context.Projects
                    .Where(whereExpression)
                    .OrderByDescending(orderByExpression)
                    .Select(GetProjectPredicate())
                    .Skip(toSkip)
                    .Take(filterParams.PageSize)
                    .ToListAsync();
            }

            return new DataCountPages<ProjectDto>
            {
                Data = projects,
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
                    ClientId = p.Company.CompanyId,
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
        public Expression<Func<Project, ProjectDto>> GetProjectPredicate()
        {
            return project => new ProjectDto
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                Created = project.Created,
                StartedWorking = project.StartedWorking,
                Finished = project.Finished,
                ExpectedDeliveryDate = project.ExpectedDeliveryDate,
                Lifecycle = project.Lifecycle,
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
                }).OrderByDescending(x => x.Username).Take(5).ToList(),
                Creator = new EmployeeShowcaseDto
                {
                    EmployeeId = project.ProjectCreator.EmployeeId,
                    Username = project.ProjectCreator.Username,
                    ProfilePicture = project.ProjectCreator.ProfilePicture
                }
            };
        }

        public async Task<OperationResult> SetProjectsFininishedBulk(int[] projectIds, int employeeId)
        {
            var projects = await _context.Projects
                .Include(x => x.Employees)
                .Where(p => projectIds.Contains(p.ProjectId))
                .ToListAsync();

            if (projects == null || !projects.Any())
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "No projects found"
                };
            }

            var alreadyFinishedProjects = projects.Where(x => x.Finished is not null).ToList();

            bool allProjectsAlreadyFinished = projectIds.All(x => alreadyFinishedProjects.Any(p => p.ProjectId == x));

            if (allProjectsAlreadyFinished)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "All projects are already finished"
                };
            }

            projects.ForEach(p => p.Finished = DateTime.UtcNow);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected is 0)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to finish the projects",
                };
            }

            int[] employeeIdsWorkingInProjects = projects.SelectMany(p => p.Employees.Select(e => e.EmployeeId)).ToArray();

            if (employeeIdsWorkingInProjects.Length > 0)
            {
                _ = await _workloadService.UpdateEmployeeCompletedProjects(employeeIdsWorkingInProjects);
            }

            List<TimelineDto> timelinesToAdd = new();

            projects.ForEach(p => timelinesToAdd.Add(new TimelineDto
            {
                Event = "has set the following project as finished:",
                EmployeeId = employeeId,
                Type = TimelineType.Finish,
                ProjectId = p.ProjectId
            }));

            await _timelineManagement.CreateTimelineEventsBulk(timelinesToAdd, UserRoles.Supervisor);

            return new OperationResult
            {
                Success = true,
                Message = "Projects finished successfully"
            };
        }

        public async Task<OperationResult> SetProjectsStartBulk(int[] projectIds, int employeeId)
        {
            var projects = await _context.Projects
                .Where(p => projectIds.Contains(p.ProjectId))
                .ToListAsync();

            if (projects == null || !projects.Any())
            {
                return new OperationResult
                {
                    Message = "No projects found",
                    Success = false
                };
            }

            var alreadyStartedProjects = projects.Where(p => p.StartedWorking is not null).ToList();

            bool allProjectsStarted = projectIds.All(x => alreadyStartedProjects.Any(p => p.ProjectId == x));

            if (allProjectsStarted)
            {
                return new OperationResult
                {
                    Message = "All projects are already started",
                    Success = false
                };
            }

            projects.Where(p => p.StartedWorking is null).ToList().ForEach(p => p.StartedWorking = DateTime.UtcNow);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected is 0)
            {
                return new OperationResult
                {
                    Message = "Failed to start the projects",
                    Success = false
                };
            }

            List<string> errors = new();
            alreadyStartedProjects.ForEach(p => errors.Add($"Project {p.ProjectId} is already started"));

            if (errors.Any())
            {
                return new OperationResult
                {
                    Message = "Projects started successfully, however some projects were already started",
                    Success = true,
                    Errors = errors
                };
            }

            List<TimelineDto> timelinesToAdd = new();

            projects.ForEach(p => timelinesToAdd.Add(new TimelineDto
            {
                Event = "has set the following project as started:",
                EmployeeId = employeeId,
                Type = TimelineType.Start,
                ProjectId = p.ProjectId
            }));

            await _timelineManagement.CreateTimelineEventsBulk(timelinesToAdd, UserRoles.Supervisor);

            return new OperationResult
            {
                Message = "Projects started successfully",
                Success = true
            };
        }

        public async Task<OperationResult> SetProjectStart(int projectId, int employeeId)
        {
            var project = await _context.Projects.FindAsync(projectId);

            if (project is null)
            {
                return new OperationResult
                {
                    Message = "Project not found",
                    Success = false
                };
            }

            if (project.StartedWorking is not null)
            {
                return new OperationResult
                {
                    Message = "Project is already started",
                    Success = false
                };
            }

            project.StartedWorking = DateTime.UtcNow;

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected is 0)
            {
                return new OperationResult
                {
                    Message = "Failed to start the project",
                    Success = false
                };
            }

            await _timelineManagement.CreateTimelineEvent(new TimelineDto
            {
                Event = "has set the following project as started:",
                EmployeeId = employeeId,
                Type = TimelineType.Start,
                ProjectId = project.ProjectId
            }, UserRoles.Supervisor);

            return new OperationResult
            {
                Message = "Project started successfully",
                Success = true
            };
        }

        public async Task<OperationResult> SetProjectFinished(int projectId, int employeeId)
        {
            var project = await _context.Projects
                .Include(x => x.Employees)
                .FirstOrDefaultAsync(x => x.ProjectId == projectId);

            if (project is null)
            {
                return new OperationResult
                {
                    Message = "Project not found",
                    Success = false
                };
            }

            if (project.Finished is not null)
            {
                return new OperationResult
                {
                    Message = "Project is already finished",
                    Success = false
                };
            }

            project.Finished = DateTime.UtcNow;

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected is 0)
            {
                return new OperationResult
                {
                    Message = "Failed to finish the project",
                    Success = false
                };
            }

            int[] employeeIdsWorkingInProject = project.Employees.Select(e => e.EmployeeId).ToArray();

            if (employeeIdsWorkingInProject.Length > 0)
            {
                _ = await _workloadService.UpdateEmployeeCompletedProjects(employeeIdsWorkingInProject);
            }

            await _timelineManagement.CreateTimelineEvent(new TimelineDto
            {
                Event = "has set the following project as finished:",
                EmployeeId = employeeId,
                Type = TimelineType.Finish,
                ProjectId = project.ProjectId
            }, UserRoles.Supervisor);

            return new OperationResult
            {
                Message = "Project finished successfully",
                Success = true
            };
        }
    }
}
