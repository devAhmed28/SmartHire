using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Users;

namespace SmartHire.Application.Features.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : IRequest<Result<UserProfileResponse>>
    {
        public Guid UserId { get; set; }
    }
}
