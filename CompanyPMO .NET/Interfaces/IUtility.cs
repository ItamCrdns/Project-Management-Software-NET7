using CompanyPMO_.NET.Dto;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IEmployeeEntity
    {
        int EmployeeId { get; set; }
    }

    public interface IUtility
    {
        // TEntity = Generic method so it can work (update) different of my database entities.
        // TDto = generic method, will be used to update the entity.
        Task<(bool updated, TDto)> UpdateEntity<TEntity, TDto>(
            int employeeId, // Represents the employee doing the patch request
            int entityId, // Represents the id of the entity we are going to update
            TDto dto,
            List<IFormFile>? images,
            Func<int, List<IFormFile>, Task<(string result, IEnumerable<ImageDto>)>> addImagesMethod,
            Func<int, Task<TEntity?>> findEntityMethod)
            where TEntity : class;

        // TEntity represents the junction table. UEntity represents the entity irself (i.e Project, Task, Issue)
        Task<(string status, IEnumerable<EmployeeDto>)> AddEmployeesToEntity<TEntity, UEntity>(
            List<int> employeeIds, // List of employees to add
            // Takes two integers as arguments one for the entityId and the other one for the employeeId
            string entityName, // Entity name used to identify whether its a task, project or issue
            int entityId,
            Func<int, int, Task<bool>> isEmployeeAlreadyInEntity) // Check if the employee already exists in the entity (i.e employee its already working on a project)
            where TEntity : class, new()
            where UEntity : class;

        int MinutesUntilTimeArrival(DateTimeOffset? time); // Relative time function to get how many minutes are there until a certain date

        Task<(IEnumerable<int> entityIds, int totalEntitiesCount, int totalPages)> GetEntitiesByEmployeeUsername<TEntity>(
            string username,
            string entityName, // Used to identify whatever the entity its a task, project or issue. We will pass a string "EntityId, TaskId or ProjectId" for example
            int? page,
            int? pageSize)
            where TEntity : class, IEmployeeEntity;

        // Very similar to the method above. But I wanted to have a different logic when trying to retrieve by an entity Id
        // Example: Retrieve all tasks based on the projectId, or retrieve all the issues based on the taskId.
        // * They can both use the same generic method because they are very similar
        Task<(IEnumerable<int> entityIds, int totalEntitiesCount, int totalPages)> GetEntitiesByEntityId<TEntity>(
            int entityId, // The entityId that we are looking for. For example: we want to find tasks by projectId 4, entityId will refer to projectId
            string entityName, // The name of the entity we are looking for. For example: we want to find tasks by projectId 4, entityName will refer to "ProjectId",
            string primaryKeyName, // Refers to the name of the primary key of the table we will be looking for. For example: TaskId, ProjectId, IssueId
            int? page,
            int? pageSize
            )
            where TEntity : class;
    }
}
