using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Reviews;
using SmartHire.Domain.Entities;

namespace SmartHire.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result<ReviewResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateReviewCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ReviewResponse>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId,  cancellationToken);

            if (company == null)
            {
                return Error.NotFound("Company");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                return Error.NotFound("User");
            }

            var existingReview = await _unitOfWork.Reviews.GetByCompanyAndUserAsync(request.CompanyId, request.UserId, cancellationToken);

            if (existingReview != null)
            {
                return Error.Conflict("You have already reviewed this company");
            }

            if (request.Rating < 1 || request.Rating > 5)
            {
                return Error.Validation("Rating must be between 1 and 5");
            }

            var review = new Review(
                request.CompanyId,
                request.UserId,
                request.Rating,
                request.Comment
            );

            await _unitOfWork.Reviews.AddAsync(review, cancellationToken);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken); 

            var response = new ReviewResponse
            {
                Id = review.Id,
                CompanyId = review.CompanyId,
                CompanyName = company.CompanyName,
                UserId = review.UserId,
                UserName = $"{user.FirstName} {user.LastName}",
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            };

            return Result.Success(response);
        }
    }
}
