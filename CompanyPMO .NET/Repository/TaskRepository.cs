using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;

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
                TaskId = task.TaskId,
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
                imageCollection = await _imageService.AddImagesToEntity(images, newTask.TaskId, "Task");
            }

            return (newTask, imageCollection);
        }
    }
}
