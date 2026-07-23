using MediatR;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
    }
}
