using AutoFixture;
using FluentAssertions;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Domain
{
    public class UserTests : TestBase
    {
        #region CreateUser

        [Fact]
        public async Task CreateUser_WithValidData_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var user = _fixture.Create<User>();

            // Assert
            user.Id.Should().NotBe(Guid.Empty);
            user.FirstName.Should().NotBeNullOrEmpty();
            user.LastName.Should().NotBeNullOrEmpty();
            user.Email.Should().NotBeNullOrEmpty();
            user.PhoneNumber.Should().NotBeNullOrEmpty();
            user.Role.Should().BeOneOf(UserRole.Candidate, UserRole.Company, UserRole.Admin);
            user.IsActive.Should().BeTrue();
            user.IsEmailConfirmed.Should().BeFalse();
            user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        #endregion

        #region Activate

        [Fact]
        public async Task Activate_WhenUserIsInactive_ShouldSetIsActiveToTrue()
        {
            // Arrange
            var user = _fixture.Create<User>();
            user.Deactivate(); // Make sure user is inactive
            user.IsActive.Should().BeFalse();

            // Act
            user.Activate();

            // Assert
            user.IsActive.Should().BeTrue();
            user.UpdatedAt.Should().NotBeNull();
        }

        #endregion

        #region Deactivate

        [Fact]
        public async Task Deactivate_WhenUserIsActive_ShouldSetIsActiveToFalse()
        {
            // Arrange
            var user = _fixture.Create<User>();
            user.IsActive.Should().BeTrue();

            // Act
            user.Deactivate();

            // Assert
            user.IsActive.Should().BeFalse();
            user.UpdatedAt.Should().NotBeNull();
        }

        #endregion

        #region UpdateLastLogin

        [Fact]
        public async Task UpdateLastLogin_ShouldSetLastLoginAt()
        {
            // Arrange
            var user = _fixture.Create<User>();
            user.LastLoginAt.Should().BeNull();

            // Act
            user.UpdateLastLogin();

            // Assert
            user.LastLoginAt.Should().NotBeNull();
            user.LastLoginAt.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        #endregion
    }
}
