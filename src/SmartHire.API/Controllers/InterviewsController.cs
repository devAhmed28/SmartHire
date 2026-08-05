using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.DTOs.Interviews;
using SmartHire.Application.Features.Candidates.Queries.GetCandidateProfile;
using SmartHire.Application.Features.Companies.Queries.GetCompanyByUserId;
using SmartHire.Application.Features.Interviews.Commands.ScheduleInterview;
using SmartHire.Application.Features.Interviews.Commands.UpdateInterviewStatus;
using SmartHire.Application.Features.Interviews.Queries.GetCompanyInterviews;
using SmartHire.Application.Features.Interviews.Queries.GetInterviewById;
using SmartHire.Application.Features.Interviews.Queries.GetMyInterviews;
using System.Security.Claims;

namespace SmartHire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InterviewsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public InterviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleInterview([FromBody] ScheduleInterviewRequest request, CancellationToken cancellationToken)
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

            var command = new ScheduleInterviewCommand
            {
                CompanyId = companyResult.Value.Id,
                ApplicationId = request.ApplicationId,
                ScheduledDate = request.ScheduledDate,
                InterviewType = request.InterviewType,
                MeetingLink = request.MeetingLink,
                Notes = request.Notes
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyInterviews(CancellationToken cancellationToken)
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

            var query = new GetMyInterviewsQuery
            {
                CandidateId = candidateResult.Value.Id
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("company")]
        public async Task<IActionResult> GetCompanyInterviews(CancellationToken cancellationToken)
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

            var query = new GetCompanyInterviewsQuery
            {
                CompanyId = companyResult.Value.Id
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateInterviewStatus(Guid id, [FromBody] UpdateInterviewStatusRequest request, CancellationToken cancellationToken)
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

            var command = new UpdateInterviewStatusCommand
            {
                InterviewId = id,
                CompanyId = companyResult.Value.Id,
                Status = request.Status,
                Notes = request.Notes
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInterview(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetInterviewByIdQuery
            {
                InterviewId = id
            };

            var result = await _mediator.Send(query, cancellationToken);

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
