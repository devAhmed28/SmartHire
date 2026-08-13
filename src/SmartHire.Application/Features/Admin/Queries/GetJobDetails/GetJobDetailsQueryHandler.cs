using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Queries.GetJobDetails
{
    public class GetJobDetailsQueryHandler : IRequestHandler<GetJobDetailsQuery, Result<JobAdminResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetJobDetailsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<JobAdminResponse>> Handle(GetJobDetailsQuery request, CancellationToken cancellationToken)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);

            if (job == null)
            {
                return Error.NotFound("Job");
            }

            var company = await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken);

            var applicationCount = job.JobApplications?.Count ?? 0;

            var response = new JobAdminResponse
            {
                Id = job.Id,
                Title = job.Title,
                CompanyName = company?.CompanyName ?? string.Empty,
                Location = job.Location,
                JobStatus = job.JobStatus,
                ApplicationCount = applicationCount,
                CreatedAt = job.CreatedAt,
                ExpirationDate = job.ExpirationDate
            };

            return Result.Success(response);
        }
    }
}
