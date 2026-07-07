using SmartHire.Domain.Common;
using SmartHire.Domain.Enums;

namespace SmartHire.Domain.Entities
{
    public class Offer : BaseAuditableEntity
    {
        public Guid JobApplicationId { get; private set; }
        public decimal Salary {  get; private set; }
        public Currency Currency { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool IsAccepted { get; private set; }
        public JobApplication JobApplication { get; private set; } = null!;
    }
}
