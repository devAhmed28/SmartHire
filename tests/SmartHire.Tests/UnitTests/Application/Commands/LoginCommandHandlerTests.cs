using AutoFixture;
using FluentAssertions;
using Moq;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Application.Features.Auth.Commands.Login;
using SmartHire.Domain.Entities;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Application.Commands
{
    public class LoginCommandHandlerTests : TestBase
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _tokenServiceMock = new Mock<ITokenService>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var userRepoMock = new Mock<IUserRepository>();
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            var refreshTokenRepoMock = new Mock<IRefreshTokenRepository>();
            _unitOfWorkMock.Setup(u => u.RefreshTokens).Returns(refreshTokenRepoMock.Object);

            _dateTimeProviderMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

            _handler = new LoginCommandHandler(
                _unitOfWorkMock.Object,
                _passwordHasherMock.Object,
                _tokenServiceMock.Object,
                _dateTimeProviderMock.Object
            );
        }

        #region Login - Success

        [Fact]
        public async Task Handle_ValidCredentials_ShouldReturnSuccessWithTokens()
        {
            // Arrange
            var user = _fixture.Create<User>();
            user.Activate();

            var command = new LoginCommand
            {
                Email = user.Email,
                Password = "Password123!"
            };

            _unitOfWorkMock.Setup(u => u.Users.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordHasherMock.Setup(p => p.VerifyPassword(command.Password, user.PasswordHash))
                .Returns(true);

            _tokenServiceMock.Setup(t => t.GenerateAccessToken(user))
                .Returns("access_token_123");
            _tokenServiceMock.Setup(t => t.GenerateRefreshToken())
                .Returns("refresh_token_456");

            _unitOfWorkMock.Setup(u => u.RefreshTokens.RevokeAllUserTokensAsync(user.Id, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Email.Should().Be(user.Email);
            result.Value.AccessToken.Should().Be("access_token_123");
            result.Value.RefreshToken.Should().Be("refresh_token_456");
            result.Value.ExpiresIn.Should().Be(900);

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        #endregion

        #region Login - Failures

        [Fact]
        public async Task Handle_UserNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var command = new LoginCommand
            {
                Email = "nonexistent@test.com",
                Password = "Password123!"
            };

            _unitOfWorkMock.Setup(u => u.Users.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("NotFound");
            result.Error.Message.Should().Contain("User");
        }

        [Fact]
        public async Task Handle_WrongPassword_ShouldReturnValidationError()
        {
            // Arrange
            var user = _fixture.Create<User>();
            user.Activate();

            var command = new LoginCommand
            {
                Email = user.Email,
                Password = "WrongPassword!"
            };

            _unitOfWorkMock.Setup(u => u.Users.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordHasherMock.Setup(p => p.VerifyPassword(command.Password, user.PasswordHash))
                .Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("Validation.Error");
            result.Error.Message.Should().Contain("Invalid email or password");
        }

        [Fact]
        public async Task Handle_DeactivatedUser_ShouldReturnForbidden()
        {
            // Arrange
            var user = _fixture.Create<User>();
            user.Deactivate();

            var command = new LoginCommand
            {
                Email = user.Email,
                Password = "Password123!"
            };

            _unitOfWorkMock.Setup(u => u.Users.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordHasherMock.Setup(p => p.VerifyPassword(command.Password, user.PasswordHash))
                .Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("Forbidden");
            result.Error.Message.Should().Contain("Account is deactivated");
        }

        #endregion
    }
}
