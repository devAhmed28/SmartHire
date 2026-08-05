using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Offers;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Offers.Commands.SendOffer
{
    public class SendOfferCommand : IRequest<Result<OfferResponse>>
    {
        public Guid CompanyId { get; set; }
        public Guid ApplicationId { get; set; }
        public decimal Salary { get; set; }
        public Currency Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Notes { get; set; }
    }
}
