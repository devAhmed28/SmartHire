namespace SmartHire.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public bool IsDeleted { get; protected set; }
        
        public void SoftDelete()
        {
            IsDeleted = true;
        }
    }
}
