using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHire.Application.DTOs.Uploads
{
    public class UploadProfileImageRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}
