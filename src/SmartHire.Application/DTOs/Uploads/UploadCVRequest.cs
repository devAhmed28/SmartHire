using Microsoft.AspNetCore.Http;

namespace SmartHire.Application.DTOs.Uploads
{
    public class UploadCVRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}
