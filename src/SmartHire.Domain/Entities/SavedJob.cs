using SmartHire.Domain.Common;

namespace SmartHire.Domain.Entities
{
    public class SavedJob : BaseAuditableEntity
    {
        public Guid CandidateProfileId { get; private set; }
        public Guid JobId { get; private set; }
        public CandidateProfile CandidateProfile { get; private set; } = null!;
        public Job Job { get; private set; } = null!;
    }
}
