using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using System.Security.Cryptography;

namespace SmartHire.Application.Common.Interfaces.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IReadOnlyList<Notification>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Notification>> GetUnreadByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Notification>> GetByTypeAsync(NotificationType type, CancellationToken cancellationToken = default);
        Task<int> CountUnreadAsync(Guid userId, CancellationToken cancellationToken = default);
        Task MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
