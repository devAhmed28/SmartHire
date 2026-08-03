using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Jobs.Commands.DeleteJob
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

            if (job.CompanyId != request.CompanyId)
            {
                return Error.Forbidden("You do not have permission to delete this job");
            }

            // !! soft delete
            await _unitOfWork.Jobs.DeleteAsync(job);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}