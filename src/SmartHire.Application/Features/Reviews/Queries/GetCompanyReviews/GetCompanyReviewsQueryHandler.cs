using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Reviews;

namespace SmartHire.Application.Features.Reviews.Queries.GetCompanyReviews
{
    public class GetCompanyReviewsQueryHandler : IRequestHandler<GetCompanyReviewsQuery, Result<List<ReviewResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyReviewsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<ReviewResponse>>> Handle(GetCompanyReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _unitOfWork.Reviews.GetWithCompanyDetailsAsync(request.CompanyId, cancellationToken); 

            var response = reviews.Select(r => new ReviewResponse
            {
                Id = r.Id,
                CompanyId = r.CompanyId,
                CompanyName = r.Company.CompanyName,
                UserId = r.UserId,
                UserName = $"{r.User.FirstName} {r.User.LastName}",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).ToList();

            return Result.Success(response);
        }
    }
}
