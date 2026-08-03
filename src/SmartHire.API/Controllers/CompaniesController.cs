using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.DTOs.Companies;
using SmartHire.Application.Features.Companies.Commands.UpdateCompany;
using SmartHire.Application.Features.Companies.Queries.GetCompany;
using System.Security.Claims;

namespace SmartHire.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompaniesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyCompany(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            
            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var query = new GetMyCompanyQuery
            {
                UserId = userId.Value
            };

            var result = await _mediator.Send(query, cancellationToken);
            return ToActionResult(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var command = new UpdateCompanyCommand
            {
                UserId = userId.Value,
                CompanyName = request.CompanyName,
                Description = request.Description,
                Industry = request.Industry,
                WebsiteUrl = request.WebsiteUrl,
                LinkedInUrl = request.LinkedInUrl,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                FoundedYear = request.FoundedYear,
                CompanySize = request.CompanySize
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
