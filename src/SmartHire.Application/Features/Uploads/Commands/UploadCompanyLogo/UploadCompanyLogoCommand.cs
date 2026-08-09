using MediatR;
using Microsoft.AspNetCore.Http;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Uploads;

namespace SmartHire.Application.Features.Uploads.Commands.UploadCompanyLogo
{
    public class UploadCompanyLogoCommand : IRequest<Result<UploadFileResponse>>
    {
        public Guid CompanyId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
