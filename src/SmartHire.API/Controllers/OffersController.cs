using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.DTOs.Offers;
using SmartHire.Application.Features.Candidates.Queries.GetCandidateProfile;
using SmartHire.Application.Features.Companies.Queries.GetCompanyByUserId;
using SmartHire.Application.Features.Offers.Commands.AcceptOffer;
using SmartHire.Application.Features.Offers.Commands.RejectOffer;
using SmartHire.Application.Features.Offers.Commands.SendOffer;
using SmartHire.Application.Features.Offers.Queries.GetCompanyOffers;
using SmartHire.Application.Features.Offers.Queries.GetMyOffers;
using SmartHire.Domain.Entities;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SmartHire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OffersController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public OffersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> SendOffer([FromBody] SendOfferRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var companyResult = await _mediator.Send(new GetCompanyByUserIdQuery { UserId = userId.Value }, cancellationToken);

            if (companyResult.IsFailure || companyResult.Value == null)
            {
                return Forbid("User does not have a company");
            }

            var command = new SendOfferCommand
            {
                CompanyId = companyResult.Value.Id,
                ApplicationId = request.ApplicationId,
                Salary = request.Salary,
                Currency = request.Currency,
                StartDate = request.StartDate,
                ExpirationDate = request.ExpirationDate,
                Notes = request.Notes
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyOffers(CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var candidateResult = await _mediator.Send(new GetCandidateProfileQuery { UserId = userId.Value }, cancellationToken);

            if (candidateResult.IsFailure || candidateResult.Value == null)
            {
                return Forbid("User is not a candidate");
            }

            var query = new GetMyOffersQuery
            {
                CandidateId = candidateResult.Value.Id
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("company")]
        public async Task<IActionResult> GetCompanyOffers(CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var companyResult = await _mediator.Send(new GetCompanyByUserIdQuery { UserId = userId.Value }, cancellationToken);

            if (companyResult.IsFailure || companyResult.Value == null)
            {
                return Forbid("User does not have a company");
            }

            var query = new GetCompanyOffersQuery
            {
                CompanyId = companyResult.Value.Id
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPost("{id}/accept")]
        public async Task<IActionResult> AcceptOffer(Guid id, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var candidateResult = await _mediator.Send(new GetCandidateProfileQuery { UserId = userId.Value }, cancellationToken);

            if (candidateResult.IsFailure || candidateResult.Value == null)
            {
                return Forbid("User is not a candidate");
            }

            var command = new AcceptOfferCommand
            {
                OfferId = id,
                CandidateId = candidateResult.Value.UserId
            };

            var result = await _mediator.Send(command, cancellationToken);
            
            return ToActionResult(result);
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectOffer(Guid id, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var candidateResult = await _mediator.Send(new GetCandidateProfileQuery { UserId = userId.Value }, cancellationToken);

            if (candidateResult.IsFailure || candidateResult.Value == null)
            {
                return Forbid("User is not a candidate");
            }

            var command = new RejectOfferCommand
            {
                OfferId = id,
                CandidateId = candidateResult.Value.UserId
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
