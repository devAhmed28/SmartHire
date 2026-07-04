using SmartHire.Domain.Common;
using SmartHire.Domain.Enums;

namespace SmartHire.Domain.Entities
{
    public class Interview : BaseAuditableEntity
    {
        public Guid JobApplicationId { get; private set; }
        public DateTime InterviewDate { get; private set; }
        public InterviewType Type { get; private set; }
        public InterviewStatus Status { get; private set; }
        public string MeetingLink { get; private set; } = string.Empty;
        public string Notes { get; private set; } = string.Empty;
    }
}
