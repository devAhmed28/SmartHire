using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Reviews;

namespace SmartHire.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, Result<ReviewResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateReviewCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ReviewResponse>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(request.ReviewId, cancellationToken);

            if (review == null)
            {
                return Error.NotFound("Review");
            }

            if (review.UserId != request.UserId)
            {
                return Error.Forbidden("You do not have permission to update this review");
            }

            if (request.Rating < 1 || request.Rating > 5)
            {
                return Error.Validation("Rating must be between 1 and 5");
            }

            review.UpdateReview(request.Rating, request.Comment);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var company = await _unitOfWork.Companies.GetByIdAsync(review.CompanyId, cancellationToken);
            
            var user = await _unitOfWork.Users.GetByIdAsync(review.UserId, cancellationToken);

            var response = new ReviewResponse
            {
                Id = review.Id,
                CompanyId = review.CompanyId,
                CompanyName = company?.CompanyName ?? string.Empty,
                UserId = review.UserId,
                UserName = user != null ? $"{user.FirstName} {user.LastName}" : string.Empty,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            };

            return Result.Success(response);
        }
    }
}
