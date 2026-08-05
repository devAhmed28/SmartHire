using SmartHire.Domain.Common;
using SmartHire.Domain.Enums;

namespace SmartHire.Domain.Entities
{
    public class Offer : BaseAuditableEntity
    {
        public Offer(
            Guid jobApplicationId,
            decimal salary,
            Currency currency,
            DateTime startDate,
            DateTime expirationDate,
            string? notes)
        {
            Id = Guid.NewGuid();
            JobApplicationId = jobApplicationId;
            Salary = salary;
            Currency = currency;
            StartDate = startDate;
            ExpirationDate = expirationDate;
            IsAccepted = false;
            Notes = notes;
            CreatedAt = DateTime.UtcNow;
        }

        private Offer() { }

        public Guid JobApplicationId { get; private set; }
        public decimal Salary {  get; private set; }
        public Currency Currency { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool IsAccepted { get; private set; }
        public string? Notes { get; private set; }
        public JobApplication JobApplication { get; private set; } = null!;

        public void Accept()
        {
            IsAccepted = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reject()
        {
            IsAccepted = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpirationDate;
        }
    }
}
