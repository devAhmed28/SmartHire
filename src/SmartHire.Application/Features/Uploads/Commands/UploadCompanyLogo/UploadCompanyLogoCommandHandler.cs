using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Uploads;

namespace SmartHire.Application.Features.Uploads.Commands.UploadCompanyLogo
{
    public class UploadCompanyLogoCommandHandler : IRequestHandler<UploadCompanyLogoCommand, Result<UploadFileResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileUploadService _fileUploadService;

        public UploadCompanyLogoCommandHandler(IUnitOfWork unitOfWork, IFileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
        }

        public async Task<Result<UploadFileResponse>> Handle(UploadCompanyLogoCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);

            if (company == null)
            {
                return Error.NotFound("Company");
            }

            var folder = $"companies/{company.Id}/logo";

            var fileUrl = await _fileUploadService.UploadFileAsync(request.File, folder);

            var publicId = fileUrl.Split('/').Last().Split('.')[0];

            var fullPublicId = $"{folder}/{publicId}";

            company.UpdateLogo(fileUrl);

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
