using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces
{
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
            string entityName, // Entity name used to identify whether its a trask, project or issue
            int entityId,
            Func<int, int, Task<bool>> isEmployeeAlreadyInEntity) // Check if the employee already exists in the entity (i.e employee its already working on a project)
            where TEntity : class, new()
            where UEntity : class;

        int MinutesUntilTimeArrival(DateTimeOffset? time); // Relative time function to get how many minutes are there until a certain date
    }
}
