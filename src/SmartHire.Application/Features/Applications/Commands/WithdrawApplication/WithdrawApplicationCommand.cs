using MediatR;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Applications.Commands.WithdrawApplication
{
    public class WithdrawApplicationCommand : IRequest<Result>
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
    }
}
