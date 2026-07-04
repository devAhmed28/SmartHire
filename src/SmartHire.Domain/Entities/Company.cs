using SmartHire.Domain.Common;

namespace SmartHire.Domain.Entities
{
    public class Company : BaseAuditableEntity
    {
        public Guid UserId { get; private set; }
        public string CompanyName { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Industry { get; private set; } = string.Empty;
        public string WebsiteUrl { get; private set; } = string.Empty;
        public string? LogoUrl { get; private set; }
        public string Location { get; private set; } = string.Empty;
        public bool IsVerified { get; private set; }
        public User User { get; private set; } = null!;
    }
}
