using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.DTOs.Auth;
using SmartHire.Application.Features.Auth.Commands.Login;
using SmartHire.Application.Features.Auth.Commands.RefreshToken;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var command = new RefreshTokenCommand
            {
                RefreshToken = request.RefreshToken
            };

            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }
    }
}
