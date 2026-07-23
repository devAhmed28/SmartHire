using SmartHire.Application.Common.Interfaces;

namespace SmartHire.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
