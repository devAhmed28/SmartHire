using MediatR;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Uploads.Commands.DeleteFile
{
    public class DeleteFileCommand : IRequest<Result>
    {
        public string PublicId { get; set; } = string.Empty;
    }
}
