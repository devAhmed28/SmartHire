using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Jobs;

namespace SmartHire.Application.Features.Jobs.Queries.GetCompanyJobs
{
    public class GetCompanyJobsQuery : IRequest<Result<List<JobResponse>>>
    {
        public Guid CompanyId { get; set; }
    }
}
