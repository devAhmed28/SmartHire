using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.Features.Admin.Commands.DeactivateCompany;
using SmartHire.Application.Features.Admin.Commands.VerifyCompany;
using SmartHire.Application.Features.Admin.Queries.GetAllCompanies;
using SmartHire.Application.Features.Admin.Queries.GetCompanyDetails;

namespace SmartHire.API.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    public class CompaniesController : AdminControllerBase
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies(CancellationToken cancellationToken)
        {
            var query = new GetAllCompaniesQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyDetails(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetCompanyDetailsQuery
            {
                CompanyId = id
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("{id}/verify")]
        public async Task<IActionResult> VerifyCompany(Guid id, CancellationToken cancellationToken)
        {
            var command = new VerifyCompanyCommand
            {
                CompanyId = id
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateCompany(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeactivateCompanyCommand
            {
                CompanyId = id
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }
    }
}
