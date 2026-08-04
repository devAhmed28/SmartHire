using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Applications;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Applications.Commands.UpdateApplicationStatus
{
    public class UpdateApplicationStatusCommand : IRequest<Result<ApplicationResponse>>
    {
        public Guid ApplicationId { get; set; }
        public Guid CompanyId { get; set; }
        public ApplicationStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
