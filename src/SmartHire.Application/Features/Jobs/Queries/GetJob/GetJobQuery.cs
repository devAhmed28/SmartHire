using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Jobs;

namespace SmartHire.Application.Features.Jobs.Queries.GetJob
{
    public class GetJobQuery : IRequest<Result<JobResponse>>
    {
        public Guid JobId { get; set; }
    }
}
