using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
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
        public async Task<(bool updated, TEntity)> UpdateEntity<TEntity, TDto>(int employeeId, int entityId, TDto dto, List<IFormFile>? images, Func<int, List<IFormFile>, Task<IEnumerable<Image>>> addImagesMethod, Func<int, Task<TEntity?>> findEntityMethod) where TEntity : class
        {
            var entityToUpdate = await findEntityMethod(entityId);

            if (entityToUpdate is not null)
            {
                // Convert the entity to JSON to store it 
                var oldEntityToJson = JsonSerializer.Serialize(entityToUpdate);
                var entityName = entityToUpdate.GetType().Name;

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

                    var eachProp = entityToUpdate.GetType().GetProperty(prop.Name);

                    var newValue = prop.GetValue(dto);

                    if (newValue is not null)
                    {
                        eachProp.SetValue(entityToUpdate, newValue);
                    }
                }

                List<Image> imageCollection = new();

                if (images is not null && images.Any(i => i.Length > 0))
                {
                    var uploadedImages = await addImagesMethod(entityId, images);

                    imageCollection.AddRange(uploadedImages);
                }

                _context.Update(entityToUpdate);
                int rowsAffected = await _context.SaveChangesAsync();

                if(rowsAffected > 0)
                {
                    var newEntityToJson = JsonSerializer.Serialize(entityToUpdate);

                    // Add a validation so we dont add a changelog if entity did not update
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

                    return (true, entityToUpdate);
                }
                else
                {
                    return (false, entityToUpdate);
                }
            }
            else
            {
                return (false, entityToUpdate);
            }
        }
    }
}
