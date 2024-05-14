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

        public async Task<(string status, IEnumerable<EmployeeShowcaseDto>)> AddEmployeesToEntity<TEntity, UEntity>(List<int> employeeIds, string entityName, int entityId, Func<int, int, Task<bool>> isEmployeeAlreadyInEntity) where TEntity : class, new() where UEntity : class
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
                    if (prop.Name.Equals("EmployeeId"))
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

            IEnumerable<EmployeeShowcaseDto> employeesAdded = await _context.Employees
                .Where(e => addedEmployees.Select(e => e.employeeId).Contains(e.EmployeeId))
                .Select(e => new EmployeeShowcaseDto
                {
                    EmployeeId = e.EmployeeId,
                    Username = e.Username,
                    ProfilePicture = e.ProfilePicture
                }).ToListAsync();

            // Check whether all employees were added, some or none and return an appropiate response
            if (rowsAffected.Equals(employeeIds.Count))
            {
                string response = "All employees were added successfully";
                return (response, employeesAdded);
            }
            else if (rowsAffected > 0)
            {
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

        public (Expression<Func<T, bool>>, Expression<Func<T, object>>?) BuildWhereAndOrderByExpressions<T>(int? constantId, IEnumerable<int>? whereIds, string? whereId, string defaultWhere, string defaultOrderBy, FilterParams filterParams)
        {
            // This method will build and return two expressions that can be used by LINQ "Where" and "OrderBy" or "OrderByDescending" methods
            var parameter = Expression.Parameter(typeof(T), "x");

            // * If constantId is provided, build the first expression. (x => x.EntityId EQUALS constantId)
            BinaryExpression equals = Expression.Equal(Expression.Constant(true), Expression.Constant(true));

            if (constantId is not null && constantId > 0)
            {
                //MemberExpression newFilterString = null;
                MemberExpression newFilterString = FilterStringSplitter(parameter, defaultWhere);
                var constant = Expression.Constant(constantId);

                if (newFilterString.Type == typeof(int?))
                {
                    // If nullable int convert to int to avoid error, however thisll trow exception if the integer is null, handle with care
                    var notNullableFilterString = Expression.Convert(newFilterString, typeof(int));

                    equals = Expression.Equal(notNullableFilterString, constant);
                }
                else
                {
                    equals = Expression.Equal(newFilterString, constant);
                }
            }

            // * If filterParams are filterValue are provided, build the second expression. (x => x.FilterBy EQUALS filterValue)
            if (filterParams.FilterBy is not null && filterParams.FilterValue is not null)
            {
                // We will handle multiple filterBy properties by separting them with an underscore. Example: "FilterBy": "Priority_Author"
                //// If the string contains underscore, split it, and create a new expression for each property. (x => x.Priority EQUALS constantId && x => x.Author EQUALS constantId)
                // Generate arrays for each properties. They should have the same number of elements, if they are different: a wrong query param were provided
                // If they are different, just do a fallback and return the default expressions. No error its necessary to be returned.
                // newFilterBys[0] will be the first property, newFilterValues[0] will be the first value, and so on. And they are equal to each other (i.e newFilterBys[0] EQUALS newFilterValues[0])
                string[] newFilterBys = filterParams.FilterBy.Split('_');
                string[] newFilterValues = filterParams.FilterValue.Split('_');

                if (newFilterBys.Length == newFilterValues.Length)
                {
                    var pairs = newFilterBys.Zip(newFilterValues, (fBy, fValue) => new { fBy, fValue }).ToArray();

                    foreach (var pair in pairs)
                    {
                        MemberExpression property = Expression.Property(parameter, pair.fBy);
                        Type propertyType = property.Type;

                        if (pair.fValue.Contains('-'))
                        {
                            // If the filterValue contains a '-', that means we need to convert the string (i.e "1-2-3") to a list of ints (i.e [1, 2, 3]) (list and not array because we need the .Contains method)
                            List<int> valueInts = pair.fValue.Split('-').Select(int.Parse).ToList();
                            ConstantExpression valueIntsConstant = Expression.Constant(valueInts);
                            MethodInfo containsMethod = typeof(List<int>).GetMethod("Contains");

                            // Checks if x.Whatever is in the list of ints (x => valueInts.Contains(x.Whatever))
                            Expression containsCall = Expression.Call(valueIntsConstant, containsMethod, property);
                            equals = Expression.AndAlso(equals, containsCall);
                        }
                        else
                        {
                            // If it does not contain a '-', we will just compare the property with the value (x => x.Whatever EQUALS value)
                            if (pair.fValue == "NoValue")
                            {
                                // handle fValue will be null. This will be usefull when we want to do for example x.Finished == null to get unfinished entities
                                ConstantExpression nullish = Expression.Constant(null, propertyType);
                                BinaryExpression equalsNull = Expression.Equal(property, nullish);
                                equals = Expression.AndAlso(equals, equalsNull);
                            }
                            else if (pair.fValue == "NotNull")
                            {
                                // Handle fValue not equal to null
                                ConstantExpression notNull = Expression.Constant(null, propertyType);
                                BinaryExpression notEqualsNull = Expression.NotEqual(property, notNull);
                                equals = Expression.AndAlso(equals, notEqualsNull);
                            }
                            else if (pair.fValue == "RightNowDate")
                            {
                                // Handle fValue will be the current date
                                ConstantExpression notNull = Expression.Constant(null, propertyType);
                                BinaryExpression notEqualsNull = Expression.NotEqual(property, notNull);

                                // Build the following expression, for overdue entities: x => x.Finished != null && x.Finished < DateTime.UtcNow
                                equals = Expression.AndAlso(equals, notEqualsNull);

                                DateTime? rNow = DateTime.UtcNow;
                                ConstantExpression constant = Expression.Constant(rNow, propertyType);
                                BinaryExpression scopedEquals = Expression.LessThan(property, constant);

                                equals = Expression.AndAlso(equals, scopedEquals);
                            }
                            else if (pair.fValue == "Ongoing")
                            {
                                // Handle fValue will be the current date
                                ConstantExpression notNull = Expression.Constant(null, propertyType);
                                BinaryExpression notEqualsNull = Expression.NotEqual(property, notNull);

                                // Build the following expression, for overdue entities: x => x.Finished != null && x.Finished < DateTime.UtcNow
                                equals = Expression.AndAlso(equals, notEqualsNull);

                                DateTime? rNow = DateTime.UtcNow;
                                ConstantExpression constant = Expression.Constant(rNow, propertyType);
                                BinaryExpression scopedEquals = Expression.GreaterThan(property, constant);

                                equals = Expression.AndAlso(equals, scopedEquals);
                            }
                            else
                            {
                                // handle fValue will be a value
                                object convertedValue = Convert.ChangeType(pair.fValue, propertyType);
                                ConstantExpression constant = Expression.Constant(convertedValue, propertyType);
                                BinaryExpression scopedEquals = Expression.Equal(property, constant);
                                equals = Expression.AndAlso(equals, scopedEquals);
                            }
                        }
                    }
                }
                else
                {
                    MemberExpression newFilterString = FilterStringSplitter(parameter, filterParams.FilterBy);
                    var otherConstant = Expression.Constant(Convert.ToInt32(filterParams.FilterValue));
                    var scopedEquals = Expression.Equal(newFilterString, otherConstant);
                    equals = Expression.AndAlso(equals, scopedEquals);
                }
            }

            // If whereIds its not null, we are going to create a new expression to filter by the given ids. Example: Get all the tasks that belong to the projectIds 1, 2 and 3
            // This is so it can work together with the GetEntitiesByEntityId method. (x => x.TaskIds.Contains("TaskId")
            if (whereIds is not null)
            {
                ConstantExpression whereIdsConstant = Expression.Constant(whereIds);
                Expression whereIdsProperty = Expression.Property(parameter, whereId);
                MethodInfo containsMethod = typeof(List<int>).GetMethod("Contains"); // Retrieving the Contains method from the List<int> class

                Expression containsCall = Expression.Call(whereIdsConstant, containsMethod, whereIdsProperty); // Check if the whereIds list contains the whereId property

                // Updates equals by combining the previous equals with the new containsCall expression using
                equals = Expression.AndAlso(equals, containsCall);
            }

            if (!string.IsNullOrWhiteSpace(filterParams.SearchBy) && !string.IsNullOrWhiteSpace(filterParams.SearchValue))
            {
                // We will handle if the user searches for a certain entity here. Example: Get all the tasks that have the word "Design" in the title
                MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                ConstantExpression searchConstant = Expression.Constant(filterParams.SearchValue.ToLower());

                // Convert the search property to lower
                MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                MemberExpression searchProperty = Expression.Property(parameter, filterParams.SearchBy);
                Expression searchPropertyToLower = Expression.Call(searchProperty, toLowerMethod); // Convert the values to lower case, ex: x.Name.ToLower() all names will be lower case, this disables 

                // This creates an expression to search case-insensitive x.Name.ToLower().Contains("text")
                Expression searchContainsCall = Expression.Call(searchPropertyToLower, containsMethod, searchConstant);

                equals = Expression.AndAlso(equals, searchContainsCall);
            }

            var whereExpression = Expression.Lambda<Func<T, bool>>(equals, parameter);

            MemberExpression newOrderString = FilterStringSplitter(parameter, filterParams.OrderBy ?? defaultOrderBy);
            UnaryExpression orderConvert = Expression.Convert(newOrderString, typeof(object));
            var orderByExpression = Expression.Lambda<Func<T, object>>(orderConvert, parameter);

            return (whereExpression, orderByExpression);
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
                    if (prop.PropertyType.Equals(typeof(int))) // Check if the value its integer
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
                        Modified = DateTime.UtcNow,
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
