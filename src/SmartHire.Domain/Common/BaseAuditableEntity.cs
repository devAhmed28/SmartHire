namespace SmartHire.Domain.Common
{
    public abstract class BaseAuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; protected set; }
        public Guid? CreatedBy { get; protected set; }
        public Guid? UpdatedBy { get; protected set; }

        public void MarkAsUpdated(Guid userId)
        {
            UpdatedAt = DateTime.UtcNow;
            CreatedBy = userId;
        }

        public void SetCreatedBy(Guid userId)
        {
            CreatedBy = userId;
        }
    }
}
