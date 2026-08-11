using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Queries.GetUserDetails
{
    public class GetUserDetailsQuery : IRequest<Result<UserAdminResponse>>
    {
        public Guid UserId { get; set; }
    }
}