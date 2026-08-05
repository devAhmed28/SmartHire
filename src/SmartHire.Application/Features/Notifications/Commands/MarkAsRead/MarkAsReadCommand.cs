using MediatR;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Notifications.Commands.MarkAsRead
{
    public class MarkAsReadCommand : IRequest<Result>
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
    }
}
