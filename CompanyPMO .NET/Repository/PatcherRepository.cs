using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CompanyPMO_.NET.Repository
{
    public class PatcherRepository : IPatcher
    {
        private readonly ApplicationDbContext _context;

        public PatcherRepository(ApplicationDbContext context)
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
    }
}
