using AutoFixture;
using FluentAssertions;
using Moq;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Application.Features.Applications.Queries.GetJobApplications;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Application.Queries
{
    public class GetJobApplicationsQueryHandlerTests : TestBase
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetJobApplicationsQueryHandler _handler;

        public GetJobApplicationsQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var jobRepoMock = new Mock<IJobRepository>();
            var applicationRepoMock = new Mock<IJobApplicationRepository>();
            var candidateRepoMock = new Mock<ICandidateProfileRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var companyRepoMock = new Mock<ICompanyRepository>();

            _unitOfWorkMock.Setup(u => u.Jobs).Returns(jobRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.JobApplications).Returns(applicationRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.CandidateProfiles).Returns(candidateRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Companies).Returns(companyRepoMock.Object);

            _handler = new GetJobApplicationsQueryHandler(_unitOfWorkMock.Object);
        }

        #region GetJobApplications - Success

        [Fact]
        public async Task Handle_ValidJobId_ShouldReturnApplications()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var candidate = new CandidateProfile(user.Id, "Bio", "Position", "Location", 5, 80000);
            var company = new Company(user.Id, "TechCorp", "Description", "Tech", CompanySize.Medium);

            var job = new Job(
                company.Id,
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

            var application = new JobApplication(
                candidate.Id,
                job.Id,
                "I am very interested"
            );

            var applications = new List<JobApplication> { application };

            var query = new GetJobApplicationsQuery
            {
                JobId = job.Id,
                CompanyId = company.Id
            };

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(job.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(job);

            _unitOfWorkMock.Setup(u => u.JobApplications.GetByJobIdAsync(job.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(applications);

            _unitOfWorkMock.Setup(u => u.CandidateProfiles.GetByIdAsync(candidate.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidate);

            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _unitOfWorkMock.Setup(u => u.Companies.GetByIdAsync(company.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(1);
            result.Value[0].JobId.Should().Be(job.Id);
            result.Value[0].JobTitle.Should().Be(job.Title);
            result.Value[0].CandidateName.Should().Be($"{user.FirstName} {user.LastName}");
            result.Value[0].Status.Should().Be(application.Status);
        }

        #endregion

        #region GetJobApplications - Failures

        [Fact]
        public async Task Handle_JobNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var query = new GetJobApplicationsQuery
            {
                JobId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid()
            };

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(query.JobId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Job?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("NotFound");
            result.Error.Message.Should().Contain("Job");
        }

        [Fact]
        public async Task Handle_WrongCompany_ShouldReturnForbidden()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var company = new Company(user.Id, "TechCorp", "Description", "Tech", CompanySize.Medium);

            var job = new Job(
                company.Id,
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

            var query = new GetJobApplicationsQuery
            {
                JobId = job.Id,
                CompanyId = Guid.NewGuid()
            };

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(job.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(job);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("Forbidden");
            result.Error.Message.Should().Contain("permission");
        }

        #endregion
    }
}
