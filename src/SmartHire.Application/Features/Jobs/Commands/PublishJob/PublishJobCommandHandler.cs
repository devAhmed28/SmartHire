using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Jobs.Commands.PublishJob
{
    public class PublishJobCommandHandler : IRequestHandler<PublishJobCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublishJobCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(PublishJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);

            if (job == null)
            {
                return Error.NotFound("Job");
            }

            if (job.CompanyId != request.CompanyId)
            {
                return Error.Forbidden("You do not have permission to publish this job");
            }

            if (job.JobStatus == JobStatus.Published)
            {
                return Error.Conflict("Job is already published");
            }

            job.Publish();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
