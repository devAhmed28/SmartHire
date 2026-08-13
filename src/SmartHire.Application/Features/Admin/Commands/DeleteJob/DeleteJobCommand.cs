using MediatR;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Admin.Commands.DeleteJob
{
    public class DeleteJobCommand : IRequest<Result>
    {
        public Guid JobId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
