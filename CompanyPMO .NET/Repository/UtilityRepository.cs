using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace CompanyPMO_.NET.Repository
{
    public class UtilityRepository : IUtility
    {
        private readonly ApplicationDbContext _context;

        public UtilityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(string status, IEnumerable<EmployeeDto>)> AddEmployeesToEntity<TEntity, UEntity>(List<int> employeeIds, string entityName, int entityId, Func<int, int, Task<bool>> isEmployeeAlreadyInEntity) where TEntity : class, new() where UEntity : class
        {
            // Check if entity exists (i.e "api/Project/3/add/employees" will check if Project with ID #3 existts)
            bool entityExists = await _context.Set<UEntity>().FindAsync(entityId) is not null;

            if (!entityExists)
            {
                return ("Entity does not exist", null);
            }

            if (employeeIds.Count is 0)
            { 
                return ("No employees were provided.", null);
            }

            List<TEntity> employeesToAdd = new(); // This is used to update the entity in the database
            List<(int employeeId, int entityId)> addedEmployees = new(); // This list will be returned at the end of the method

            foreach (var employeeId in employeeIds)
            {
                bool isEmployeeIn = await isEmployeeAlreadyInEntity(employeeId, entityId);
                if (isEmployeeIn)
                {
                    continue; // Skip iteration and do not add the employee because he is already in the entity
                }

                var newEntity = new TEntity();

                var props = typeof(TEntity).GetProperties();
                foreach (var prop in props)
                {
                    if (prop.Name.Equals("RelationId")) continue; // Primary key. Skip iteration
                    if(prop.Name.Equals("EmployeeId"))
                    {
                        prop.SetValue(newEntity, employeeId);
                        addedEmployees.Add((employeeId, entityId));
                    }
                    else if (prop.Name.Equals(entityName))
                    {
                        prop.SetValue(newEntity, entityId);
                    }
                }

                employeesToAdd.Add(newEntity);
            }

            _context.Set<TEntity>().AddRange(employeesToAdd);

            int rowsAffected = await _context.SaveChangesAsync();

            IEnumerable<EmployeeDto> employeesAdded = await _context.Employees
                .Where(e => addedEmployees.Select(e => e.employeeId).Contains(e.EmployeeId))
                .Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    Username = e.Username,
                    ProfilePicture = e.ProfilePicture,
                    Role = e.Role,
                }).ToListAsync();

            // Check whether all employees were added, some or none and return an appropiate response
            if (rowsAffected.Equals(employeeIds.Count))
            {
                string response = "All employees were added successfully";
                return (response, employeesAdded);
            }
            else if (rowsAffected > 0) {
                string response = "Operation was completed. However, not all employees could be added. Are you trying to add employees that are already working in this project?";
                return (response, employeesAdded);
            }
            else
            {
                string response = "No employees were added. Are you trying to add employees that are already working in this project?";
                return (response, null);
            }
        }

        public MemberExpression FilterStringSplitter(ParameterExpression parameter, string filterString)
        {
            // * FilterBy and OrderBy
            // * Returns a property: 
            // Creating expression for a single property if the filter string does not have a dot (Ex: {x.Priority})
            // If the filter string has a dot, we will split it and create a nested expression for each property (Ex: {x.TaskCreator.EmployeeId})

            string lowerCaseFilterString = filterString.ToLower();

            if (lowerCaseFilterString.Equals("employees"))
            {
                filterString = "Employees.Count";
            }

            if (lowerCaseFilterString.Equals("projectcreator"))
            {
                filterString = "ProjectCreator.employeeId";
            }

            if (lowerCaseFilterString.Equals("company"))
            {
                filterString = "Company.companyId";
            }

            if (lowerCaseFilterString.Equals("issuecreator"))
            {
                filterString = "IssueCreator.employeeId";
            }

            if (lowerCaseFilterString.Equals("task"))
            {
                filterString = "Task.taskId";
            }

            if (lowerCaseFilterString.Equals("taskcreator"))
            {
                filterString = "TaskCreator.employeeId";
            }

            if (lowerCaseFilterString.Equals("project"))
            {
                filterString = "Project.projectId";
            }

            if (filterString.Contains('.'))
            {
                string[] parts = filterString.Split(".");
                MemberExpression splitPropertyExpression = Expression.Property(parameter, parts[0]);
                MemberExpression splitPropertyExpression2 = Expression.Property(splitPropertyExpression, parts[1]);

                return splitPropertyExpression2;
            }
            else
            {
                MemberExpression property = Expression.Property(parameter, filterString);
                return property;
            }
        }

        public async Task<(ICollection<T> entity, int totalEntitiesCount, int totalPages)> GetAllEntities<T>(FilterParams filterParams, List<string> navigationProperties = null) where T : class
        {
            var filterProperty = typeof(T).GetProperty(filterParams.OrderBy ?? "Created", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            // * If ascending or descending orders are provided in the query params, we will use them
            bool ShallOrderAscending = filterParams.Sort is not null && filterParams.Sort.Equals("ascending");
            bool ShallOrderDescending = filterParams.Sort is not null && filterParams.Sort.Equals("descending");

            bool filterExists = filterProperty is not null;

            if (!filterExists)
            {
                var entity = new Collection<T>();
                return (entity, 0, 0);
            }

            int toSkip = (filterParams.Page - 1) * filterParams.PageSize;

            var parameter = Expression.Parameter(typeof(T), "x");

            // Initialize the empty WHERE and ORDERBY expression
            var whereExpression = Expression.Lambda<Func<T, bool>>(Expression.Constant(true), Expression.Parameter(typeof(T)));
            var orderExpression = Expression.Lambda<Func<T, object>>(Expression.Constant(null, typeof(object)), Expression.Parameter(typeof(T)));

            if (filterParams.FilterBy is not null && filterParams.FilterValue is not null)
            {
                MemberExpression newFilterString = FilterStringSplitter(parameter, filterParams.FilterBy);
                UnaryExpression convertedWhereProperty = Expression.Convert(newFilterString, typeof(object));
                BinaryExpression whereEquals = Expression.Equal(convertedWhereProperty, Expression.Constant(filterParams.FilterValue));
                whereExpression = Expression.Lambda<Func<T, bool>>(whereEquals, parameter);
            }
            else
            {
                // Fallback if no filterBy and filterValue query params are provided
                whereExpression = p => true; // Will have no effect
            }

            if (filterParams.OrderBy is not null)
            {
                MemberExpression newFilterString = FilterStringSplitter(parameter, filterParams.OrderBy);
                UnaryExpression convertedOrderByProperty = Expression.Convert(newFilterString, typeof(object));
                orderExpression = Expression.Lambda<Func<T, object>>(convertedOrderByProperty, parameter);
            }

            ICollection<T> entities = new List<T>();

            IQueryable<T> query = _context.Set<T>();

            if (navigationProperties is not null)
            {
                foreach (var navProperty in navigationProperties)
                {
                    query = query.Include(navProperty);
                }
            }
            
            if (ShallOrderAscending)
            {
                entities = await query
                    .OrderBy(orderExpression)
                    .Where(whereExpression)
                    .Skip(toSkip)
                    .Take(filterParams.PageSize)
                    .ToListAsync();
            }
            else if (ShallOrderDescending || (!ShallOrderAscending && !ShallOrderDescending))
            {
                entities = await query
                    .OrderByDescending(orderExpression)
                    .Where(whereExpression)
                    .Skip(toSkip)
                    .Take(filterParams.PageSize)
                    .ToListAsync();
            }

            int totalEntitiesCount = await query
                .Where(whereExpression)
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalEntitiesCount / filterParams.PageSize);

            return (entities, totalEntitiesCount, totalPages);
        }

        public async Task<(IEnumerable<int> entityIds, int totalEntitiesCount, int totalPages)> GetEntitiesByEmployeeUsername<TEntity>(string username, string entityName, int? page, int? pageSize) where TEntity : class, IEmployeeEntity // TEntity is constrained with IEmployeeEntity
        {
            // Generic method to return a list of entities that the employee belongs to, its count and the total pages.
            // Result comes paginated.

            // Get the employeeId from the username. Why? Junction tables store the Id, not the username
            int employeeId = await _context.Employees
                .Where(e => e.Username.Equals(username))
                .Select(e => e.EmployeeId)
                .FirstOrDefaultAsync();

            // Count the total entities that the employee belongs to
            int totalEntitiesCount = await _context.Set<TEntity>()
                .Where(i => i.EmployeeId.Equals(employeeId))
                .CountAsync();

            // If the pageSize is null, return all the entities
            
            int pageValue = page ?? 1;
            int pageValueSize = pageSize ?? totalEntitiesCount;

            int totalPages = (int)Math.Ceiling((double)totalEntitiesCount / pageValueSize);

            int toSkip = (pageValue - 1) * pageValueSize;
            
            // Use reflection to get the EntityId
            var entityId = typeof(TEntity).GetProperty(entityName);

            // Returns a list of integers of all the entity ids that the employee belongs to
            List<int> entityIds = await _context.Set<TEntity>()
                .Where(i => i.EmployeeId.Equals(employeeId))
                .Select(e => (int)entityId.GetValue(e))
                .Skip(toSkip)
                .Take(pageValueSize)
                .ToListAsync();

            return (entityIds, totalEntitiesCount, totalPages);
        }

        public async Task<(IEnumerable<int> entityIds, int totalEntitiesCount, int totalPages)> GetEntitiesByEntityId<TEntity>(int entityId, string entityName, string primaryKeyName, int? page, int? pageSize) where TEntity : class
        {
            // The name its kind of redundant but its what it says: it will, for example, return tasks based on the projectId

            var entityProperty = typeof(TEntity).GetProperty(entityName);
            var primaryKey = typeof(TEntity).GetProperty(primaryKeyName);

            // Use expression tree to build a predicate value for the where clause
            var parameter = Expression.Parameter(typeof(TEntity), "x");

            // Create the property access expression (x => x.Property)
            var propertyAccess = Expression.Property(parameter, entityProperty);

            // Create the comparasion expression (x => x.Property EQUALS entityId)
            var equals = Expression.Equal(propertyAccess, Expression.Constant(entityId));

            // Create a lambda expression (x => x.Property EQUALS entityId)
            var whereExpression = Expression.Lambda<Func<TEntity, bool>>(equals, parameter);

            // Compile the lambda expression into a delegate
            //var predicate = lambda.Compile(); //+		System.Linq.Expressions.Expression<TDelegate>.Compile devuelto	{Method = <Internal Error evaluating expression>}	System.Func<CompanyPMO_.NET.Models.Task, bool>

            // Count the total entities that the employee belongs to
            var totalEntitiesCount = _context.Set<TEntity>()
                .Where(whereExpression)
                .Select(x => (int)entityProperty.GetValue(x))
                .Count();

            // If the pageSize is null, return all the entities
            int pageValue = page ?? 1;
            int pageValueSize = pageSize ?? totalEntitiesCount;

            int totalPages = (int)Math.Ceiling((double)totalEntitiesCount / pageValueSize);

            int toSkip = (pageValue - 1) * pageValueSize;

            // Get a list of the entity Ids
            List<int> entityIds = await _context.Set<TEntity>()
                .Where(whereExpression)
                .Select(x => (int)primaryKey.GetValue(x)) // Select the primary key of the entity and return a list of those primary keys
                .Skip(toSkip)
                .Take(pageValueSize)
                .ToListAsync();

            return (entityIds, totalEntitiesCount, totalPages);
        }

        public int MinutesUntilTimeArrival(DateTimeOffset? time)
        {
            DateTimeOffset currentTime = DateTimeOffset.Now;

            if (time <= currentTime)
            {
                return 0;
            }

            TimeSpan timeUntilArrival = (time.Value - currentTime);

            double rounded = Math.Round(timeUntilArrival.TotalSeconds / 60);

            return (int)rounded;
        }

        public async Task<(bool updated, TDto)> UpdateEntity<TEntity, TDto>(int employeeId, int entityId, TDto dto, List<IFormFile>? images, Func<int, List<IFormFile>, Task<(string result, IEnumerable<ImageDto>)>> addImagesMethod, Func<int, Task<TEntity?>> findEntityMethod) where TEntity : class
        {
            // This gets the whole entity. Including (at the moment) images, but will include more in the future.
            var entityBeforeBeingUpdated = await findEntityMethod(entityId);

            if (entityBeforeBeingUpdated is not null)
            {
                // Convert the entity to JSON to store it 
                var oldEntityToJson = JsonSerializer.Serialize(entityBeforeBeingUpdated);
                var entityName = entityBeforeBeingUpdated.GetType().Name;

                var type = typeof(TDto);
                var props = type.GetProperties();

                foreach (var prop in props)
                {
                    if(prop.PropertyType.Equals(typeof(int))) // Check if the value its integer
                    {
                        int propValue = (int)prop.GetValue(dto); // Skip iteration if value is zero
                        if (propValue is 0)
                        {
                            continue;
                        }
                    }

                    var eachProp = entityBeforeBeingUpdated.GetType().GetProperty(prop.Name);

                    var newValue = prop.GetValue(dto);

                    if (newValue is not null)
                    {
                        eachProp.SetValue(entityBeforeBeingUpdated, newValue);
                    }
                }

                List<ImageDto> imageCollection = new();
                string status = string.Empty;

                if (images is not null && images.Any(i => i.Length > 0))
                {
                    var (receivedStatus, uploadedImages) = await addImagesMethod(entityId, images);

                    imageCollection.AddRange(uploadedImages);
                    status = receivedStatus;
                }

                var result = new PatchEntityImagesDto
                {
                    Images = imageCollection,
                    Status = status
                };

                _context.Update(entityBeforeBeingUpdated);
                int rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    var imagesProperty = dto.GetType().GetProperty("Images");
                    imagesProperty.SetValue(dto, result);

                    // Load the entity again, now updated with the new values
                    var entityAfterBeingUpdated = await findEntityMethod(entityId);

                    var newEntityToJson = JsonSerializer.Serialize(entityAfterBeingUpdated);

                    var newChangeLog = new Changelog
                    {
                        EntityType = entityName,
                        EntityId = entityId,
                        Operation = "PATCH",
                        EmployeeId = employeeId,
                        Modified = DateTimeOffset.UtcNow,
                        OldData = oldEntityToJson, // Save the old and new data as a plain json 
                        NewData = newEntityToJson
                    };

                    _context.Add(newChangeLog);
                    _ = await _context.SaveChangesAsync();

                    return (true, dto);
                }
                else
                {
                    return (false, dto);
                }
            }
            else
            {
                return (false, dto);
            }
        }

        async Task<(IEnumerable<int> entityIds, int totalEntitiesCount, int totalPages)> IUtility.GetEntitiesEmployeeCreatedOrParticipates<TEntity, UEntity>(string username, string entityNameForEntityCreatorId, string entityIdToSelect, int? page, int? pageSize)
        {
            int employeeId = await _context.Employees
                .Where(e => e.Username.Equals(username))
                .Select(e => e.EmployeeId)
                .FirstOrDefaultAsync();

            int TEntityCount = await _context.Set<TEntity>()
                .Where(i => i.EmployeeId.Equals(employeeId))
                .CountAsync();

            // Build a predicate value for the where clause of UEntity
            var parameter = Expression.Parameter(typeof(UEntity), "x");
            
            var entityProperty = typeof(TEntity).GetProperty("EmployeeId"); // Always employee Id. This generic method its to get employees so we dont need any other property
            var entityPropertyJunctionTable = typeof(UEntity).GetProperty(entityNameForEntityCreatorId);

            var propertyAccess = Expression.Property(parameter, entityPropertyJunctionTable);
            var equals = Expression.Equal(propertyAccess, Expression.Constant(employeeId));

            var whereExpression = Expression.Lambda<Func<UEntity, bool>>(equals, parameter);

            int UEntityCount = await _context.Set<UEntity>()
                .Where(whereExpression)
                .CountAsync();

            int totalEntitiesCount = TEntityCount + UEntityCount;

            // Default values for the page and pageSize in case they are null
            int pageValue = page ?? 1;
            int pageValueSize = pageSize ?? totalEntitiesCount;

            int totalPages = (int)Math.Ceiling((double)totalEntitiesCount / pageValueSize);

            int toSkip = (pageValue - 1) * pageValueSize;

            List<int> entityIds = new();

            var entityIdU = typeof(UEntity).GetProperty(entityIdToSelect);
            var entityIdT = typeof(TEntity).GetProperty(entityIdToSelect);

            List<int> TEntityIds = await _context.Set<UEntity>()
                .Where(whereExpression)
                .Select(e => (int)entityIdU.GetValue(e))
                .ToListAsync();

            entityIds.AddRange(TEntityIds);

            List<int> UEntityIds = await _context.Set<TEntity>()
                .Where(i => i.EmployeeId.Equals(employeeId))
                .Select(e => (int)entityIdT.GetValue(e))
                .ToListAsync();

            entityIds.AddRange(UEntityIds);

            // Select, join and paginate the entities. And return them so we can use them as we like in different endpoints
            entityIds = entityIds.Skip(toSkip).Take(pageValueSize).ToList();

            return (entityIds, totalEntitiesCount, totalPages);
        }
    }
}
