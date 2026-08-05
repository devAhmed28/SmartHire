using SmartHire.Domain.Enums;

namespace SmartHire.Application.DTOs.Interviews
{
    public class UpdateInterviewStatusRequest
    {
        public InterviewStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
