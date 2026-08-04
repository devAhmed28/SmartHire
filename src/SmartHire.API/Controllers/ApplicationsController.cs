using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.DTOs.Applications;
using SmartHire.Application.Features.Applications.Commands.ApplyForJob;
using SmartHire.Application.Features.Applications.Commands.UpdateApplicationStatus;
using SmartHire.Application.Features.Applications.Commands.WithdrawApplication;
using SmartHire.Application.Features.Applications.Queries.GetJobApplications;
using SmartHire.Application.Features.Applications.Queries.GetMyApplications;
using SmartHire.Application.Features.Candidates.Queries.GetCandidateProfile;
using SmartHire.Application.Features.Companies.Queries.GetCompanyByUserId;
using SmartHire.Domain.Entities;
using System.Security.Claims;

namespace SmartHire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> ApplyForJob([FromBody] ApplyForJobRequest request, CancellationToken cancellationToken)
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

            var command = new ApplyForJobCommand
            {
                CandidateId = candidateResult.Value.UserId,
                JobId = request.JobId,
                CoverLetter = request.CoverLetter
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyApplications(CancellationToken cancellationToken)
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

            var query = new GetMyApplicationsQuery
            {
                CandidateId = candidateResult.Value.Id
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetJobApplications(Guid jobId, CancellationToken cancellationToken)
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

            var query = new GetJobApplicationsQuery
            {
                JobId = jobId,
                CompanyId = companyResult.Value.Id
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("{applicationId}/status")]
        public async Task<IActionResult> UpdateApplicationStatus(Guid applicationId, [FromBody] UpdateApplicationStatusRequest request, CancellationToken cancellationToken)
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

            var command = new UpdateApplicationStatusCommand
            {
                ApplicationId = applicationId,
                CompanyId = companyResult.Value.Id,
                Status = request.Status,
                Notes = request.Notes
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpDelete("{applicationId}")]
        public async Task<IActionResult> WithdrawApplication(Guid applicationId, CancellationToken cancellationToken)
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

            var command = new WithdrawApplicationCommand
            {
                ApplicationId = applicationId,
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
