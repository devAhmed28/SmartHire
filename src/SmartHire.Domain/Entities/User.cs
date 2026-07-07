using SmartHire.Domain.Common;
using SmartHire.Domain.Enums;

namespace SmartHire.Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string? ProfileImageUrl { get; private set; }
        public Gender? Gender { get; private set; }
        public UserRole Role { get; private set; }
        public bool IsEmailConfirmed { get; private set; }
        public bool IsActive { get; private set; } = true;
        public DateTime? LastLoginAt { get; private set; }
        public Company? Company { get; private set; }
        public CandidateProfile? CandidateProfile { get; private set; }
        public ICollection<Notification> Notifications { get; private set; } = new List<Notification>();
        public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();
        public ICollection<Review> Reviews { get; private set; } = new List<Review>();
    }
}
