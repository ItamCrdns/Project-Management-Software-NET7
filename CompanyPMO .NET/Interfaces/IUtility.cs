using CompanyPMO_.NET.Models;
using System.Linq.Expressions;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IEmployeeEntity
    {
        int EmployeeId { get; set; }
    }

    public interface IUtility
    {
        int MinutesUntilTimeArrival(DateTimeOffset? time); // Relative time function to get how many minutes are there until a certain date

        Task<(IEnumerable<int> entityIds, int totalEntitiesCount, int totalPages)> GetEntitiesByEmployeeUsername<TEntity>(
            string username,
            string entityName, // Used to identify whatever the entity its a task, project or issue. We will pass a string "EntityId, TaskId or ProjectId" for example
            int? page,
            int? pageSize)
            where TEntity : class, IEmployeeEntity;

        Task<(IEnumerable<int> entityIds, int totalEntitiesCount, int totalPages)> GetEntitiesEmployeeCreatedOrParticipates<TEntity, UEntity>(
            string username,
            string entityNameForEntityCreatorId, // EntityId of the creator id. Example: EmployeeProject table, entityNameForEntityCreatorId will be "ProjectCreatorId"
            string entityIdToSelect, // EntityId to select the data from. Example: Project table, entityIdToSelect will be "ProjectId". This will select all of the entityIds and will return them
            int? page,
            int? pageSize)
            where TEntity : class, IEmployeeEntity // TEntity will be used to represent the junction table
            where UEntity : class; // UEntity will be used to represent the entity itself (i.e Project, Task, Issue)

        Task<(ICollection<U> entity, int totalEntitiesCount, int totalPages)> GetAllEntities<T, U>(
            FilterParams filterParams,
            Expression<Func<T, U>> predicate) // Predicate to select the data from the entity. Example: x => x.Project == project
            where T : class
            where U : class;


        // Splits the given filter from 'FilterExample' to 'Filter.Example' for linq querying purposes
        MemberExpression FilterStringSplitter(ParameterExpression parameter, string filterString);

        // * Build Where and orderBy expressions during runtime
        (Expression<Func<T, bool>>, Expression<Func<T, object>>?) BuildWhereAndOrderByExpressions<T>(
            int? constantId,
            string? constantString, // Do not pass both constantId and constantStringIncludes at the same time
                                    // ! Just pass null, null if you dont want to use the whereIds and whereId parameters. This will disable the extra .Where (x => x.whereIds.Contains(x.whereId) expression
                                    // TODO: Might deprecate whereIds
            //IEnumerable<int>? whereIds, // * Pass a list of ids here: example: new List<int> { 1, 2, 3 }. This will be used to filter the data and will only return the data that matches the given ids
            //string? whereId, // * Used to build the where expression along with the whereIds list. Example: x => whereIds.Contains(x.whereId)
            string defaultWhere, // * Pass it here: example:  filterParams.FilterWhere ?? "CompanyId"
            string defaultOrderBy, // * Pass it here: example: filterParams.OrderBy ?? "Created");
            FilterParams filterParams
            );
    }
}
