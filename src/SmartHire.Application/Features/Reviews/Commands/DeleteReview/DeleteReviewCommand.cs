using MediatR;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommand : IRequest<Result>
    {
        public Guid ReviewId { get; set; }
        public Guid UserId { get; set; }
    }
}
