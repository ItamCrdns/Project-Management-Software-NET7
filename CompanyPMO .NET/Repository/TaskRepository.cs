using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Hubs;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Interfaces.Task_interfaces;
using CompanyPMO_.NET.Interfaces.Timeline_interfaces;
using CompanyPMO_.NET.Interfaces.Workload_interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Task = CompanyPMO_.NET.Models.Task;

namespace CompanyPMO_.NET.Repository
{
    public class TaskRepository : ITask, ITaskEmployeeQueries, ITaskProjectQueries, ITaskManagement
    {
        private readonly ApplicationDbContext _context;
        private readonly IImage _imageService;
        private readonly IUtility _utilityService;
        private readonly IWorkloadTask _workloadService;
        private readonly ITimelineManagement _timelineManagement;

        public TaskRepository(ApplicationDbContext context, IImage imageService, IUtility utilityService, IWorkloadTask workloadService, ITimelineManagement timelineManagement)
        {
            _context = context;
            _imageService = imageService;
            _utilityService = utilityService;
            _workloadService = workloadService;
            _timelineManagement = timelineManagement;
        }

        public async Task<OperationResult<int>> CreateTask(TaskDto task, int employeeSupervisorId, int projectId, List<IFormFile>? images, List<int>? employeeIds, bool shouldStartNow)
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

            bool isUserOwner = await _context.Projects.AnyAsync(p => p.ProjectId.Equals(projectId) && p.ProjectCreatorId.Equals(employeeSupervisorId));

            if (project is null || !isUserOwner)
            {
                return new OperationResult<int>
                {
                    Success = false,
                    Message = "Project not found or you are not the owner",
                    Data = 0
                };
            }

            var newTask = new Task
            {
                Name = task.Name,
                Description = task.Description,
                Created = DateTime.UtcNow,
                TaskCreatorId = employeeSupervisorId,
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

            List<TaskPicture> imageCollection = new();

            if (images is not null && images.Count > 0)
            {
                // Not implemented yet
            }

            List<string> errors = new();

            var workloadCreatedTasksResult = await _workloadService.UpdateEmployeeCreatedTasks(employeeSupervisorId);

            if (!workloadCreatedTasksResult.Success)
            {
                errors.Add($"Failed to update the workload of the task creator. Error = {workloadCreatedTasksResult.Message}");
            }

            if (employeeIds is not null && employeeIds.Count > 0)
            {
                var employeesToAdd = employeeIds.Where(employee => employee != employeeSupervisorId).Select(employee => new EmployeeTask
                {
                    EmployeeId = employee,
                    TaskId = newTask.TaskId
                });

                if (employeeIds.Any(x => x == employeeSupervisorId))
                {
                    errors.Add("You can't add yourself");
                }

                await _context.EmployeeTasks.AddRangeAsync(employeesToAdd);
                int employeeRowsAffected = await _context.SaveChangesAsync();

                var workloadUpdateResult = await _workloadService.UpdateEmployeeAssignedTasks(employeeIds.ToArray());

                if (!workloadUpdateResult.Success)
                {
                    errors.Add($"Failed to update the workload of the employees. Error = {workloadUpdateResult.Message}");
                }

                if (employeeRowsAffected is 0)
                {
                    errors.Add("Failed to add the employees to the task");
                }
            }

            var timelineEvent = new TimelineDto
            {
                Event = "created the task",
                EmployeeId = employeeSupervisorId,
                Type = TimelineType.Create,
                TaskId = newTask.TaskId
            };

            await _timelineManagement.CreateTimelineEvent(timelineEvent, UserRoles.Supervisor);

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
                         ExpectedDeliveryDate = x.ExpectedDeliveryDate,
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
                             Priority = x.Project.Priority,
                             ClientId = x.Project.CompanyId
                         }
                     },
                     IsOwner = x.TaskCreatorId == userId,
                     IsParticipant = x.Employees.Any(e => e.EmployeeId == userId)
                 })
                 .FirstOrDefaultAsync();
        }

        public async Task<List<Task>> GetTasks(int page, int pageSize)
        {
            int postsToSkip = (page - 1) * pageSize;

            var tasks = await _context.Tasks
                .OrderByDescending(p => p.Created)
                .Include(t => t.Pictures)
                .Skip(postsToSkip)
                .Take(pageSize)
                .ToListAsync();

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
            //var (taskIds, totalTasksCount, totalPages) = await _utilityService.GetEntitiesByEntityId<Task>(projectId, "ProjectId", "TaskId", filterParams.Page, filterParams.PageSize);

            var (whereExpression, orderByExpression) = _utilityService.BuildWhereAndOrderByExpressions<Task>(projectId, null, "ProjectId", "Created", filterParams);

            bool shallOrderAscending = filterParams.Sort is not null && filterParams.Sort.Equals("ascending");
            bool shallOrderDescending = filterParams.Sort is not null && filterParams.Sort.Equals("descending");

            int totalTasksCount = await _context.Tasks
                .Where(whereExpression)
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalTasksCount / filterParams.PageSize);

            List<TaskDto> tasks = new();

            int toSkip = (filterParams.Page - 1) * filterParams.PageSize;

            if (shallOrderAscending)
            {
                tasks = await _context.Tasks
                    .Where(whereExpression)
                    .OrderBy(orderByExpression)
                    .Select(GetTaskPredicate())
                    .Skip(toSkip)
                    .Take(filterParams.PageSize)
                    .ToListAsync();
            }
            else if (shallOrderDescending || (!shallOrderAscending && !shallOrderDescending))
            {
                tasks = await _context.Tasks
                    .Where(whereExpression)
                    .OrderByDescending(orderByExpression)
                    .Select(GetTaskPredicate())
                    .Skip(toSkip)
                    .Take(filterParams.PageSize)
                    .ToListAsync();
            }

            return new DataCountPages<TaskDto>
            {
                Data = tasks,
                Count = totalTasksCount,
                Pages = totalPages
            };
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
            var (tasks, totalTasksCount, totalPages) = await _utilityService.GetAllEntities(filterParams, GetTaskPredicate());

            return new DataCountPages<TaskDto>
            {
                Data = tasks,
                Count = totalTasksCount,
                Pages = totalPages
            };
        }

        public async Task<DataCountPages<ProjectTaskGroup>> GetTasksGroupedByProject(FilterParams filterParams, int tasksPage, int tasksPageSize, int employeeId)
        {
            List<ProjectTaskGroup> tasks = await _context.Tasks
                .GroupBy(t => t.Project)
                .Select(x => new ProjectTaskGroup
                {
                    ProjectName = x.Key.Name,
                    ProjectId = x.Key.ProjectId,
                    ClientId = x.Key.CompanyId,
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

        public async Task<bool> IsParticipant(int taskId, int employeeId) => await _context.EmployeeTasks
            .AnyAsync(et => et.TaskId.Equals(taskId) && et.EmployeeId.Equals(employeeId));

        public async Task<bool> IsOwner(int taskId, int employeeId) => await _context.Tasks
            .AnyAsync(t => t.TaskId.Equals(taskId) && t.TaskCreatorId.Equals(employeeId));

        public Expression<Func<Task, TaskDto>> GetTaskPredicate()
        {
            return t => new TaskDto
            {
                TaskId = t.TaskId,
                Name = t.Name,
                Description = t.Description,
                Created = t.Created,
                StartedWorking = t.StartedWorking,
                Finished = t.Finished,
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
                }).OrderByDescending(x => x.Username).Take(5).ToList(),
                Project = new ProjectShowcaseDto
                {
                    ProjectId = t.Project.ProjectId,
                    Name = t.Project.Name,
                    Priority = t.Project.Priority,
                    ClientId = t.Project.CompanyId
                }
            };
        }

        public async Task<OperationResult> SetTasksStartBulk(int[] taskIds, int employeeId)
        {
            var tasks = await _context.Tasks
                .Where(x => taskIds.Contains(x.TaskId))
                .ToListAsync();

            if (tasks == null || !tasks.Any())
            {
                return new OperationResult
                {
                    Message = "No tasks found",
                    Success = false
                };
            }

            var alreadyStartedTasks = tasks.Where(x => x.StartedWorking is not null).ToList();

            bool allTasksAlreadyStarted = taskIds.All(x => alreadyStartedTasks.Any(t => t.TaskId == x));

            if (allTasksAlreadyStarted)
            {
                return new OperationResult
                {
                    Message = "All tasks are already started",
                    Success = false
                };
            }

            tasks.Where(x => x.StartedWorking is null).ToList().ForEach(x => x.StartedWorking = DateTime.UtcNow);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected is 0)
            {
                return new OperationResult
                {
                    Message = "Failed to start the tasks",
                    Success = false
                };
            }

            List<string> errors = new();
            alreadyStartedTasks.ForEach(x => errors.Add($"Task {x.TaskId} is already started"));

            if (errors.Any())
            {
                return new OperationResult
                {
                    Message = "Tasks started successfully, however some tasks were already started",
                    Success = true,
                    Errors = errors
                };
            }

            List<TimelineDto> timelinesToAdd = new();

            tasks.ForEach(p => timelinesToAdd.Add(new TimelineDto
            {
                Event = "has set the following task as started:",
                EmployeeId = employeeId,
                Type = TimelineType.Finish,
                TaskId = p.TaskId
            }));

            await _timelineManagement.CreateTimelineEventsBulk(timelinesToAdd, UserRoles.Supervisor);

            return new OperationResult
            {
                Message = "Tasks started successfully",
                Success = true
            };
        }

        public async Task<OperationResult> SetTaskStart(int taskId, int employeeId)
        {
            var task = await _context.Tasks.FindAsync(taskId);

            if (task is null)
            {
                return new OperationResult
                {
                    Message = "Task not found",
                    Success = false
                };
            }

            if (task.StartedWorking is not null)
            {
                return new OperationResult
                {
                    Message = "Task is already started",
                    Success = false
                };
            }

            task.StartedWorking = DateTime.UtcNow;

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected is 0)
            {
                return new OperationResult
                {
                    Message = "Failed to start the task",
                    Success = false
                };
            }

            var timelineEvent = new TimelineDto
            {
                Event = "has set the following task as started:",
                EmployeeId = employeeId,
                Type = TimelineType.Start,
                TaskId = taskId
            };

            await _timelineManagement.CreateTimelineEvent(timelineEvent, UserRoles.Supervisor);

            return new OperationResult
            {
                Message = "Task started successfully",
                Success = true
            };
        }

        public async Task<OperationResult> SetTasksFinishedBulk(int[] taskIds, int employeeId)
        {
            var tasks = await _context.Tasks
                .Include(t => t.Employees)
                .Where(x => taskIds.Contains(x.TaskId))
                .ToListAsync();

            if (tasks == null || !tasks.Any()) 
            {
                return new OperationResult
                {
                    Message = "No tasks found",
                    Success = false
                };
            }

            var alreadyFinishedTasks = tasks.Where(x => x.Finished is not null).ToList();

            bool allTasksAlreadyFinished = taskIds.All(x => alreadyFinishedTasks.Any(t => t.TaskId == x));

            if (allTasksAlreadyFinished)
            {
                return new OperationResult
                {
                    Message = "All tasks are already finished",
                    Success = false
                };
            }

            tasks.Where(x => x.Finished is null).ToList().ForEach(x => x.Finished = DateTime.UtcNow);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected is 0)
            {
                return new OperationResult
                {
                    Message = "Failed to finish the tasks",
                    Success = false
                };
            }

            List<string> errors = new();
            alreadyFinishedTasks.ForEach(x => errors.Add($"Task {x.TaskId} is already finished"));

            int[] employeeIdsWorkingOnTask = tasks.SelectMany(x => x.Employees).Select(x => x.EmployeeId).ToArray();

            if (employeeIdsWorkingOnTask.Length > 0)
            {
                _ = await _workloadService.UpdateEmployeeCompletedTasks(employeeIdsWorkingOnTask);
            }
            
            if (errors.Any())
            {
                return new OperationResult
                {
                    Message = "Tasks finished successfully, however some tasks were already finished",
                    Success = true,
                    Errors = errors
                };
            }

            List<TimelineDto> timelinesToAdd = new();

            tasks.ForEach(p => timelinesToAdd.Add(new TimelineDto
            {
                Event = "has set the following task as finished:",
                EmployeeId = employeeId,
                Type = TimelineType.Finish,
                TaskId = p.TaskId
            }));

            await _timelineManagement.CreateTimelineEventsBulk(timelinesToAdd, UserRoles.Supervisor);

            return new OperationResult
            {
                Message = "Tasks finished successfully",
                Success = true,
                Errors = errors
            };
        }

        public async Task<OperationResult> SetTaskFinished(int taskId, int employeeId)
        {
            var task = await _context.Tasks
                .Include(t => t.Employees)
                .FirstOrDefaultAsync(t => t.TaskId.Equals(taskId));

            if (task is null)
            {
                return new OperationResult
                {
                    Message = "Task not found",
                    Success = false
                };
            }

            if (task.Finished is not null)
            {
                return new OperationResult
                {
                    Message = "Task is already finished",
                    Success = false
                };
            }

            task.Finished = DateTime.UtcNow;

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected is 0)
            {
                return new OperationResult
                {
                    Message = "Failed to finish the task",
                    Success = false
                };
            }

            int[] employeeIdsWorkingOnTask = task.Employees.Select(x => x.EmployeeId).ToArray();

            if (employeeIdsWorkingOnTask.Length > 0)
            {
                _ = await _workloadService.UpdateEmployeeCompletedTasks(employeeIdsWorkingOnTask);
            }

            var timelineEvent = new TimelineDto
            {
                Event = "has set the following task as finished:",
                EmployeeId = employeeId,
                Type = TimelineType.Finish,
                TaskId = taskId
            };

            await _timelineManagement.CreateTimelineEvent(timelineEvent, UserRoles.Supervisor);

            return new OperationResult
            {
                Message = "Task finished successfully",
                Success = true
            };
        }
    }
}
