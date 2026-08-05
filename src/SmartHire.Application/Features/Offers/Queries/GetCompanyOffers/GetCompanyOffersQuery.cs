using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Offers;

namespace SmartHire.Application.Features.Offers.Queries.GetCompanyOffers
{
    public class GetCompanyOffersQuery : IRequest<Result<List<OfferResponse>>>
    {
        public Guid CompanyId { get; set; }
    }
}
