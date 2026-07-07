using SmartHire.Domain.Common;

namespace SmartHire.Domain.Entities
{
    public class Review : BaseAuditableEntity
    {
        public Guid CompanyId { get; private set; }
        public Guid UserId { get; private set; }
        public int Rating { get; private set; }
        public string Comment { get; private set; } = string.Empty;
        public Company Company { get; private set; } = null!;
        public User User { get; private set; } = null!;
    }
}
