using MediatR;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Users.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
