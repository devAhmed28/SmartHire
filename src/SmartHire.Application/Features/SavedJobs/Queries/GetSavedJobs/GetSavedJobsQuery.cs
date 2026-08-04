using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.SavedJobs;

namespace SmartHire.Application.Features.SavedJobs.Queries.GetSavedJobs
{
    public class GetSavedJobsQuery : IRequest<Result<List<SavedJobResponse>>>
    {
        public Guid CandidateId { get; set; }
    }
}
