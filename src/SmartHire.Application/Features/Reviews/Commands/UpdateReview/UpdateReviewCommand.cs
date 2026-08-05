using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Reviews;

namespace SmartHire.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommand : IRequest<Result<ReviewResponse>>
    {
        public Guid ReviewId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
