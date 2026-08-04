using SmartHire.Domain.Common;
using SmartHire.Domain.Enums;

namespace SmartHire.Domain.Entities
{
    public class JobApplication : BaseAuditableEntity
    {
        public JobApplication(Guid candidateProfileId, Guid jobId, string? coverLetter)
        {
            Id = Guid.NewGuid();
            CandidateProfileId = candidateProfileId;
            JobId = jobId;
            CoverLetter = coverLetter;
            Status = ApplicationStatus.Pending;
            AppliedAt = DateTime.UtcNow;
        }

        public Guid CandidateProfileId { get; private set; }
        public Guid JobId { get; private set; }
        public ApplicationStatus Status { get; private set; }
        public DateTime AppliedAt { get; private set; }
        public string? CoverLetter { get; private set; }
        public CandidateProfile CandidateProfile { get; private set; } = null!;
        public Job Job { get; private set; } = null!;
        public ICollection<Interview> Interviews { get; private set; } = new List<Interview>();
        public Offer? Offer { get; private set; }

        public void UpdateStatus(ApplicationStatus status)
        {
            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Withdraw()
        {
            Status = ApplicationStatus.Withdrawn;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
