using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Queries.GetAllJobs
{
    public class GetAllJobsQueryHandler : IRequestHandler<GetAllJobsQuery, Result<List<JobAdminResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllJobsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<JobAdminResponse>>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
        {
            var jobs = await _unitOfWork.Jobs.GetAllAsync(cancellationToken);

            var response = new List<JobAdminResponse>();

            foreach (var job in jobs)
            {
                var company = await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken);

                var applicationCount = job.JobApplications?.Count ?? 0;

                response.Add(new JobAdminResponse
                {
                    Id = job.Id,
                    Title = job.Title,
                    CompanyName = company?.CompanyName ?? string.Empty,
                    Location = job.Location,
                    JobStatus = job.JobStatus,
                    ApplicationCount = applicationCount,
                    CreatedAt = job.CreatedAt,
                    ExpirationDate = job.ExpirationDate
                });
            }

            return Result.Success(response);
        }
    }
}
