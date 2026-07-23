using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.DTOs.Auth;
using SmartHire.Application.Features.Auth.Commands.Register;

namespace SmartHire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterCommand
            {
                AccountType = request.AccountType,
                Email = request.Email,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
                Candidate = request.Candidate,
                Company = request.Company,
            };

            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }
    }
}
