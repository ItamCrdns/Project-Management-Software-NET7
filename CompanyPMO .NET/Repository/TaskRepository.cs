﻿using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyPMO_.NET.Repository
{
    public class TaskRepository : ITask
    {
        private readonly ApplicationDbContext _context;
        private readonly IImage _imageService;
        private readonly IUtility _utilityService;

        public TaskRepository(ApplicationDbContext context, IImage imageService, IUtility utilityService)
        {
            _context = context;
            _imageService = imageService;
            _utilityService = utilityService;
        }

        public async Task<(string status, IEnumerable<EmployeeShowcaseDto>)> AddEmployeesToTask(int taskId, List<int> employeeIds)
        {
            // Later: check if employee its working in the project & company the task is in
            return await _utilityService.AddEmployeesToEntity<EmployeeTask, Models.Task>(employeeIds, "TaskId", taskId, IsEmployeeAlreadyInTask);
        }

        public async Task<OperationResult<int>> CreateTask(TaskDto task, int employeeId, int projectId, List<IFormFile>? images, List<int>? employeeIds, bool shouldStartNow)
        {
            if (string.IsNullOrWhiteSpace(task.Name) || string.IsNullOrWhiteSpace(task.Description))
            {
                return new OperationResult<int>
                {
                    Success = false,
                    Message = "Task name and description are required",
                    Data = 0
                };
            }

            var project = await _context.Projects.FindAsync(projectId);

            var newTask = new Models.Task
            {
                Name = task.Name,
                Description = task.Description,
                Created = DateTime.UtcNow,
                TaskCreatorId = employeeId,
                ProjectId = projectId,
                ExpectedDeliveryDate = task.ExpectedDeliveryDate,
                StartedWorking = shouldStartNow ? DateTime.UtcNow : null
            };

            _context.Add(newTask);

            project.LatestTaskCreation = DateTime.UtcNow;
            _context.Update(project);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected is 0)
            {
                return new OperationResult<int>
                {
                    Success = false,
                    Message = "Failed to create the Task",
                    Data = 0
                };
            }

            List<Image> imageCollection = new();

            if (images is not null && images.Count > 0)
            {
                imageCollection = await _imageService.AddImagesToNewEntity(images, newTask.TaskId, "Task", null);
            }

            List<string> errors = new();

            if (employeeIds is not null && employeeIds.Count > 0)
            {
                if (employeeIds.Any(x => x == employeeId))
                {
                    errors.Add("You can't add yourself");
                }

                var (status, _) = await AddEmployeesToTask(newTask.TaskId, employeeIds);

                // If its not the success response, then its an error
                if (status != "All employees were added successfully" && !string.IsNullOrWhiteSpace(status))
                {
                    errors.Add(status);
                }
            }

            return new OperationResult<int>
            {
                Success = true,
                Message = "Task created successfully",
                Data = newTask.TaskId,
                Errors = errors
            };
        }

        public async Task<bool> DoesTaskExist(int taskId)
        {
            return await _context.Tasks.AnyAsync(t => t.TaskId.Equals(taskId));
        }

        public async Task<bool> FinishedWorkingOnTask(int userId, int taskId)
        {
            var employeesWorkingOnTask = await GetEmployeesWorkingOnTask(taskId);

            bool isUserIdAnEmployeeWorkingOnTheTask = employeesWorkingOnTask.Any(e => e.EmployeeId.Equals(userId));

            if (isUserIdAnEmployeeWorkingOnTheTask)
            {
                var taskToUpdate = await _context.Tasks.FindAsync(taskId);

                if (taskToUpdate is not null && taskToUpdate.StartedWorking is not null) // If tasks does not have a startedWorking date do not change
                {
                    taskToUpdate.Finished = DateTime.UtcNow;
                    _context.Update(taskToUpdate);
                    return await _context.SaveChangesAsync() > 0;
                }
            }
            return false;
        }

        public async Task<List<Employee>> GetEmployeesWorkingOnTask(int taskId) => await _context.Tasks.Where(t => t.TaskId.Equals(taskId)).Include(e => e.Employees).SelectMany(e => e.Employees).ToListAsync();

        public async Task<EntityParticipantOrOwnerDTO<TaskDto>?> GetTaskById(int taskId, int projectId, int userId)
            {
           return await _context.Tasks
                .Where(t => t.TaskId == taskId && t.ProjectId == projectId)
                .Select(x => new EntityParticipantOrOwnerDTO<TaskDto>
                {
                    Entity = new TaskDto
                    {
                        TaskId = x.TaskId,
                        Name = x.Name,
                        Description = x.Description,
                        Created = x.Created,
                        StartedWorking = x.StartedWorking,
                        Finished = x.Finished,
                        TaskCreator = new EmployeeShowcaseDto
                        {
                            EmployeeId = x.TaskCreator.EmployeeId,
                            Username = x.TaskCreator.Username,
                            ProfilePicture = x.TaskCreator.ProfilePicture
                        },
                        Employees = x.Employees.Select(employee => new EmployeeShowcaseDto
                        {
                            EmployeeId = employee.EmployeeId,
                            Username = employee.Username,
                            ProfilePicture = employee.ProfilePicture,
                        }).Take(5).ToList(),
                        EmployeeCount = x.Employees.Count,
                        Project = new ProjectShowcaseDto
                        {
                            ProjectId = x.Project.ProjectId,
                            Name = x.Project.Name,
                            Priority = x.Project.Priority
                        }
                    },
                    IsOwner = x.TaskCreatorId == userId,
                    IsParticipant = x.Employees.Any(e => e.EmployeeId == userId)
                })
                .FirstOrDefaultAsync(); 
        }

        public async Task<List<Models.Task>> GetTasks(int page, int pageSize)
        {
            int postsToSkip = (page - 1) * pageSize;

            var tasks = await _context.Tasks
                .OrderByDescending(p => p.Created)
                .Include(t => t.Images)
                .Skip(postsToSkip)
                .Take(pageSize)
                .ToListAsync();

            foreach (var task in tasks)
            {
                task.Images = SelectImages(task.Images);
            }

            return tasks;
        }

        public async Task<DataCountPages<TaskShowcaseDto>> GetTasksShowcaseByEmployeeUsername(string username, int page, int pageSize)
        {
            var (taskIds, totalTasksCount, totalPages) = await _utilityService.GetEntitiesEmployeeCreatedOrParticipates<EmployeeTask, Models.Task>(username, "TaskCreatorId", "TaskId", page, pageSize);

            ICollection<TaskShowcaseDto> tasks = await _context.Tasks
                .Where(t => taskIds.Contains(t.TaskId))
                .Select(t => new TaskShowcaseDto
                {
                    TaskId = t.TaskId,
                    Name = t.Name
                })
                .ToListAsync();

            return new DataCountPages<TaskShowcaseDto>
            {
                Data = tasks,
                Count = totalTasksCount,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<TaskDto>> GetTasksByProjectId(int projectId, FilterParams filterParams)
        {
            // ATM We are just ordering the entities so this count and pages are actually good i think
            var (taskIds, totalTasksCount, totalPages) = await _utilityService.GetEntitiesByEntityId<Models.Task>(projectId, "ProjectId", "TaskId", filterParams.Page, filterParams.PageSize);

            var (whereExpression, orderByExpression) = _utilityService.BuildWhereAndOrderByExpressions<Models.Task>(projectId, taskIds, "TaskId", "ProjectId", "Created", filterParams);

            bool ShallOrderAscending = filterParams.Sort is not null && filterParams.Sort.Equals("ascending");
            bool ShallOrderDescending = filterParams.Sort is not null && filterParams.Sort.Equals("descending");

            List<Models.Task> tasks = new();

            if (ShallOrderAscending)
            {
                tasks = await _context.Tasks
                    .Where(whereExpression)
                    .OrderBy(orderByExpression)
                    .Include(t => t.TaskCreator)
                    .Include(e => e.Employees)
                    .Include(p => p.Project)
                    .ToListAsync();
            }
            else if (ShallOrderDescending || (!ShallOrderAscending && !ShallOrderDescending))
            {
                tasks = await _context.Tasks
                    .Where(whereExpression)
                    .OrderByDescending(orderByExpression)
                    .Include(t => t.TaskCreator)
                    .Include(e => e.Employees)
                    .Include(p => p.Project)
                    .ToListAsync();
            }

            var taskDtos = TaskDtoSelectQuery(tasks);

            return new DataCountPages<TaskDto>
            {
                Data = taskDtos,
                Count = totalTasksCount,
                Pages = totalPages
            };
        }

        public async Task<bool> IsEmployeeAlreadyInTask(int employeeId, int taskId) => await _context.EmployeeTasks.AnyAsync(x => x.EmployeeId.Equals(employeeId) && x.TaskId.Equals(taskId));

        public ICollection<Image> SelectImages(ICollection<Image> images)
        {
            var projectImages = images
                .Where(et => et.EntityType.Equals("Task"))
                .Select(i => new Image
                {
                    ImageId = i.ImageId,
                    EntityType = i.EntityType,
                    EntityId = i.EntityId,
                    ImageUrl = i.ImageUrl,
                    PublicId = i.PublicId,
                    Created = i.Created,
                    UploaderId = i.UploaderId
                }).ToList();

            return projectImages;
        }

        public async Task<bool> StartingWorkingOnTask(int userId, int taskId)
        {
            var employeesWorkingOnTask = await GetEmployeesWorkingOnTask(taskId);

            bool isUserIdAnEmployeeWorkingOnTheTask = employeesWorkingOnTask.Any(e => e.EmployeeId.Equals(userId));

            if (isUserIdAnEmployeeWorkingOnTheTask)
            {
                var taskToUpdate = await _context.Tasks.FindAsync(taskId);

                if (taskToUpdate is not null)
                {
                    taskToUpdate.StartedWorking = DateTime.UtcNow;
                    _context.Update(taskToUpdate);
                    return await _context.SaveChangesAsync() > 0;
                }
            }

            return false; // Task to update was null
        }

        public async Task<DataCountPages<TaskDto>> GetTasksByEmployeeUsername(string username, int page, int pageSize)
        {
            var (taskIds, totalTasksCount, totalPages) = await _utilityService.GetEntitiesByEmployeeUsername<EmployeeTask>(username, "TaskId", page, pageSize);

            ICollection<TaskDto> tasks = await _context.Tasks
                .Where(t => taskIds.Contains(t.TaskId))
                .Select(t => new TaskDto
                {
                    TaskId = t.TaskId,
                    Name = t.Name,
                    Description = t.Description,
                    Created = t.Created,
                    StartedWorking = t.StartedWorking,
                    Finished = t.Finished
                })
                .ToListAsync();

            return new DataCountPages<TaskDto>
            {
                Data = tasks,
                Count = totalTasksCount,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<TaskShowcaseDto>> GetTasksShowcaseByProjectId(int projectId, int page, int pageSize)
        {
            var tasks = await _context.Tasks
                .Where(t => t.ProjectId.Equals(projectId))
                .OrderByDescending(t => t.Created)
                .Select(t => new TaskShowcaseDto
                {
                    TaskId = t.TaskId,
                    Name = t.Name,
                    Created = t.Created,
                    Project = new ProjectSomeInfoDto
                    {
                        ProjectId = t.Project.ProjectId,
                        Name = t.Project.Name,
                        Priority = t.Project.Priority
                    },
                    TaskCreator = new EmployeeShowcaseDto
                    {
                        EmployeeId = t.TaskCreator.EmployeeId,
                        Username = t.TaskCreator.Username,
                        ProfilePicture = t.TaskCreator.ProfilePicture
                    },
                    Employees = t.Employees.Select(employee => new EmployeeShowcaseDto
                    {
                        EmployeeId = employee.EmployeeId,
                        Username = employee.Username,
                        ProfilePicture = employee.ProfilePicture,
                    }).ToList()
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int totalTasksCount = await _context.Tasks.CountAsync(t => t.ProjectId.Equals(projectId));

            int totalPages = (int)Math.Ceiling((double)totalTasksCount / pageSize);

            return new DataCountPages<TaskShowcaseDto>
            {
                Data = tasks,
                Count = totalTasksCount,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<TaskShowcaseDto>> GetAllTasksShowcase(int page, int pageSize)
        {
            // Admin only endpoint. Get all projects without any additional information (showcase only)

            int toSkip = (page - 1) * pageSize;

            ICollection<TaskShowcaseDto> tasks = await _context.Tasks
                .OrderByDescending(t => t.Created)
                .Select(t => new TaskShowcaseDto
                {
                    TaskId = t.TaskId,
                    Name = t.Name
                })
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            int totalTasksCount = await _context.Tasks.CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalTasksCount / pageSize);

            return new DataCountPages<TaskShowcaseDto>
            {
                Data = tasks,
                Count = totalTasksCount,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<TaskDto>> GetAllTasks(FilterParams filterParams)
        {
            List<string> navProperties = new() { "Employees", "TaskCreator", "Project" };

            var (tasks, totalTasksCount, totalPages) = await _utilityService.GetAllEntities<Models.Task>(filterParams, navProperties);

            var taskDtos = TaskDtoSelectQuery(tasks);

            return new DataCountPages<TaskDto>
            {
                Data = taskDtos,
                Count = totalTasksCount,
                Pages = totalPages
            };
        }

        public IEnumerable<TaskDto> TaskDtoSelectQuery(ICollection<Models.Task> tasks)
        {
            var taskDtos = tasks.Select(task => new TaskDto
            {
                TaskId = task.TaskId,
                Name = task.Name,
                Description = task.Description,
                Created = task.Created,
                StartedWorking = task.StartedWorking,
                Finished = task.Finished,
                TaskCreator = new EmployeeShowcaseDto
                {
                    EmployeeId = task.TaskCreator.EmployeeId,
                    Username = task.TaskCreator.Username,
                    ProfilePicture = task.TaskCreator.ProfilePicture
                },
                Employees = task?.Employees?.Select(employee => new EmployeeShowcaseDto
                {
                    EmployeeId = employee.EmployeeId,
                    Username = employee.Username,
                    ProfilePicture = employee.ProfilePicture,
                }).ToList(),
                Project = new ProjectShowcaseDto
                {
                    ProjectId = task.Project.ProjectId,
                    Name = task.Project.Name,
                    Priority = task.Project.Priority
                }
            }).ToList();

            return taskDtos;
        }

        public async Task<DataCountPages<ProjectTaskGroup>> GetTasksGroupedByProject(FilterParams filterParams, int tasksPage, int tasksPageSize, int employeeId)
        {
            List<ProjectTaskGroup> tasks = await _context.Tasks
                .GroupBy(t => t.Project)
                .Select(x => new ProjectTaskGroup
                {
                    ProjectName = x.Key.Name,
                    ProjectId = x.Key.ProjectId,
                    IsCurrentUserOwner = x.Key.ProjectCreatorId == employeeId,
                    IsCurrentUserInTeam = _context.EmployeeProjects.Any(t => t.ProjectId == x.Key.ProjectId && t.EmployeeId == employeeId),
                    Tasks = x.Select(t => new TaskShowcaseDto
                    {
                        TaskId = t.TaskId,
                        Name = t.Name,
                        Created = t.Created,
                        Project = new ProjectSomeInfoDto
                        {
                            ProjectId = t.Project.ProjectId,
                            Name = t.Project.Name,
                            Priority = t.Project.Priority,
                            Lifecycle = t.Project.Lifecycle
                        },
                        TaskCreator = new EmployeeShowcaseDto
                        {
                            EmployeeId = t.TaskCreator.EmployeeId,
                            Username = t.TaskCreator.Username,
                            ProfilePicture = t.TaskCreator.ProfilePicture
                        },
                        Employees = t.Employees.Select(employee => new EmployeeShowcaseDto
                        {
                            EmployeeId = employee.EmployeeId,
                            Username = employee.Username,
                            ProfilePicture = employee.ProfilePicture,
                        }).ToList()
                    }).OrderBy(t => t.Created).Skip((tasksPage - 1) * tasksPageSize).Take(tasksPageSize),
                    Count = x.Count(),
                    Pages = (int)Math.Ceiling((double)x.Count() / tasksPageSize),
                    LatestTaskCreation = x.Key.LatestTaskCreation
                })
                .Skip((filterParams.Page - 1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .OrderByDescending(t => t.LatestTaskCreation)
                .ToListAsync();

            int totalProjectsWithTasks = await _context.Projects.CountAsync(p => _context.Tasks.Any(t => t.ProjectId == p.ProjectId));

            int totalPages = (int)Math.Ceiling((double)totalProjectsWithTasks / filterParams.PageSize);

            return new DataCountPages<ProjectTaskGroup>
            {
                Data = tasks,
                Count = totalProjectsWithTasks,
                Pages = totalPages
            };
        }
    }
}
