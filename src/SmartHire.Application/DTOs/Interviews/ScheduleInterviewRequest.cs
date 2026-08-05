using SmartHire.Domain.Enums;

namespace SmartHire.Application.DTOs.Interviews
{
    public class ScheduleInterviewRequest
    {
        public Guid ApplicationId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public InterviewType InterviewType { get; set; }
        public string? MeetingLink { get; set; } 
        public string? Notes { get; set; } 
    }
}
