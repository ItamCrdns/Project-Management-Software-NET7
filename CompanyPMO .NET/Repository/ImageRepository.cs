using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Repository
{
    public class ImageRepository : IImage
    {
        private readonly ApplicationDbContext _context;
        private readonly Cloudinary _cloudinary;

        public ImageRepository(ApplicationDbContext context, Cloudinary cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        public async Task<IEnumerable<ImageDto>> AddImagesToExistingEntity(int entityId, List<IFormFile>? images, string entityType)
        {
            if (images is not null && images.Any(i => i.Length > 0))
            {
                List<Image> imageCollection = await AddImagesToNewEntity(images, entityId, entityType);

                return imageCollection.Select(i => new ImageDto
                {
                    ImageId = i.ImageId,
                    EntityType = i.EntityType,
                    EntityId = i.EntityId,
                    ImageUrl = i.ImageUrl,
                    PublicId = i.PublicId,
                    Created = i.Created
                });
            }
            return null;
        }

        public async Task<List<Image>> AddImagesToNewEntity(ICollection<IFormFile> images, int entityId, string entityType)
        {
            List<Image> imageCollection = new();
            foreach (var image in images)
            {
                var (imageUrl, publicId) = await UploadToCloudinary(image, 0, 0);

                var newImage = new Image
                {
                    EntityType = entityType,
                    EntityId = entityId,
                    ImageUrl = imageUrl,
                    PublicId = publicId,
                    Created = DateTimeOffset.UtcNow
                };

                _context.Add(newImage);
                imageCollection.Add(newImage);
                _ = await _context.SaveChangesAsync();
            }

            return imageCollection.Select(i => new Image
            {
                ImageId = i.ImageId,
                EntityType = i.EntityType,
                ImageUrl = i.ImageUrl,
                PublicId = i.PublicId
            }).ToList();
        }

        public async Task<(string imageUrl, string publicId)> UploadToCloudinary(IFormFile file, int width, int height)
        {
            if(file is not null && file.Length > 0)
            {
                await using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = true,
                    Transformation = new Transformation().Quality(50).FetchFormat("webp")
                };

                // If width and height arguements are given crop the image accordingly
                if (width > 0 && height > 0)
                {
                    uploadParams.Transformation = new Transformation().Width(width).Height(height).Crop("fill");
                }

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error is not null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                return (uploadResult.SecureUrl.ToString(), uploadResult.PublicId);
            }
            return (null, null);
        }
    }
}
