using SmartHire.Domain.Common;

namespace SmartHire.Domain.Entities
{
    public class SavedJob : BaseAuditableEntity
    {
        public SavedJob(Guid candidateProfileId, Guid jobId) 
        {
            Id = Guid.NewGuid();
            CandidateProfileId = candidateProfileId;
            JobId = jobId;
            CreatedAt = DateTime.UtcNow;
        }
        public Guid CandidateProfileId { get; private set; }
        public Guid JobId { get; private set; }
        public CandidateProfile CandidateProfile { get; private set; } = null!;
        public Job Job { get; private set; } = null!;
    }
}
