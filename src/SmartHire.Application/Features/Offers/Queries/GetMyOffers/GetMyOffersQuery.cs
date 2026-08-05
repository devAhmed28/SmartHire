using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Offers;

namespace SmartHire.Application.Features.Offers.Queries.GetMyOffers
{
    public class GetMyOffersQuery : IRequest<Result<List<OfferResponse>>>
    {
        public Guid CandidateId { get; set; }
    }
}
