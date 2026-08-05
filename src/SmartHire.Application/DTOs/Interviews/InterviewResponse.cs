using SmartHire.Domain.Enums;

namespace SmartHire.Application.DTOs.Interviews
{
    public class InterviewResponse
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CandidateName { get; set; } = string.Empty;
        public string CandidateEmail { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public InterviewType InterviewType { get; set; }
        public InterviewStatus Status { get; set; }
        public string? MeetingLink { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
