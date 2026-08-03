using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Jobs;

namespace SmartHire.Application.Features.Jobs.Queries.GetCompanyJobs
{
    public class GetCompanyJobsQueryHandler : IRequestHandler<GetCompanyJobsQuery, Result<List<JobResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyJobsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<JobResponse>>> Handle(GetCompanyJobsQuery request, CancellationToken cancellationToken)
        {
            var jobs = await _unitOfWork.Jobs.GetByCompanyIdAsync(request.CompanyId, cancellationToken);

            var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);

            var response = jobs.Select(job => new JobResponse
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
            }).ToList();

            return Result.Success(response);
        }
    }
}
