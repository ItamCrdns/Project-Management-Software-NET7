using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Repository
{
    public class PatcherRepository : IPatcher
    {
        private readonly ApplicationDbContext _context;

        public PatcherRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<(bool updated, TEntity)> UpdateEntity<TEntity, TDto>(int entityId, TDto dto, List<IFormFile>? images, Func<int, List<IFormFile>, Task<IEnumerable<Image>>> addImagesMethod, Func<int, Task<TEntity?>> findEntityMethod) where TEntity : class
        {
            var entityToUpdate = await findEntityMethod(entityId);

            if (entityToUpdate is not null)
            {
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
                _ = await _context.SaveChangesAsync();

                return (true, entityToUpdate);
            }
            else
            {
                return (false, entityToUpdate);
            }
        }
    }
}
