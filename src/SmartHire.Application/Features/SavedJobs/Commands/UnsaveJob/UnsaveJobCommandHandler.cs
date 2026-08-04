using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.SavedJobs.Commands.UnsaveJob
{
    public class UnsaveJobCommandHandler : IRequestHandler<UnsaveJobCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnsaveJobCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UnsaveJobCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.CandidateProfiles.GetByUserIdAsync(request.CandidateId, cancellationToken);

            if (candidate == null)
            {
                return Error.NotFound("Candidate profile");
            }

            var savedJob = await _unitOfWork.SavedJobs.GetByCandidateAndJobAsync(
                candidate.Id,
                request.JobId,
                cancellationToken
            );

            if (savedJob == null)
            {
                return Error.NotFound("Saved job not found");
            }

            await _unitOfWork.SavedJobs.DeleteAsync(savedJob);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
