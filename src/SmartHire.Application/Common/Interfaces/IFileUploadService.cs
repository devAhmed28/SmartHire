using Microsoft.AspNetCore.Http;

namespace SmartHire.Application.Common.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder);

        Task DeleteFileAsync(string publicId);
    }
}
