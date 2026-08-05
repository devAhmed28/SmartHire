using SmartHire.Domain.Enums;

namespace SmartHire.Application.DTOs.Offers
{
    public class SendOfferRequest
    {
        public Guid ApplicationId { get; set; }
        public decimal Salary { get; set; }
        public Currency Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Notes { get; set; }
    }
}
