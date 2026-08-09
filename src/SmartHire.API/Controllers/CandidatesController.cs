using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.DTOs.Candidate;
using SmartHire.Application.DTOs.Uploads;
using SmartHire.Application.Features.Candidates.Commands.AddSkill;
using SmartHire.Application.Features.Candidates.Commands.RemoveSkill;
using SmartHire.Application.Features.Candidates.Commands.UpdateCandidateProfile;
using SmartHire.Application.Features.Candidates.Queries.GetCandidateProfile;
using SmartHire.Application.Features.SavedJobs.Queries.GetSavedJobs;
using SmartHire.Application.Features.Uploads.Commands.UploadCV;
using System.Security.Claims;

namespace SmartHire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CandidatesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CandidatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var query = new GetCandidateProfileQuery
            {
                UserId = userId.Value
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateCandidateProfileRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var command = new UpdateCandidateProfileCommand
            {
                UserId = userId.Value,
                Bio = request.Bio,
                GitHubUrl = request.GitHubUrl,
                LinkedInUrl = request.LinkedInUrl,
                PortfolioUrl = request.PortfolioUrl,
                YearsOfExperience = request.YearsOfExperience,
                CurrentPosition = request.CurrentPosition,
                CurrentLocation = request.CurrentLocation,
                ExpectedSalary = request.ExpectedSalary,
                IsOpenToWork = request.IsOpenToWork
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPost("skills")]
        public async Task<IActionResult> AddSkill([FromBody] AddSkillRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var command = new AddSkillCommand
            {
                UserId = userId.Value,
                SkillName = request.SkillName,
                YearsOfExperience = request.YearsOfExperience
            };

            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        [HttpDelete("skills/{skillName}")]
        public async Task<IActionResult> RemoveSkill(string skillName, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var command = new RemoveSkillCommand
            {
                UserId = userId.Value,
                SkillName = skillName
            };

            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        [HttpGet("me/saved-jobs")]
        public async Task<IActionResult> GetSavedJobs(CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var candidateResult = await _mediator.Send(new GetCandidateProfileQuery { UserId =  userId.Value }, cancellationToken);

            if (candidateResult.IsFailure || candidateResult.Value == null)
            {
                return Forbid("User is not a candidate");
            }

            var query = new GetSavedJobsQuery
            {
                CandidateId = candidateResult.Value.Id
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPost("me/cv")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> UploadCV([FromForm] UploadCVRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var command = new UploadCVCommand
            {
                UserId = userId.Value,
                File = request.File
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        private Guid? GetUserId()
        {
            var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaims) || !Guid.TryParse(userIdClaims, out var userId))
            {
                return null;
            }

            return userId;
        }
    }
}
