using SmartHire.Domain.Common;
using SmartHire.Domain.Enums;

namespace SmartHire.Domain.Entities
{
    public class Interview : BaseAuditableEntity
    {
        public Interview(
        Guid jobApplicationId,
        DateTime interviewDate,
        InterviewType type,
        string? meetingLink,
        string? notes)
        {
            Id = Guid.NewGuid();
            JobApplicationId = jobApplicationId;
            InterviewDate = interviewDate;
            Type = type;
            MeetingLink = meetingLink;
            Notes = notes ?? string.Empty;
            Status = InterviewStatus.Scheduled;
        }

        public Guid JobApplicationId { get; private set; }
        public DateTime InterviewDate { get; private set; }
        public InterviewType Type { get; private set; }
        public InterviewStatus Status { get; private set; }
        public string? MeetingLink { get; private set; }
        public string Notes { get; private set; } = string.Empty;
        public JobApplication JobApplication { get; private set; } = null!;

        public void UpdateStatus(InterviewStatus status, string? notes = null)
        {
            Status = status;

            if (!string.IsNullOrEmpty(notes))
            {
                Notes = notes;
            }

            UpdatedAt = DateTime.UtcNow;
        }
    }
}
