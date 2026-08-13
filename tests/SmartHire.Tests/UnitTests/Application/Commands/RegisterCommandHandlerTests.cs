using AutoFixture;
using FluentAssertions;
using Moq;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Application.DTOs.Auth;
using SmartHire.Application.Features.Auth.Commands.Register;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Application.Commands
{
    public class RegisterCommandHandlerTests : TestBase
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _tokenServiceMock = new Mock<ITokenService>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var userRepoMock = new Mock<IUserRepository>();
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            var companyRepoMock = new Mock<ICompanyRepository>();
            _unitOfWorkMock.Setup(u => u.Companies).Returns(companyRepoMock.Object);

            var candidateRepoMock = new Mock<ICandidateProfileRepository>();
            _unitOfWorkMock.Setup(u => u.CandidateProfiles).Returns(candidateRepoMock.Object);

            var refreshTokenRepoMock = new Mock<IRefreshTokenRepository>();
            _unitOfWorkMock.Setup(u => u.RefreshTokens).Returns(refreshTokenRepoMock.Object);

            _dateTimeProviderMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

            _handler = new RegisterCommandHandler(
                _unitOfWorkMock.Object,
                _passwordHasherMock.Object,
                _tokenServiceMock.Object,
                _dateTimeProviderMock.Object
            );
        }

        #region Register - Candidate

        [Fact]
        public async Task Handle_ValidCandidateRegistration_ShouldReturnSuccess()
        {
            // Arrange
            var command = _fixture.Build<RegisterCommand>()
                .With(c => c.Email, "candidate@test.com")
                .With(c => c.Password, "Password123!")
                .With(c => c.AccountType, AccountType.Candidate)
                .With(c => c.Candidate, new CandidateInfo
                {
                    FirstName = "John",
                    LastName = "Doe"
                })
                .Create();

            // Email is unique
            _unitOfWorkMock.Setup(u => u.Users.IsEmailUniqueAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Phone is unique
            _unitOfWorkMock.Setup(u => u.Users.IsPhoneNumberUniqueAsync(command.PhoneNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Password hashing
            _passwordHasherMock.Setup(p => p.HashPassword(command.Password))
                .Returns("hashed_password");

            // Token generation
            _tokenServiceMock.Setup(t => t.GenerateAccessToken(It.IsAny<User>()))
                .Returns("access_token");
            _tokenServiceMock.Setup(t => t.GenerateRefreshToken())
                .Returns("refresh_token");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Email.Should().Be(command.Email);
            result.Value.AccessToken.Should().Be("access_token");
            result.Value.RefreshToken.Should().Be("refresh_token");

            // User added
            _unitOfWorkMock.Verify(u => u.Users.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);

            // CandidateProfile added
            _unitOfWorkMock.Verify(u => u.CandidateProfiles.AddAsync(It.IsAny<CandidateProfile>(), It.IsAny<CancellationToken>()), Times.Once);

            // RefreshToken added
            _unitOfWorkMock.Verify(u => u.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Once);

            // SaveChanges called
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        #endregion

        #region Register - Company

        [Fact]
        public async Task Handle_ValidCompanyRegistration_ShouldReturnSuccess()
        {
            // Arrange
            var command = _fixture.Build<RegisterCommand>()
                .With(c => c.Email, "company@test.com")
                .With(c => c.Password, "Password123!")
                .With(c => c.AccountType, AccountType.Company)
                .With(c => c.Company, new CompanyInfo
                {
                    CompanyName = "TechCorp",
                    Description = "We build software",
                    Industry = "Technology",
                    CompanySize = CompanySize.Medium
                })
                .Create();

            // email is unique
            _unitOfWorkMock.Setup(u => u.Users.IsEmailUniqueAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // phone is unique
            _unitOfWorkMock.Setup(u => u.Users.IsPhoneNumberUniqueAsync(command.PhoneNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // password hashing
            _passwordHasherMock.Setup(p => p.HashPassword(command.Password))
                .Returns("hashed_password");

            // token generation
            _tokenServiceMock.Setup(t => t.GenerateAccessToken(It.IsAny<User>()))
                .Returns("access_token");
            _tokenServiceMock.Setup(t => t.GenerateRefreshToken())
                .Returns("refresh_token");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Email.Should().Be(command.Email);

            // Verify User added
            _unitOfWorkMock.Verify(u => u.Users.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);

            // Verify Company added
            _unitOfWorkMock.Verify(u => u.Companies.AddAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Once);

            // Verify RefreshToken added!
            _unitOfWorkMock.Verify(u => u.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Register - Validation Errors

        [Fact]
        public async Task Handle_DuplicateEmail_ShouldReturnConflict()
        {
            // Arrange
            var command = _fixture.Build<RegisterCommand>()
                .With(c => c.Email, "existing@test.com")
                .With(c => c.AccountType, AccountType.Candidate)
                .Create();

            // Email not unique
            _unitOfWorkMock.Setup(u => u.Users.IsEmailUniqueAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("Conflict");
            result.Error.Message.Should().Contain("already registered");
        }

        [Fact]
        public async Task Handle_MissingCandidateDetails_ShouldReturnValidationError()
        {
            // Arrange
            var command = _fixture.Build<RegisterCommand>()
                .With(c => c.AccountType, AccountType.Candidate)
                .With(c => c.Candidate, (CandidateInfo?)null)
                .Create();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("Validation.Error");
            result.Error.Message.Should().Contain("Candidate details are required");
        }

        [Fact]
        public async Task Handle_MissingCompanyDetails_ShouldReturnValidationError()
        {
            // Arrange
            var command = _fixture.Build<RegisterCommand>()
                .With(c => c.AccountType, AccountType.Company)
                .With(c => c.Company, (CompanyInfo?)null)
                .Create();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("Validation.Error");
            result.Error.Message.Should().Contain("Company details are required");
        }

        #endregion
    }
}
