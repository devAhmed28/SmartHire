using AutoFixture;
using FluentAssertions;
using Moq;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Application.Features.Jobs.Queries.GetJob;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Application.Queries
{
    public class GetJobQueryHandlerTests : TestBase
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetJobQueryHandler _handler;

        public GetJobQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var jobRepoMock = new Mock<IJobRepository>();
            var companyRepoMock = new Mock<ICompanyRepository>();

            _unitOfWorkMock.Setup(u => u.Jobs).Returns(jobRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Companies).Returns(companyRepoMock.Object);

            _handler = new GetJobQueryHandler(_unitOfWorkMock.Object);
        }

        #region GetJob - Success

        [Fact]
        public async Task Handle_ValidJobId_ShouldReturnJob()
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

            var query = new GetJobQuery
            {
                JobId = job.Id
            };

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(job.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(job);

            _unitOfWorkMock.Setup(u => u.Companies.GetByIdAsync(job.CompanyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(job.Id);
            result.Value.Title.Should().Be(job.Title);
            result.Value.CompanyName.Should().Be(company.CompanyName);
            result.Value.SalaryMin.Should().Be(job.SalaryMin);
            result.Value.SalaryMax.Should().Be(job.SalaryMax);
            result.Value.Location.Should().Be(job.Location);
            result.Value.JobType.Should().Be(job.JobType);
            result.Value.WorkMode.Should().Be(job.WorkMode);
            result.Value.JobStatus.Should().Be(job.JobStatus);
        }

        #endregion

        #region GetJob - Failures

        [Fact]
        public async Task Handle_JobNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var query = new GetJobQuery
            {
                JobId = Guid.NewGuid()
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

        #endregion
    }
}
