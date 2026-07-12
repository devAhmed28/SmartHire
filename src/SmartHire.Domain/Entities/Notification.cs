using SmartHire.Domain.Common;
using SmartHire.Domain.Enums;

namespace SmartHire.Domain.Entities
{
    public class Notification : BaseAuditableEntity
    {
        public Notification(Guid userId, string title, string message, NotificationType type)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Title = title;
            Message = message;
            Type = type;
            IsRead = false;
            CreatedAt = DateTime.UtcNow;
        }

        private Notification() {  }

        public Guid UserId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Message { get; private set; } = string.Empty;
        public NotificationType Type { get; private set; }
        public bool IsRead { get; private set; }
        public User User { get; private set; } = null!;

        public void MarkAsRead()
        {
            IsRead = true;
        }
        
        public void MarkAsUnRead()
        {
            IsRead = false;
        }
    }
}
