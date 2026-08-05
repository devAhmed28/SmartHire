using MediatR;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Notifications.Queries.GetUnreadCount
{
    public class GetUnreadCountQuery : IRequest<Result<int>>
    {
        public Guid UserId { get; set; }
    }
}
