using SmartHire.Domain.Common;

namespace SmartHire.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public RefreshToken(Guid userId, string token, DateTime expiresAt)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
            IsRevoked = false;
        }

        public Guid UserId { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsRevoked { get; private set; }
        public User User { get; private set; } = null!;

        public void Revoke()
        {
            IsRevoked = true;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpiresAt;
        }
    }
}
