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

        public async Task<(Models.Task, List<Image>)> CreateTask(TaskDto task, int employeeId, int projectId, List<IFormFile>? images)
        {
            bool taskNameIsntNull = !string.IsNullOrWhiteSpace(task.Name);
            bool taskDescriptionIsntNull = !string.IsNullOrWhiteSpace(task.Description);

            var project = await _context.Projects.FindAsync(projectId);

            if (taskNameIsntNull && taskDescriptionIsntNull && project is not null)
            {
                var newTask = new Models.Task
                {
                    Name = task.Name,
                    Description = task.Description,
                    Created = DateTime.UtcNow,
                    TaskCreatorId = employeeId,
                    ProjectId = projectId
                };

                _context.Add(newTask);

                project.LatestTaskCreation = DateTime.UtcNow;
                _context.Update(project);

                int rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected is 0)
                {
                    return (null, null);
                }

                List<Image> imageCollection = new();

                if (images is not null && images.Count > 0)
                {
                    imageCollection = await _imageService.AddImagesToNewEntity(images, newTask.TaskId, "Task", null);
                }

                return (newTask, imageCollection);
            } else
            {
                return (null, null);
            }
        }

        public async Task<bool> DoesTaskExist(int taskId)
        {
            return await _context.Tasks.AnyAsync(t => t.TaskId.Equals(taskId));
        }

        public async Task<bool> FinishedWorkingOnTask(int userId, int taskId)
        {
            var employeesWorkingOnTask = await GetEmployeesWorkingOnTask(taskId);

            bool isUserIdAnEmployeeWorkingOnTheTask = employeesWorkingOnTask.Any(e => e.EmployeeId.Equals(userId));

            if(isUserIdAnEmployeeWorkingOnTheTask)
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

        public async Task<Models.Task> GetTaskById(int taskId) => await _context.Tasks.FindAsync(taskId);

        public async Task<List<Models.Task>> GetTasks(int page, int pageSize)
        {
            int postsToSkip = (page - 1) * pageSize;

            var tasks = await _context.Tasks
                .OrderByDescending(p => p.Created)
                .Include(t => t.Images)
                .Skip(postsToSkip)
                .Take(pageSize)
                .ToListAsync();

            foreach(var task in tasks)
            {
                task.Images = SelectImages(task.Images);
            }

            return tasks;
        }

        public async Task<DataCountAndPagesizeDto<ICollection<TaskShowcaseDto>>> GetTasksShowcaseByEmployeeUsername(string username, int page, int pageSize)
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

            var result = new DataCountAndPagesizeDto<ICollection<TaskShowcaseDto>>
            {
                Data = tasks,
                Count = totalTasksCount,
                Pages = totalPages
            };

            return result;
        }

        public async Task<DataCountAndPagesizeDto<IEnumerable<TaskDto>>> GetTasksByProjectId(int projectId, FilterParams filterParams)
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
            } else if (ShallOrderDescending || (!ShallOrderAscending && !ShallOrderDescending))
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

            var result = new DataCountAndPagesizeDto<IEnumerable<TaskDto>>
            {
                Data = taskDtos,
                Count = totalTasksCount,
                Pages = totalPages
            };

            return result;
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

        public async Task<DataCountAndPagesizeDto<ICollection<TaskDto>>> GetTasksByEmployeeUsername(string username, int page, int pageSize)
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

            var result = new DataCountAndPagesizeDto<ICollection<TaskDto>>
            {
                Data = tasks,
                Count = totalTasksCount,
                Pages = totalPages
            };

            return result;
        }

        public Task<Dictionary<string, object>> GetTasksShowcaseByProjectId(int projectId, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<DataCountAndPagesizeDto<ICollection<TaskShowcaseDto>>> GetAllTasksShowcase(int page, int pageSize)
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

            var result = new DataCountAndPagesizeDto<ICollection<TaskShowcaseDto>>
            {
                Data = tasks,
                Count = totalTasksCount,
                Pages = totalPages
            };

            return result;
        }

        public async Task<DataCountAndPagesizeDto<IEnumerable<TaskDto>>> GetAllTasks(FilterParams filterParams)
        {
            List<string> navProperties = new() { "Employees", "TaskCreator", "Project" };

            var (tasks, totalTasksCount, totalPages) = await _utilityService.GetAllEntities<Models.Task>(filterParams, navProperties);

            var taskDtos = TaskDtoSelectQuery(tasks);

            var result = new DataCountAndPagesizeDto<IEnumerable<TaskDto>>
            {
                Data = taskDtos,
                Count = totalTasksCount,
                Pages = totalPages
            };

            return result;
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

        public async Task<DataCountAndPagesizeDto<List<ProjectTaskGroup>>> GetTasksGroupedByProject(FilterParams filterParams, int tasksPage, int tasksPageSize)
        {
            List<ProjectTaskGroup> tasks = await _context.Tasks
                .GroupBy(t => t.Project)
                .Select(x => new ProjectTaskGroup
                {
                    ProjectName = x.Key.Name,
                    Tasks = x.Select(t => new TaskShowcaseDto
                    {
                        TaskId = t.TaskId,
                        Name = t.Name,
                        Created = t.Created,
                        Project = new ProjectShowcaseDto
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

            return new DataCountAndPagesizeDto<List<ProjectTaskGroup>>
            {
                Data = tasks,
                Count = totalProjectsWithTasks,
                Pages = totalPages
            };
        }
    }
}
