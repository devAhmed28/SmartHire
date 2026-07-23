using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Auth;

namespace SmartHire.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<Result<AuthResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
