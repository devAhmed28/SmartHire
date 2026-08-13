using AutoFixture;
using FluentAssertions;
using Moq;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Application.Features.Jobs.Commands.UpdateJob;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Application.Commands
{
    public class UpdateJobCommandHandlerTests : TestBase
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdateJobCommandHandler _handler;

        public UpdateJobCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var jobRepoMock = new Mock<IJobRepository>();
            var companyRepoMock = new Mock<ICompanyRepository>();
            var skillRepoMock = new Mock<ISkillRepository>();

            _unitOfWorkMock.Setup(u => u.Jobs).Returns(jobRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Companies).Returns(companyRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Skills).Returns(skillRepoMock.Object);

            _handler = new UpdateJobCommandHandler(_unitOfWorkMock.Object);
        }

        #region UpdateJob - Success

        [Fact]
        public async Task Handle_ValidUpdate_ShouldReturnSuccess()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var company = new Company(user.Id, "TechCorp", "Description", "Tech", CompanySize.Medium);

            var job = new Job(
                company.Id,
                "Old Title",
                "Old Description",
                "Old Responsibilities",
                "Old Requirements",
                50000,
                70000,
                "Old Location",
                2,
                Currency.EGP,
                JobType.PartTime,
                WorkMode.Remote,
                DateTime.UtcNow.AddDays(15)
            );

            var command = _fixture.Build<UpdateJobCommand>()
                .With(c => c.JobId, job.Id)
                .With(c => c.CompanyId, company.Id)
                .With(c => c.Title, "Senior Software Engineer")
                .With(c => c.Description, "New Description")
                .With(c => c.SalaryMin, 90000)
                .With(c => c.SalaryMax, 130000)
                .With(c => c.Location, "New Location")
                .With(c => c.Vacancies, 5)
                .With(c => c.Currency, Currency.USD)
                .With(c => c.JobType, JobType.FullTime)
                .With(c => c.WorkMode, WorkMode.OnSite)
                .With(c => c.ExpirationDate, DateTime.UtcNow.AddDays(60))
                .Create();

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(job.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(job);

            // Skills (if any)
            _unitOfWorkMock.Setup(u => u.Skills.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Skill?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Title.Should().Be("Senior Software Engineer");
            result.Value.Description.Should().Be("New Description");
            result.Value.SalaryMin.Should().Be(90000);
            result.Value.SalaryMax.Should().Be(130000);
            result.Value.Location.Should().Be("New Location");
            result.Value.Vacancies.Should().Be(5);
            result.Value.Currency.Should().Be(Currency.USD);
            result.Value.JobType.Should().Be(JobType.FullTime);
            result.Value.WorkMode.Should().Be(WorkMode.OnSite);

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        #endregion

        #region UpdateJob - Failures

        [Fact]
        public async Task Handle_JobNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var command = _fixture.Build<UpdateJobCommand>()
                .With(c => c.JobId, Guid.NewGuid())
                .Create();

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
        public async Task Handle_WrongCompany_ShouldReturnForbidden()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var company = new Company(user.Id, "TechCorp", "Description", "Tech", CompanySize.Medium);

            var job = new Job(
                company.Id,
                "Title",
                "Description",
                "Responsibilities",
                "Requirements",
                50000,
                70000,
                "Location",
                2,
                Currency.USD,
                JobType.FullTime,
                WorkMode.Hybrid,
                DateTime.UtcNow.AddDays(30)
            );

            var command = _fixture.Build<UpdateJobCommand>()
                .With(c => c.JobId, job.Id)
                .With(c => c.CompanyId, Guid.NewGuid()) // Wrong company ID
                .Create();

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(job.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(job);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("Forbidden");
            result.Error.Message.Should().Contain("permission");
        }

        #endregion
    }
}
