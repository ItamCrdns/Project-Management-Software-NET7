using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
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
