using SmartHire.Domain.Enums;

namespace SmartHire.Application.DTOs.Admin
{
    public class JobAdminResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public JobStatus JobStatus { get; set; }
        public int ApplicationCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
