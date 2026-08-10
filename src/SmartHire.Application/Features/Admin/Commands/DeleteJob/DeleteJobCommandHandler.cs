using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Admin.Commands.DeleteJob
{
    public class DeleteJobCommandHandler : IRequestHandler<DeleteJobCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteJobCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);

            if (job == null)
            {
                return Error.NotFound("Job");
            }

            if (job.JobStatus == JobStatus.Closed)
            {
                return Error.Conflict("Job is already deleted");
            }

            job.Close();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
