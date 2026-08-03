using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Jobs;

namespace SmartHire.Application.Features.Jobs.Queries.GetJob
{
    public class GetJobQueryHandler : IRequestHandler<GetJobQuery, Result<JobResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetJobQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<JobResponse>> Handle(GetJobQuery request, CancellationToken cancellationToken)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);

            if (job == null)
            {
                return Error.NotFound("Job");
            }

            var company = await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken);

            var response = new JobResponse
            {
                Id = job.Id,
                CompanyId = job.CompanyId,
                CompanyName = company?.CompanyName ?? string.Empty,
                Title = job.Title,
                Description = job.Description,
                Responsibilities = job.Responsibilities,
                Requirements = job.Requirements,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                Location = job.Location,
                Vacancies = job.Vacancies,
                Currency = job.Currency,
                JobType = job.JobType,
                WorkMode = job.WorkMode,
                JobStatus = job.JobStatus,
                ExpirationDate = job.ExpirationDate,
                CreatedAt = job.CreatedAt,
                Skills = new List<string>()
            };

            return Result.Success(response);
        }
    }
}
