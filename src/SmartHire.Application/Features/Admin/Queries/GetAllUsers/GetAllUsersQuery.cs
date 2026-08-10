using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<Result<List<UserAdminResponse>>>
    {
    }
}
