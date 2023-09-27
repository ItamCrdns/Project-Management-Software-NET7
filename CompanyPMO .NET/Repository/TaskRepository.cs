using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyPMO_.NET.Repository
{
    public class TaskRepository : ITask
    {
        private readonly ApplicationDbContext _context;
        private readonly IImage _imageService;

        public TaskRepository(ApplicationDbContext context, IImage imageService)
        {
            _context = context;
            _imageService = imageService;
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
                imageCollection = await _imageService.AddImagesToNewEntity(images, newTask.TaskId, "Task");
            }

            return (newTask, imageCollection);
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

        public async Task<List<Models.Task>> GetTasksByProjectId(int projectId)
        {
            var tasks = await _context.Tasks
                .Where(p => p.ProjectId.Equals(projectId))
                .Include(i => i.Images)
                .Include(e => e.Employees)
                .ToListAsync();

            foreach(var task in tasks)
            {
                var tasksImages = task.Images
                    .Where(et => et.EntityType.Equals("Task")) // Client side filtering
                    .Select(i => new Image
                {
                    ImageId = i.ImageId,
                    EntityType = i.EntityType,
                    EntityId = i.EntityId,
                    ImageUrl = i.ImageUrl,
                    PublicId = i.PublicId
                }).ToList();

                var tasksEmployes = task.Employees
                    .Select(e => new Employee
                {
                    EmployeeId = e.EmployeeId,
                    Username = e.Username,
                    Role = e.Role,
                    ProfilePicture = e.ProfilePicture,
                    FirstName = e.FirstName,
                    LastName = e.LastName
                }).ToList();

                task.Employees = tasksEmployes;
                task.Images = tasksImages;
            }

            return tasks;
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
    }
}
