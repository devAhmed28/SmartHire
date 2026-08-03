using MediatR;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Jobs.Commands.CloseJob
{
    public class CloseJobCommand : IRequest<Result>
    {
        public Guid JobId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
