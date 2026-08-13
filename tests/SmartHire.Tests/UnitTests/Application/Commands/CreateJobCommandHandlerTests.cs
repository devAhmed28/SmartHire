using AutoFixture;
using FluentAssertions;
using Moq;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Application.Features.Jobs.Commands.CreateJob;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Application.Commands
{
    public class CreateJobCommandHandlerTests : TestBase
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateJobCommandHandler _handler;

        public CreateJobCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var companyRepoMock = new Mock<ICompanyRepository>();
            var jobRepoMock = new Mock<IJobRepository>();
            var skillRepoMock = new Mock<ISkillRepository>();

            _unitOfWorkMock.Setup(u => u.Companies).Returns(companyRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Jobs).Returns(jobRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Skills).Returns(skillRepoMock.Object);

            _handler = new CreateJobCommandHandler(_unitOfWorkMock.Object);
        }

        #region CreateJob - Success

        [Fact]
        public async Task Handle_ValidJobCreation_ShouldReturnSuccess()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var company = new Company(
                user.Id,
                "TechCorp",
                "Description",
                "Technology",
                CompanySize.Medium
            );

            var command = _fixture.Build<CreateJobCommand>()
                .With(c => c.CompanyId, company.Id)
                .With(c => c.Title, "Software Engineer")
                .With(c => c.Description, "We are looking for a Software Engineer")
                .With(c => c.SalaryMin, 70000)
                .With(c => c.SalaryMax, 100000)
                .With(c => c.Location, "New York")
                .With(c => c.Vacancies, 3)
                .With(c => c.Currency, Currency.USD)
                .With(c => c.JobType, JobType.FullTime)
                .With(c => c.WorkMode, WorkMode.Hybrid)
                .With(c => c.ExpirationDate, DateTime.UtcNow.AddDays(30))
                .Create();

            _unitOfWorkMock.Setup(u => u.Companies.GetByIdAsync(command.CompanyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            _unitOfWorkMock.Setup(u => u.Skills.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Skill?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Title.Should().Be("Software Engineer");
            result.Value.CompanyName.Should().Be("TechCorp");
            result.Value.SalaryMin.Should().Be(70000);
            result.Value.SalaryMax.Should().Be(100000);

            _unitOfWorkMock.Verify(u => u.Jobs.AddAsync(It.IsAny<Job>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        #endregion

        #region CreateJob - Failures

        [Fact]
        public async Task Handle_CompanyNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var command = _fixture.Build<CreateJobCommand>()
                .With(c => c.CompanyId, Guid.NewGuid())
                .Create();

            _unitOfWorkMock.Setup(u => u.Companies.GetByIdAsync(command.CompanyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Company?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("NotFound");
            result.Error.Message.Should().Contain("Company");
        }

        [Fact]
        public async Task Handle_WithSkills_ShouldAddJobSkills()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var company = new Company(
                user.Id,
                "TechCorp",
                "Description",
                "Technology",
                CompanySize.Medium
            );

            var skillId1 = Guid.NewGuid();
            var skillId2 = Guid.NewGuid();

            var command = _fixture.Build<CreateJobCommand>()
                .With(c => c.CompanyId, company.Id)
                .With(c => c.Title, "Software Engineer")
                .With(c => c.Description, "We are looking for a Software Engineer")
                .With(c => c.SkillIds, new List<Guid> { skillId1, skillId2 })
                .Create();

            _unitOfWorkMock.Setup(u => u.Companies.GetByIdAsync(command.CompanyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            var skill1 = new Skill("C#");
            var skill2 = new Skill("JavaScript");

            _unitOfWorkMock.Setup(u => u.Skills.GetByIdAsync(skillId1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(skill1);
            _unitOfWorkMock.Setup(u => u.Skills.GetByIdAsync(skillId2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(skill2);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _unitOfWorkMock.Verify(u => u.Jobs.AddAsync(It.IsAny<Job>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        #endregion
    }
}
