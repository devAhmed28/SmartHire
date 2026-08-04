using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Applications;

namespace SmartHire.Application.Features.Applications.Commands.UpdateApplicationStatus
{
    public class UpdateApplicationStatusCommandHandler : IRequestHandler<UpdateApplicationStatusCommand, Result<ApplicationResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateApplicationStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ApplicationResponse>> Handle(UpdateApplicationStatusCommand request, CancellationToken cancellationToken)
        {
            var application = await _unitOfWork.JobApplications.GetByIdAsync(request.ApplicationId, cancellationToken);
            if (application == null)
            {
                return Error.NotFound("Application");
            }

            var job = await _unitOfWork.Jobs.GetByIdAsync(application.JobId, cancellationToken);
            if (job == null)
            {
                return Error.NotFound("Job");
            }

            if (job.CompanyId != request.CompanyId)
            {
                return Error.Forbidden("You do not have permission to update this application");
            }

            application.UpdateStatus(request.Status);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var candidate = await _unitOfWork.CandidateProfiles.GetByIdAsync(application.CandidateProfileId, cancellationToken);
            var user = candidate != null ? await _unitOfWork.Users.GetByIdAsync(candidate.UserId, cancellationToken) : null;

            var response = new ApplicationResponse
            {
                Id = application.Id,
                JobId = application.JobId,
                JobTitle = job.Title,
                CompanyName = job.Company.CompanyName,
                CandidateName = user != null ? $"{user.FirstName} {user.LastName}" : string.Empty,
                CandidateEmail = user?.Email ?? string.Empty,
                Status = application.Status,
                CoverLetter = application.CoverLetter,
                AppliedAt = application.AppliedAt,
                UpdatedAt = application.UpdatedAt
            };

            return Result.Success(response);
        }
    }
}
