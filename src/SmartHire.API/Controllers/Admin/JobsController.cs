using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.Features.Admin.Commands.DeleteJob;
using SmartHire.Application.Features.Admin.Queries.GetAllJobs;
using SmartHire.Application.Features.Admin.Queries.GetJobDetails;
using System.Formats.Asn1;

namespace SmartHire.API.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    public class JobsController : AdminControllerBase
    {
        private readonly IMediator _mediator;

        public JobsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobs(CancellationToken cancellationToken)
        {
            var query = new GetAllJobsQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobDetails(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetJobDetailsQuery
            {
                JobId = id
            };

            var result = await _mediator.Send(query, cancellationToken);
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteJobCommand
            {
                JobId = id
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }
    }
}
