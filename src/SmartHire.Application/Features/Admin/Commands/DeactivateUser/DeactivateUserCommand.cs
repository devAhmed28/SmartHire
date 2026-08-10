using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Commands.DeactivateUser
{
    public class DeactivateUserCommand : IRequest<Result<UserAdminResponse>>
    {
        public Guid UserId { get; set; }
    }
}
