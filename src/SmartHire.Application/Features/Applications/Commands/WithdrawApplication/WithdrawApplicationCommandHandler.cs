using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Applications.Commands.WithdrawApplication
{
    public class WithdrawApplicationCommandHandler : IRequestHandler<WithdrawApplicationCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public WithdrawApplicationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _unitOfWork.JobApplications.GetByIdAsync(request.ApplicationId, cancellationToken);

            if (application == null)
            {
                return Error.NotFound("Application");
            }

            var candidate = await _unitOfWork.CandidateProfiles.GetByIdAsync(application.CandidateProfileId, cancellationToken);

            if (candidate == null || candidate.UserId != request.CandidateId)
            {
                return Error.Forbidden("You do not have permission to withdraw this application");
            }

            if (application.Status == ApplicationStatus.Accepted || application.Status == ApplicationStatus.Rejected)
            {
                return Error.Validation($"Cannot withdraw application with status: {application.Status}");
            }

            application.Withdraw();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
