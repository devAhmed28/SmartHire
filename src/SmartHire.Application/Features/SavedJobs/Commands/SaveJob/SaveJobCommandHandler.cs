using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Domain.Entities;

namespace SmartHire.Application.Features.SavedJobs.Commands.SaveJob
{
    public class SaveJobCommandHandler : IRequestHandler<SaveJobCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaveJobCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SaveJobCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.CandidateProfiles.GetByUserIdAsync(request.CandidateId, cancellationToken);

            if (candidate == null)
            {
                return Error.NotFound("Candidate profile");
            }

            var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);

            if (job == null)
            {
                return Error.NotFound("Job");
            }

            var existingSavedJob = await _unitOfWork.SavedJobs.GetByCandidateAndJobAsync(
                candidate.Id,
                request.JobId,
                cancellationToken
            );

            if (existingSavedJob != null)
            {
                return Error.Conflict("Job is already saved");
            }

            var savedJob = new SavedJob(candidate.Id, request.JobId);
            await _unitOfWork.SavedJobs.AddAsync(savedJob, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
