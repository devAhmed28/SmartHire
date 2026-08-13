using AutoFixture;
using FluentAssertions;
using Moq;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Application.Features.Applications.Queries.GetMyApplications;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Application.Queries
{
    public class GetMyApplicationsQueryHandlerTests : TestBase
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetMyApplicationsQueryHandler _handler;

        public GetMyApplicationsQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var applicationRepoMock = new Mock<IJobApplicationRepository>();
            var jobRepoMock = new Mock<IJobRepository>();
            var companyRepoMock = new Mock<ICompanyRepository>();
            var candidateRepoMock = new Mock<ICandidateProfileRepository>();
            var userRepoMock = new Mock<IUserRepository>();

            _unitOfWorkMock.Setup(u => u.JobApplications).Returns(applicationRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Jobs).Returns(jobRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Companies).Returns(companyRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.CandidateProfiles).Returns(candidateRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Users).Returns(userRepoMock.Object);

            _handler = new GetMyApplicationsQueryHandler(_unitOfWorkMock.Object);
        }

        #region GetMyApplications - Success

        [Fact]
        public async Task Handle_ValidCandidateId_ShouldReturnApplications()
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

            var query = new GetMyApplicationsQuery
            {
                CandidateId = candidate.Id
            };

            _unitOfWorkMock.Setup(u => u.JobApplications.GetByCandidateProfileIdAsync(candidate.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(applications);

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(job.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(job);

            _unitOfWorkMock.Setup(u => u.Companies.GetByIdAsync(company.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            _unitOfWorkMock.Setup(u => u.CandidateProfiles.GetByIdAsync(candidate.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidate);

            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(1);
            result.Value[0].JobId.Should().Be(job.Id);
            result.Value[0].JobTitle.Should().Be(job.Title);
            result.Value[0].CompanyName.Should().Be(company.CompanyName);
            result.Value[0].Status.Should().Be(application.Status);
        }

        #endregion

        #region GetMyApplications - Empty

        [Fact]
        public async Task Handle_NoApplications_ShouldReturnEmptyList()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var candidate = new CandidateProfile(user.Id, "Bio", "Position", "Location", 5, 80000);

            var query = new GetMyApplicationsQuery
            {
                CandidateId = candidate.Id
            };

            _unitOfWorkMock.Setup(u => u.JobApplications.GetByCandidateProfileIdAsync(candidate.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<JobApplication>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEmpty();
        }

        #endregion
    }
}