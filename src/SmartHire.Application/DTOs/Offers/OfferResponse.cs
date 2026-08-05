using SmartHire.Domain.Enums;

namespace SmartHire.Application.DTOs.Offers
{
    public class OfferResponse
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CandidateName { get; set; } = string.Empty;
        public string CandidateEmail { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public Currency Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsAccepted { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
