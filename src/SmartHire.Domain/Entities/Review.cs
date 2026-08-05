using SmartHire.Domain.Common;

namespace SmartHire.Domain.Entities
{
    public class Review : BaseAuditableEntity
    {
        public Review(Guid companyId, Guid userId, int rating, string comment)
        {
            Id = Guid.NewGuid();
            CompanyId = companyId;
            UserId = userId;
            Rating = rating;
            Comment = comment;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid CompanyId { get; private set; }
        public Guid UserId { get; private set; }
        public int Rating { get; private set; }
        public string Comment { get; private set; } = string.Empty;
        public Company Company { get; private set; } = null!;
        public User User { get; private set; } = null!;

        public void UpdateReview(int rating, string comment)
        {
            Rating = rating;
            Comment = comment;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
