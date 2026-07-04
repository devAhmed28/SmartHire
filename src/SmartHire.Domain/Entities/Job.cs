using SmartHire.Domain.Common;
using SmartHire.Domain.Enums;

namespace SmartHire.Domain.Entities
{
    public class Job : BaseAuditableEntity
    {
        public Guid CompanyId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Responsibilities { get; private set; } = string.Empty;
        public string Requirements { get; private set; } = string.Empty;
        public decimal Salary { get; private set; }
        public string Currency { get; private set; } = "USD";
        public string Location { get; private set; } = string.Empty;
        public JobType JobType { get; private set; }
        public WorkMode WorkMode { get; private set; }
        public JobStatus JobStatus { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public int Vacancies { get; private set; }
    }
}
