using SmartHire.Domain.Common;

namespace SmartHire.Domain.Entities
{
    public class SavedJob : BaseAuditableEntity
    {
        public Guid CandidateProfileId { get; private set; }
        public Guid JobId { get; private set; }
    }
}
