using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Interviews;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Interviews.Commands.ScheduleInterview
{
    public class ScheduleInterviewCommand : IRequest<Result<InterviewResponse>>
    {
        public Guid CompanyId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public InterviewType InterviewType { get; set; }
        public string? MeetingLink { get; set; }
        public string? Notes { get; set; }
    }
}
