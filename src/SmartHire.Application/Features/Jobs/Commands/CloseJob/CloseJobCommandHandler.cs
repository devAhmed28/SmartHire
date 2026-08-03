using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Jobs.Commands.CloseJob
{
    public class CloseJobCommandHandler : IRequestHandler<CloseJobCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CloseJobCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CloseJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);

            if (job == null)
            {
                return Error.NotFound("Job");
            }

            if (job.CompanyId != request.CompanyId)
            {
                return Error.Forbidden("You do not have permission to close this job");
            }

            if (job.JobStatus == JobStatus.Closed)
            {
                return Error.Conflict("Job is already closed");
            }

            job.Close();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}