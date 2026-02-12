using CloudinaryDotNet.Actions;

namespace API.Interface
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadIamge(IFormFile file);
        Task<DeletionResult> DeleteImage(string publicId);
    }
}
