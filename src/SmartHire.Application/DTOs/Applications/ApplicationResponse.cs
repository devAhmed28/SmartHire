using SmartHire.Domain.Enums;

namespace SmartHire.Application.DTOs.Applications
{
    public class ApplicationResponse
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string CandidateName { get; set; } = string.Empty;
        public string CandidateEmail { get; set; } = string.Empty;
        public ApplicationStatus Status { get; set; }
        public string? CoverLetter { get; set; }
        public DateTime AppliedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
