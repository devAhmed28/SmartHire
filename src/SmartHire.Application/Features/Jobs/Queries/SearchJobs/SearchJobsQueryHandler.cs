using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Jobs;

namespace SmartHire.Application.Features.Jobs.Queries.SearchJobs
{
    public class SearchJobsQueryHandler : IRequestHandler<SearchJobsQuery, Result<List<JobResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchJobsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<JobResponse>>> Handle(SearchJobsQuery request, CancellationToken cancellationToken)
        {
            var jobs = await _unitOfWork.Jobs.SearchJobsAsync(
                request.SearchTerm,
                request.JobType,
                request.WorkMode,
                request.MinSalary,
                request.MaxSalary,
                request.Location,
                cancellationToken
            );

            var response = new List<JobResponse>();

            foreach (var job in jobs)
            {
                var company = await _unitOfWork.Companies.GetByIdAsync(job.CompanyId, cancellationToken);

                response.Add(new JobResponse
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
                });
            }

            return Result.Success(response);
        }
    }
}
