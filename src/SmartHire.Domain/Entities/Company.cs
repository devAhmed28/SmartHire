using SmartHire.Domain.Common;
using SmartHire.Domain.Enums;

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
        public string? LinkedInUrl { get; private set; }
        public string Address { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string Country { get; private set; } = string.Empty;
        public int FoundedYear { get; private set; }
        public bool IsVerified { get; private set; }
        public CompanySize CompanySize { get; private set; }
        public User User { get; private set; } = null!;
        public ICollection<Job> Jobs { get; private set; } = new List<Job>();
        public ICollection<Review> Reviews { get; private set; } = new List<Review>();
    }
}
