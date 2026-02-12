using API.Interface;
using API.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> options)
        {
            var account = new Account
            {
                ApiKey = options.Value.ApiKey,
                ApiSecret = options.Value.ApiSecret,
                Cloud = options.Value.CloudName
            };

            _cloudinary = new Cloudinary(account);
        }

        public async Task<DeletionResult> DeleteImage(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deletionParams);
        }

        public async Task<ImageUploadResult> UploadIamge(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "DatingApp"    
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;

        }
    }
}
