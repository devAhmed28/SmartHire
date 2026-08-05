using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Offers;

namespace SmartHire.Application.Features.Offers.Commands.RejectOffer
{
    public class RejectOfferCommand : IRequest<Result<OfferResponse>>
    {
        public Guid OfferId { get; set; }
        public Guid CandidateId { get; set; }
    }
}
