using AutoFixture;
using FluentAssertions;
using Moq;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Application.Features.Jobs.Commands.DeleteJob;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Application.Commands
{
    public class DeleteJobCommandHandlerTests : TestBase
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly DeleteJobCommandHandler _handler;

        public DeleteJobCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var jobRepoMock = new Mock<IJobRepository>();
            _unitOfWorkMock.Setup(u => u.Jobs).Returns(jobRepoMock.Object);

            _handler = new DeleteJobCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ValidDelete_ShouldReturnSuccess()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var company = new Company(user.Id, "TechCorp", "Description", "Technology", CompanySize.Medium);

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

            var command = new DeleteJobCommand
            {
                JobId = job.Id,
                CompanyId = company.Id
            };

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(job.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(job);

            _unitOfWorkMock.Setup(u => u.Jobs.DeleteAsync(job))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _unitOfWorkMock.Verify(u => u.Jobs.DeleteAsync(job), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_JobNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var command = new DeleteJobCommand
            {
                JobId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid()
            };

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(command.JobId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Job?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Code.Should().Be("NotFound");
        }

        [Fact]
        public async Task Handle_WrongCompany_ShouldReturnForbidden()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var company = new Company(user.Id, "TechCorp", "Description", "Technology", CompanySize.Medium);

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

            var command = new DeleteJobCommand
            {
                JobId = job.Id,
                CompanyId = Guid.NewGuid()
            };

            _unitOfWorkMock.Setup(u => u.Jobs.GetByIdAsync(job.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(job);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Code.Should().Be("Forbidden");
        }
    }
}