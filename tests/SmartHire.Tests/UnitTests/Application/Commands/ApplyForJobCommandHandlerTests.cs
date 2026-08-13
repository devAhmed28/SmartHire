using AutoFixture;
using FluentAssertions;
using Moq;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Application.Features.Applications.Commands.ApplyForJob;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Application.Commands
{
    public class ApplyForJobCommandHandlerTests : TestBase
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ApplyForJobCommandHandler _handler;

        public ApplyForJobCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            // Setup repositories
            var candidateRepoMock = new Mock<ICandidateProfileRepository>();
            var jobRepoMock = new Mock<IJobRepository>();
            var applicationRepoMock = new Mock<IJobApplicationRepository>();
            var companyRepoMock = new Mock<ICompanyRepository>();
            var userRepoMock = new Mock<IUserRepository>();

            _unitOfWorkMock.Setup(u => u.CandidateProfiles).Returns(candidateRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Jobs).Returns(jobRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.JobApplications).Returns(applicationRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Companies).Returns(companyRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            _handler = new ApplyForJobCommandHandler(_unitOfWorkMock.Object);
        }

        #region ApplyForJob - Success

        [Fact]
        public async Task Handle_ValidApplication_ShouldReturnSuccess()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var candidate = new CandidateProfile(
                user.Id,
                "Bio",
                "Position",
                "Location",
                5,
                80000
            );

            var job = new Job(
                Guid.NewGuid(),
                "Software Engineer",
                "Description",
                "Responsibilities",
                "Requirements",
                70000,
                100000,
                "New York",
                3,
                Currency.USD,
                JobType.FullTime,
                WorkMode.Hybrid,
                DateTime.UtcNow.AddDays(30)
            );
            job.Publish();

            var command = _fixture.Build<ApplyForJobCommand>()
                .With(c => c.CandidateId, user.Id)
                .With(c => c.JobId, job.Id)
                .With(c => c.CoverLetter, "I am very interested in this position")
                .Create();

            _unitOfWorkMock.Setup(u => u.CandidateProfiles.GetByUserIdAsync(command.CandidateId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidate);

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(command.JobId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(job);

            _unitOfWorkMock.Setup(u => u.JobApplications.HasAppliedAsync(command.JobId, candidate.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var company = new Company(user.Id, "TechCorp", "Description", "Tech", CompanySize.Medium);
            _unitOfWorkMock.Setup(u => u.Companies.GetByIdAsync(job.CompanyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(candidate.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.JobId.Should().Be(job.Id);
            result.Value.JobTitle.Should().Be(job.Title);
            result.Value.CoverLetter.Should().Be(command.CoverLetter);
            result.Value.Status.Should().Be(ApplicationStatus.Pending);

            // Verify
            _unitOfWorkMock.Verify(u => u.JobApplications.AddAsync(It.IsAny<JobApplication>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        #endregion

        #region ApplyForJob - Failures

        [Fact]
        public async Task Handle_CandidateNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var command = _fixture.Build<ApplyForJobCommand>()
                .With(c => c.CandidateId, Guid.NewGuid())
                .Create();

            _unitOfWorkMock.Setup(u => u.CandidateProfiles.GetByUserIdAsync(command.CandidateId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CandidateProfile?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("NotFound");
            result.Error.Message.Should().Contain("Candidate profile");
        }

        [Fact]
        public async Task Handle_JobNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var candidate = new CandidateProfile(user.Id, "Bio", "Position", "Location", 5, 80000);

            var command = _fixture.Build<ApplyForJobCommand>()
                .With(c => c.CandidateId, user.Id)
                .With(c => c.JobId, Guid.NewGuid())
                .Create();

            _unitOfWorkMock.Setup(u => u.CandidateProfiles.GetByUserIdAsync(command.CandidateId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidate);

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(command.JobId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Job?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("NotFound");
            result.Error.Message.Should().Contain("Job");
        }

        [Fact]
        public async Task Handle_JobNotPublished_ShouldReturnValidationError()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var candidate = new CandidateProfile(user.Id, "Bio", "Position", "Location", 5, 80000);

            var job = new Job(
                Guid.NewGuid(),
                "Software Engineer",
                "Description",
                "Responsibilities",
                "Requirements",
                70000,
                100000,
                "New York",
                3,
                Currency.USD,
                JobType.FullTime,
                WorkMode.Hybrid,
                DateTime.UtcNow.AddDays(30)
            );

            var command = _fixture.Build<ApplyForJobCommand>()
                .With(c => c.CandidateId, user.Id)
                .With(c => c.JobId, job.Id)
                .Create();

            _unitOfWorkMock.Setup(u => u.CandidateProfiles.GetByUserIdAsync(command.CandidateId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidate);

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(command.JobId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(job);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("Validation.Error");
            result.Error.Message.Should().Contain("not available for applications");
        }

        [Fact]
        public async Task Handle_JobExpired_ShouldReturnValidationError()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var candidate = new CandidateProfile(user.Id, "Bio", "Position", "Location", 5, 80000);

            var job = new Job(
                Guid.NewGuid(),
                "Software Engineer",
                "Description",
                "Responsibilities",
                "Requirements",
                70000,
                100000,
                "New York",
                3,
                Currency.USD,
                JobType.FullTime,
                WorkMode.Hybrid,
                DateTime.UtcNow.AddDays(-5)
            );
            job.Publish();

            var command = _fixture.Build<ApplyForJobCommand>()
                .With(c => c.CandidateId, user.Id)
                .With(c => c.JobId, job.Id)
                .Create();

            _unitOfWorkMock.Setup(u => u.CandidateProfiles.GetByUserIdAsync(command.CandidateId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidate);

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(command.JobId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(job);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("Validation.Error");
            result.Error.Message.Should().Contain("expired");
        }

        [Fact]
        public async Task Handle_AlreadyApplied_ShouldReturnConflict()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var candidate = new CandidateProfile(user.Id, "Bio", "Position", "Location", 5, 80000);

            var job = new Job(
                Guid.NewGuid(),
                "Software Engineer",
                "Description",
                "Responsibilities",
                "Requirements",
                70000,
                100000,
                "New York",
                3,
                Currency.USD,
                JobType.FullTime,
                WorkMode.Hybrid,
                DateTime.UtcNow.AddDays(30)
            );
            job.Publish();

            var command = _fixture.Build<ApplyForJobCommand>()
                .With(c => c.CandidateId, user.Id)
                .With(c => c.JobId, job.Id)
                .Create();

            _unitOfWorkMock.Setup(u => u.CandidateProfiles.GetByUserIdAsync(command.CandidateId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidate);

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(command.JobId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(job);

            _unitOfWorkMock.Setup(u => u.JobApplications.HasAppliedAsync(command.JobId, candidate.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("Conflict");
            result.Error.Message.Should().Contain("already applied");
        }

        #endregion
    }
}
