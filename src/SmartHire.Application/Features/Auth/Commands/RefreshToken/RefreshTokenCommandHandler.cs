using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Auth;

namespace SmartHire.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RefreshTokenCommandHandler(
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork=unitOfWork;
            _tokenService=tokenService;
            _dateTimeProvider=dateTimeProvider;
        }

        public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshTokenEntity = await _unitOfWork.RefreshTokens.GetByTokenAsync(request.RefreshToken, cancellationToken);

            if (refreshTokenEntity == null)
            {
                return Error.NotFound("Refresh token");
            }

            if (refreshTokenEntity.IsRevoked)
            {
                return Error.Forbidden("Refresh token has been revoked");
            }

            if (refreshTokenEntity.IsExpired())
            {
                return Error.Validation("Refresh token has expired. Please login again.");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(refreshTokenEntity.UserId, cancellationToken);

            if (user == null)
            {
                return Error.NotFound("User");
            }

            if (!user.IsActive)
            {
                return Error.Forbidden("Account is deactivated. Please contact support.");
            }

            refreshTokenEntity.Revoke();

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var newRefreshTokenEntity = new Domain.Entities.RefreshToken(
                user.Id,
                newRefreshToken,
                _dateTimeProvider.UtcNow.AddDays(7)
            );

            await _unitOfWork.RefreshTokens.AddAsync(newRefreshTokenEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = 900
            };

            return Result.Success(response);
        }
    }
}
