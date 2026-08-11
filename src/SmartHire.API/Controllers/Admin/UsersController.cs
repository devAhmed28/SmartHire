using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.Features.Admin.Commands.ActivateUser;
using SmartHire.Application.Features.Admin.Commands.DeactivateUser;
using SmartHire.Application.Features.Admin.Queries.GetAllUsers;
using SmartHire.Application.Features.Admin.Queries.GetUserDetails;

namespace SmartHire.API.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    public class UsersController : AdminControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var query = new GetAllUsersQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetails(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetUserDetailsQuery
            {
                UserId = id
            };

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeactivateUserCommand
            {
                UserId = id
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateUser(Guid id, CancellationToken cancellationToken)
        {
            var command = new ActivateUserCommand
            {
                UserId = id
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }
    }
}
