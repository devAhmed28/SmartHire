using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Reviews;

namespace SmartHire.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommand : IRequest<Result<ReviewResponse>>
    {
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
