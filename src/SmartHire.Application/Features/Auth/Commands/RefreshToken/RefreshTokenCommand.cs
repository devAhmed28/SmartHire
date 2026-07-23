using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Auth;

namespace SmartHire.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<Result<AuthResponse>>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
