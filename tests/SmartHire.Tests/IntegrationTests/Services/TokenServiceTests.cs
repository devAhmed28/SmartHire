using AutoFixture;
using FluentAssertions;
using SmartHire.Domain.Entities;
using SmartHire.Infrastructure.Common.Settings;
using SmartHire.Infrastructure.Services;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.IntegrationTests.Services
{
    public class TokenServiceTests : TestBase
    {
        private readonly TokenService _tokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly DateTimeProvider _dateTimeProvider;

        public TokenServiceTests()
        {
            _jwtSettings = new JwtSettings
            {
                Secret = "MySuperSecretKeyThatIsAtLeast32CharactersLong123",
                Issuer = "SmartHire",
                Audience = "SmartHireClients",
                AccessTokenExpirationMinutes = 15,
                RefreshTokenExpirationDays = 7
            };

            _dateTimeProvider = new DateTimeProvider();
            _tokenService = new TokenService(_jwtSettings, _dateTimeProvider);
        }

        #region GenerateAccessToken

        [Fact]
        public async Task GenerateAccessToken_ShouldReturnValidToken()
        {
            // Arrange
            var user = _fixture.Create<User>();

            // Act
            var token = _tokenService.GenerateAccessToken(user);

            // Assert
            token.Should().NotBeNullOrEmpty();
            token.Split('.').Should().HaveCount(3);
        }

        #endregion

        #region GenerateRefreshToken

        [Fact]
        public async Task GenerateRefreshToken_ShouldReturnToken()
        {
            // Act
            var token = _tokenService.GenerateRefreshToken();

            // Assert
            token.Should().NotBeNullOrEmpty();
            token.Length.Should().BeGreaterThan(10);
        }

        #endregion
    }
}
