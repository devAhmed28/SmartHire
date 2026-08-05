using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Reviews;

namespace SmartHire.Application.Features.Reviews.Queries.GetMyReviews
{
    public class GetMyReviewsQuery : IRequest<Result<List<ReviewResponse>>>
    {
        public Guid UserId { get; set; }
    }
}
