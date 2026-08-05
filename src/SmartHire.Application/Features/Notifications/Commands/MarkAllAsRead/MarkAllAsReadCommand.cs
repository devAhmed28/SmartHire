using MediatR;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Notifications.Commands.MarkAllAsRead
{
    public class MarkAllAsReadCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
    }
}
