using SmartHire.Domain.Common;

namespace SmartHire.Domain.Entities
{
    public class Offer : BaseAuditableEntity
    {
        public Guid JobApplicationId { get; private set; }
        public decimal Salary {  get; private set; }
        public string Currency { get; private set; } = "USD";
        public DateTime StartDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool IsAccepted { get; private set; }
    }
}
