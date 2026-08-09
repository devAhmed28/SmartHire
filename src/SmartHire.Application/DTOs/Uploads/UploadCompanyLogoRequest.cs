using Microsoft.AspNetCore.Http;

namespace SmartHire.Application.DTOs.Uploads
{
    public class UploadCompanyLogoRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}
