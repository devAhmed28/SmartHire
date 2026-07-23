using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Auth;
using SmartHire.Domain.Entities;

namespace SmartHire.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public LoginCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork=unitOfWork;
            _passwordHasher=passwordHasher;
            _tokenService=tokenService;
            _dateTimeProvider=dateTimeProvider;
        }

        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);

            if (user == null)
            {
                return Error.NotFound("User");
            }

            var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return Error.Validation("Invalid email or password");
            }

            if (!user.IsActive)
            {
                return Error.Forbidden("Account is deactivated. Please contact support.");
            }

            user.UpdateLastLogin();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _unitOfWork.RefreshTokens.RevokeAllUserTokensAsync(user.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenEntity = new Domain.Entities.RefreshToken(
                user.Id,
                refreshToken,
                _dateTimeProvider.UtcNow.AddDays(7)
            );

            await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 900
            };

            return Result.Success(response);
        }
    }
}
