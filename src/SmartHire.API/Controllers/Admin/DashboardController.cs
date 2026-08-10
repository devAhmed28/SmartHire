using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.Features.Admin.Queries.GetDashboardStats;

namespace SmartHire.API.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    public class DashboardController : AdminControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardStats(CancellationToken cancellationToken)
        {
            var query = new GetDashboardStatsQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }
    }
}
