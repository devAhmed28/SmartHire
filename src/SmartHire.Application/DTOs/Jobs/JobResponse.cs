using SmartHire.Domain.Enums;

namespace SmartHire.Application.DTOs.Jobs
{
    public class JobResponse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
        public string Location { get; set; } = string.Empty;
        public int Vacancies { get; set; }
        public Currency Currency { get; set; }
        public JobType JobType { get; set; }
        public WorkMode WorkMode { get; set; }
        public JobStatus JobStatus { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Skills { get; set; } = new List<string>();
    }
}
