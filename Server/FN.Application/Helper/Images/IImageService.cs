using Microsoft.AspNetCore.Http;

namespace FN.Application.Helper.Images
{
    public interface IImageService
    {
        string GenerateId();
        Task<string?> UploadImage(IFormFile file, string publicId, string folderName);
        Task<List<string>> UploadImages(List<IFormFile> files, string folderName);
        Task<bool> DeleteImage(string publicId);
        Task<bool> DeleteFolderImage(string folderName);
    }
}
