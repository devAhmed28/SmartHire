using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Reviews;

namespace SmartHire.Application.Features.Reviews.Queries.GetCompanyReviews
{
    public class GetCompanyReviewsQuery : IRequest<Result<List<ReviewResponse>>>
    {
        public Guid CompanyId { get; set; }
    }
}
