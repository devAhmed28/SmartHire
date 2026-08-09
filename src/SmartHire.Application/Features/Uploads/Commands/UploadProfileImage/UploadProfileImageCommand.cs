using MediatR;
using Microsoft.AspNetCore.Http;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Uploads;

namespace SmartHire.Application.Features.Uploads.Commands.UploadProfileImage
{
    public class UploadProfileImageCommand : IRequest<Result<UploadFileResponse>>
    {
        public Guid UserId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
