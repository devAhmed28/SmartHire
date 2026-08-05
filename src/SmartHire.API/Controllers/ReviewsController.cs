using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.DTOs.Reviews;
using SmartHire.Application.Features.Reviews.Commands.CreateReview;
using SmartHire.Application.Features.Reviews.Commands.DeleteReview;
using SmartHire.Application.Features.Reviews.Commands.UpdateReview;
using SmartHire.Application.Features.Reviews.Queries.GetCompanyReviews;
using SmartHire.Application.Features.Reviews.Queries.GetMyReviews;
using System.Security.Claims;

namespace SmartHire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var command = new CreateReviewCommand
            {
                UserId = userId.Value,
                CompanyId = request.CompanyId,
                Rating = request.Rating,
                Comment = request.Comment
            };

            var result = await _mediator.Send(command, cancellationToken);
            
            return ToActionResult(result);
        }

        [HttpGet("company/{companyId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanyReviews(Guid companyId, CancellationToken cancellationToken)
        {
            var query = new GetCompanyReviewsQuery
            {
                CompanyId = companyId
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyReviews(CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var query = new GetMyReviewsQuery
            {
                UserId = userId.Value
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReviews(Guid id, [FromBody] UpdateReviewRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var command = new UpdateReviewCommand
            {
                ReviewId = id,
                UserId = userId.Value,
                Rating = request.Rating,
                Comment = request.Comment
            };

            var result = await _mediator.Send(command, cancellationToken);
            
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReviews(Guid id, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var command = new DeleteReviewCommand
            {
                ReviewId = id,
                UserId = userId.Value
            };

            var result = await _mediator.Send(command, cancellationToken);
            
            return ToActionResult(result);
        }

        private Guid? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return null;
            }

            return userId;
        }
    }
}
