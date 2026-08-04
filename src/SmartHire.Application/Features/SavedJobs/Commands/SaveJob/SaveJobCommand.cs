using MediatR;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.SavedJobs.Commands.SaveJob
{
    public class SaveJobCommand : IRequest<Result>
    {
        public Guid CandidateId { get; set; }
        public Guid JobId { get; set; }
    }
}
