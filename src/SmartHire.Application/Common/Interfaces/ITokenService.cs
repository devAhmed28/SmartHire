using SmartHire.Domain.Entities;

namespace SmartHire.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        Guid? GetUserIdFromToken(string token);
    }
}
