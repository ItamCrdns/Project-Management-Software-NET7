using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Services
{
    public class ImageService : IImage
    {
        private readonly ApplicationDbContext _context;
        private readonly Cloudinary _cloudinary;
        public ImageService(ApplicationDbContext context, Cloudinary cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }
        public async Task<(string status, IEnumerable<ImageDto>)> AddImagesToExistingEntity(int entity, List<IFormFile>? images, string entityType, int? imagesInEntity)
        {
            if (images is not null && images.Count > 0)
            {
                // If more than 10 images in the entity the count of this collection will be 0
                var imageCollection = await AddImagesToNewEntity(images, entity, entityType, imagesInEntity);

                var imageCollectionToReturn = imageCollection.Select(i => new ImageDto
                {
                    ImageId = i.ImageId,
                    EntityType = i.EntityType,
                    EntityId = i.EntityId,
                    ImageUrl = i.ImageUrl,
                    PublicId = i.PublicId,
                    Created = i.Created
                });

                if (imageCollection.Any())
                {
                    string message = imageCollection.Count + " images added.";
                    return (message, imageCollectionToReturn);
                }
                else
                {
                    string message = "You cannot add more images to this collection. There are already " + imagesInEntity + " images stored.";
                    return (message, imageCollectionToReturn);
                }
            }
            return (null, null);
        }

        public async Task<List<Image>> AddImagesToNewEntity(List<IFormFile> images, int entityId, string entityType, int? imagesInEntity)
        {
            List<Image> imageCollection = new();

            // Check how many images the entity has. If null it means a new entity so set it to zero
            int imageCount = imagesInEntity ?? 0;

            foreach (var image in images)
            {
                if (imageCount >= 10)
                {
                    break;
                }

                var (imageUrl, publicId) = await UploadToCloudinary(image, 0, 0);

                var newImage = new Image
                {
                    EntityType = entityType,
                    EntityId = entityId,
                    ImageUrl = imageUrl,
                    PublicId = publicId,
                    Created = DateTime.Now
                };

                imageCount++; // Add one image for every iteration
                _context.Images.Add(newImage);
                imageCollection.Add(newImage);
            }
            // If more than 10 images provided images will be truncated but we are handling that in the controller (return a 400 statuscode if more than 10 images are provided)
            _ = await _context.SaveChangesAsync();

            return imageCollection
                .Select(i => new Image
                {
                    ImageId = i.ImageId,
                    EntityType = i.EntityType,
                    ImageUrl = i.ImageUrl,
                    PublicId = i.PublicId,
                    Created = i.Created
                }).ToList();
        }

        public async Task<(string imageUrl, string publicId)> UploadToCloudinary(IFormFile file, int width, int height)
        {
            if (file is not null && file.Length > 0)
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
