using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Uploads;

namespace SmartHire.Application.Features.Uploads.Commands.UploadProfileImage
{
    public class UploadProfileImageCommandHandler : IRequestHandler<UploadProfileImageCommand, Result<UploadFileResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileUploadService _fileUploadService;

        public UploadProfileImageCommandHandler(IUnitOfWork unitOfWork, IFileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
        }

        public async Task<Result<UploadFileResponse>> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                return Error.NotFound("User");
            }

            var folder = $"users/{user.Id}/profile";

            var fileUrl = await _fileUploadService.UploadFileAsync(request.File, folder);

            var publicId = fileUrl.Split('/').Last().Split('.')[0];

            var fullPublicId = $"{folder}/{publicId}";

            user.UpdateProfileImage(fileUrl);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new UploadFileResponse
            {
                FileUrl = fileUrl,
                PublicId = fullPublicId,
                FileName = request.File.FileName,
                FileSize = request.File.Length
            };

            return Result.Success(response);
        }
    }
}
