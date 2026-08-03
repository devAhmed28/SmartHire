using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Jobs;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Jobs.Queries.SearchJobs
{
    public class SearchJobsQuery : IRequest<Result<List<JobResponse>>>
    {
        public string? SearchTerm { get; set; }
        public JobType? JobType { get; set; }
        public WorkMode? WorkMode { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string? Location { get; set; }
    }
}
