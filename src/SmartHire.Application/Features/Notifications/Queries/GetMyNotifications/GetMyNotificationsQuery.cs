using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Notifications;

namespace SmartHire.Application.Features.Notifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsQuery : IRequest<Result<List<NotificationResponse>>>
    {
        public Guid UserId { get; set; }
    }
}
