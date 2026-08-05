using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Interviews;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Interviews.Commands.UpdateInterviewStatus
{
    public class UpdateInterviewStatusCommand : IRequest<Result<InterviewResponse>>
    {
        public Guid InterviewId { get; set; }
        public Guid CompanyId { get; set; }
        public InterviewStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
