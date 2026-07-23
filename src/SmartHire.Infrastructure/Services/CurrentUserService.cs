using Microsoft.AspNetCore.Http;
using SmartHire.Application.Common.Interfaces;
using System.Security.Claims;

namespace SmartHire.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?
                    .User?.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return userId;
                }

                return null;
            }
        }

        public string? Email
        {
            get
            {
                return _httpContextAccessor.HttpContext?
                    .User?.FindFirst(ClaimTypes.Email)?.Value;
            }
        }

        public string? Role
        {
            get
            {
                return _httpContextAccessor.HttpContext?
                    .User?.FindFirst(ClaimTypes.Role)?.Value;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return _httpContextAccessor.HttpContext?
                    .User?.Identity?.IsAuthenticated ?? false;
            }
        }
    }
}
