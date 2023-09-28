using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IPatcher
    {
        // TEntity = Generic method so it can work (update) different of my database entities.
        // TDto = generic method, will be used to update the entity.
        Task<(bool updated, TDto)> UpdateEntity<TEntity, TDto>(
            int employeeId, // Represents the employee doing the patch request
            int entityId, // Represents the id of the entity we are going to update
            TDto dto,
            List<IFormFile>? images,
            Func<int, List<IFormFile>, Task<IEnumerable<ImageDto>>> addImagesMethod,
            Func<int, Task<TEntity?>> findEntityMethod)
            where TEntity : class;
    }
}
