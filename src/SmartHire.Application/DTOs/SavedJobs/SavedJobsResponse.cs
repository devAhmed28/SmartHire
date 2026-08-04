namespace SmartHire.Application.DTOs.SavedJobs
{
    public class SavedJobResponse
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
        public DateTime SavedAt { get; set; }
    }
}
