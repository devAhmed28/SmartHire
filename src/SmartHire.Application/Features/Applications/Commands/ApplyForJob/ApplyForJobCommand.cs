using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Applications;

namespace SmartHire.Application.Features.Applications.Commands.ApplyForJob
{
    public class ApplyForJobCommand : IRequest<Result<ApplicationResponse>>
    {
        public Guid CandidateId { get; set; }
        public Guid JobId { get; set; }
        public string? CoverLetter { get; set; }
    }
}
