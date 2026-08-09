using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Uploads;

namespace SmartHire.Application.Features.Uploads.Commands.UploadCV
{
    public class UploadCVCommandHandler : IRequestHandler<UploadCVCommand, Result<UploadFileResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileUploadService _fileUploadService;

        public UploadCVCommandHandler(IUnitOfWork unitOfWork, IFileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
        }

        public async Task<Result<UploadFileResponse>> Handle(UploadCVCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.CandidateProfiles.GetByUserIdAsync(request.UserId, cancellationToken);

            if (candidate == null)
            {
                return Error.NotFound("Candidate Profile");
            }

            var folder = $"candidate/{candidate.Id}/cv";

            var fileUrl = await _fileUploadService.UploadFileAsync(request.File, folder);

            var publicId = fileUrl.Split('/').Last().Split('.')[0];

            var fullPublicId = $"{folder}/{publicId}";

            candidate.UpdateCV(fileUrl);

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
