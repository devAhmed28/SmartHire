using FluentAssertions;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Infrastructure.Persistence.Repositories;
using SmartHire.Tests.Fixtures;

namespace SmartHire.Tests.IntegrationTests.Repositories
{
    public class UserRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public UserRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        #region AddAsync

        [Fact]
        public async Task AddAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var repository = new UserRepository(_fixture.Context);
            var user = new User(
                "John",
                "Doe",
                "john@test.com",
                "hashed_password",
                "+1234567890",
                UserRole.Candidate
            );

            // Act
            await repository.AddAsync(user);
            await _fixture.Context.SaveChangesAsync();

            // Assert
            var savedUser = await repository.GetByEmailAsync("john@test.com");
            savedUser.Should().NotBeNull();
            savedUser.Email.Should().Be("john@test.com");
            savedUser.FirstName.Should().Be("John");
            savedUser.LastName.Should().Be("Doe");
            savedUser.Role.Should().Be(UserRole.Candidate);
        }

        #endregion

        #region GetByEmailAsync

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnUser()
        {
            // Arrange
            var repository = new UserRepository(_fixture.Context);
            var user = new User(
                "Jane",
                "Smith",
                "jane@test.com",
                "hashed_password",
                "+9876543210",
                UserRole.Company
            );
            await repository.AddAsync(user);
            await _fixture.Context.SaveChangesAsync();

            // Act
            var result = await repository.GetByEmailAsync("jane@test.com");

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be("jane@test.com");
            result.Role.Should().Be(UserRole.Company);
        }

        #endregion

        #region IsEmailUniqueAsync

        [Fact]
        public async Task IsEmailUniqueAsync_ShouldReturnTrueForUniqueEmail()
        {
            // Arrange
            var repository = new UserRepository(_fixture.Context);
            var user = new User(
                "Test",
                "User",
                "unique@test.com",
                "hash",
                "1234567890",
                UserRole.Candidate
            );
            await repository.AddAsync(user);
            await _fixture.Context.SaveChangesAsync();

            // Act
            var result = await repository.IsEmailUniqueAsync("unique@test.com");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsEmailUniqueAsync_ShouldReturnFalseForExistingEmail()
        {
            // Arrange
            var repository = new UserRepository(_fixture.Context);

            // Act
            var result = await repository.IsEmailUniqueAsync("nonexistent@test.com");

            // Assert
            result.Should().BeTrue();
        }

        #endregion
    }
}
