using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Infrastructure.Common.Settings;

namespace SmartHire.Infrastructure.Services
{
    public class CloudinaryService : IFileUploadService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var account = new Account(
                options.Value.CloudName,
                options.Value.ApiKey,
                options.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }
        public async Task<string> UploadFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null");
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception($"Cloudinary upload failed: {uploadResult.Error.Message}");
            }

            return uploadResult.SecureUrl.ToString();
        }

        public async Task DeleteFileAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);

            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            if (deletionResult.Error != null)
            {
                throw new Exception($"Cloudinary deletion failed: {deletionResult.Error.Message}");
            }
        }
    }
}
