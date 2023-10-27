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

        public async Task<(string status, IEnumerable<EmployeeDto>)> AddEmployeesToTask(int taskId, List<int> employeeIds)
        {
            // Later: check if employee its working in the project & company the task is in
            return await _utilityService.AddEmployeesToEntity<EmployeeTask, Models.Task>(employeeIds, "TaskId", taskId, IsEmployeeAlreadyInTask);
        }

        public async Task<(Models.Task, List<Image>)> CreateTask(Models.Task task, int employeeId, int projectId, List<IFormFile>? images)
        {
            var newTask = new Models.Task
            {
                Name = task.Name,
                Description = task.Description,
                Created = DateTimeOffset.UtcNow,
                TaskCreatorId = employeeId,
                ProjectId = projectId
            };

            _context.Add(newTask);
            _ = await _context.SaveChangesAsync();

            List<Image> imageCollection = new();

            if(images is not null && images.Any(i => i.Length >  0))
            {
                imageCollection = await _imageService.AddImagesToNewEntity(images, newTask.TaskId, "Task", null);
            }

            return (newTask, imageCollection);
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
                    taskToUpdate.Finished = DateTimeOffset.UtcNow;
                    _context.Update(taskToUpdate);
                    return await _context.SaveChangesAsync() > 0;
                }
            }

            return false;
        }

        public async Task<List<Employee>> GetEmployeesWorkingOnTask(int taskId)
        {
            var employees = await _context.Tasks
                .Where(t => t.TaskId.Equals(taskId))
                .Include(e => e.Employees)
                .SelectMany(e => e.Employees)
                .ToListAsync();

            return employees;
        }

        public async Task<Models.Task> GetTaskById(int taskId)
        {
            var task = await _context.Tasks
                .Where(t => t.TaskId.Equals(taskId))
                .Include(i => i.Images)
                .FirstOrDefaultAsync();

            //var tasksImages = task.Images

            return task;
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

            foreach(var task in tasks)
            {
                task.Images = SelectImages(task.Images);
            }

            return tasks;
        }

        public async Task<Dictionary<string, object>> GetTasksShowcaseByEmployeeUsername(string username, int page, int pageSize)
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

            var result = new Dictionary<string, object>
            {
                { "data", tasks },
                { "count", totalTasksCount },
                { "pages", totalPages}
            };

            return result;
        }

        public async Task<Dictionary<string, object>> GetTasksByProjectId(int projectId, int page, int pageSize)
        {
            var (taskIds, totalTasksCount, totalPages) = await _utilityService.GetEntitiesByEntityId<Models.Task>(projectId, "ProjectId", "TaskId", page, pageSize);

            var tasks = await _context.Tasks
                .OrderByDescending(t => t.Created)
                .Where(task => taskIds.Contains(task.TaskId))
                .Include(t => t.TaskCreator)
                .Include(e => e.Employees)
                .Include(p => p.Project)
                .ToListAsync();

            var taskDtos = TaskDtoSelectQuery(tasks);

            var result = new Dictionary<string, object>
            {
                { "data", taskDtos },
                { "count", totalTasksCount },
                { "pages", totalPages }
            };

            return result;
        }

        public async Task<bool> IsEmployeeAlreadyInTask(int employeeId, int taskId)
        {
            return await _context.EmployeeTasks
                .AnyAsync(et => et.EmployeeId.Equals(employeeId) && et.TaskId.Equals(taskId));
        }

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
                    Created = i.Created
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
                    taskToUpdate.StartedWorking = DateTimeOffset.UtcNow;
                    _context.Update(taskToUpdate);
                    return await _context.SaveChangesAsync() > 0;
                }
            }

            return false; // Task to update was null
        }

        public async Task<Dictionary<string, object>> GetTasksByEmployeeUsername(string username, int page, int pageSize)
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

            var result = new Dictionary<string, object>
            {
                { "data", tasks },
                { "count", totalTasksCount },
                { "pages", totalPages }
            };

            return result;
        }

        public Task<Dictionary<string, object>> GetTasksShowcaseByProjectId(int projectId, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> GetAllTasksShowcase(int page, int pageSize)
        {
            // Admin only endpoint. Get all projects without any additional information (showcase only)

            int toSkip = (page - 1) * pageSize;
            IEnumerable<TaskShowcaseDto> tasks = await _context.Tasks
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

            var result = new Dictionary<string, object>
            {
                { "data", tasks },
                { "count", totalTasksCount },
                { "pages", totalPages }
            };

            return result;
        }

        public async Task<Dictionary<string, object>> GetAllTasks(int page, int pageSize)
        {
            int toSkip = (page - 1) * pageSize;

            var tasks = await _context.Tasks
                .OrderByDescending(t => t.Created)
                .Include(e => e.Employees)
                .Include(t => t.TaskCreator)
                .Include(p => p.Project)
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            int totalTasksCount = await _context.Tasks.CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalTasksCount / pageSize);

            var taskDtos = TaskDtoSelectQuery(tasks);

            var result = new Dictionary<string, object>
            {
                { "data", taskDtos },
                { "count", totalTasksCount },
                { "pages", totalPages }
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
                Employees = task.Employees.Select(employee => new EmployeeShowcaseDto
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
            });

            return taskDtos;
        }
    }
}
