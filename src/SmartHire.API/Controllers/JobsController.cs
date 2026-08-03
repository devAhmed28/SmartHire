using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.DTOs.Jobs;
using SmartHire.Application.Features.Companies.Queries.GetCompanyByUserId;  // ← CHANGE THIS
using SmartHire.Application.Features.Jobs.Commands.CreateJob;
using SmartHire.Application.Features.Jobs.Commands.DeleteJob;
using SmartHire.Application.Features.Jobs.Commands.UpdateJob;
using SmartHire.Application.Features.Jobs.Queries.GetCompanyJobs;
using SmartHire.Application.Features.Jobs.Queries.GetJob;

namespace SmartHire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public JobsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobRequest request, CancellationToken cancellationToken)
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

            var command = new CreateJobCommand
            {
                CompanyId = companyResult.Value.Id,
                Title = request.Title,
                Description = request.Description,
                Responsibilities = request.Responsibilities,
                Requirements = request.Requirements,
                SalaryMin = request.SalaryMin,
                SalaryMax = request.SalaryMax,
                Location = request.Location,
                Vacancies = request.Vacancies,
                Currency = request.Currency,
                JobType = request.JobType,
                WorkMode = request.WorkMode,
                ExpirationDate = request.ExpirationDate,
                SkillIds = request.SkillIds
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("company")]
        public async Task<IActionResult> GetMyJobs(CancellationToken cancellationToken)
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

            var query = new GetCompanyJobsQuery
            {
                CompanyId = companyResult.Value.Id
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJob(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetJobQuery
            {
                JobId = id
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(Guid id, [FromBody] UpdateJobRequest request, CancellationToken cancellationToken)
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

            var command = new UpdateJobCommand
            {
                JobId = id,
                CompanyId = companyResult.Value.Id,
                Title = request.Title,
                Description = request.Description,
                Responsibilities = request.Responsibilities,
                Requirements = request.Requirements,
                SalaryMin = request.SalaryMin,
                SalaryMax = request.SalaryMax,
                Location = request.Location,
                Vacancies = request.Vacancies,
                Currency = request.Currency,
                JobType = request.JobType,
                WorkMode = request.WorkMode,
                ExpirationDate = request.ExpirationDate,
                SkillIds = request.SkillIds
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(Guid id, CancellationToken cancellationToken)
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

            var command = new DeleteJobCommand
            {
                JobId = id,
                CompanyId = companyResult.Value.Id
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