using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Uploads.Commands.DeleteFile
{
    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, Result>
    {
        private readonly IFileUploadService _fileUploadService;

        public DeleteFileCommandHandler(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        public async Task<Result> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.PublicId))
            {
                return Error.Validation("Public ID is required");
            }

            await _fileUploadService.DeleteFileAsync(request.PublicId);

            return Result.Success();
        }
    }
}
