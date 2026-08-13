using AutoFixture;
using FluentAssertions;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Domain
{
    public class JobTests : TestBase
    {
        #region CreateJob

        [Fact]
        public async Task CreateJob_WithValidData_ShouldSetPropertiesCorrectly()
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

            var expirationDate = DateTime.UtcNow.AddDays(30);

            // Act
            var job = new Job(
                company.Id,
                "Software Engineer",
                "We are looking for a talented Software Engineer",
                "- Design and develop applications\n- Write clean code",
                "- 3+ years experience\n- C# and .NET",
                70000,
                100000,
                "New York, NY",
                3,
                Currency.USD,
                JobType.FullTime,
                WorkMode.Hybrid,
                expirationDate
            );

            // Assert
            job.Id.Should().NotBe(Guid.Empty);
            job.CompanyId.Should().Be(company.Id);
            job.Title.Should().Be("Software Engineer");
            job.Description.Should().Be("We are looking for a talented Software Engineer");
            job.Responsibilities.Should().Contain("Design and develop applications");
            job.Requirements.Should().Contain("C# and .NET");
            job.SalaryMin.Should().Be(70000);
            job.SalaryMax.Should().Be(100000);
            job.Location.Should().Be("New York, NY");
            job.Vacancies.Should().Be(3);
            job.Currency.Should().Be(Currency.USD);
            job.JobType.Should().Be(JobType.FullTime);
            job.WorkMode.Should().Be(WorkMode.Hybrid);
            job.JobStatus.Should().Be(JobStatus.Draft);
            job.ExpirationDate.Should().Be(expirationDate);
            job.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        #endregion

        #region Publish

        [Fact]
        public async Task Publish_WhenJobIsDraft_ShouldSetStatusToPublished()
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

            var job = new Job(
                company.Id,
                "Software Engineer",
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

            job.JobStatus.Should().Be(JobStatus.Draft);

            // Act
            job.Publish();

            // Assert
            job.JobStatus.Should().Be(JobStatus.Published);
            job.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public async Task Publish_WhenJobIsAlreadyPublished_ShouldRemainPublished()
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

            var job = new Job(
                company.Id,
                "Software Engineer",
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

            job.Publish();
            job.JobStatus.Should().Be(JobStatus.Published);

            // Act
            job.Publish();

            // Assert
            job.JobStatus.Should().Be(JobStatus.Published);
        }

        #endregion

        #region Close

        [Fact]
        public async Task Close_WhenJobIsPublished_ShouldSetStatusToClosed()
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

            var job = new Job(
                company.Id,
                "Software Engineer",
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

            job.Publish();
            job.JobStatus.Should().Be(JobStatus.Published);

            // Act
            job.Close();

            // Assert
            job.JobStatus.Should().Be(JobStatus.Closed);
            job.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public async Task Close_WhenJobIsAlreadyClosed_ShouldRemainClosed()
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

            var job = new Job(
                company.Id,
                "Software Engineer",
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

            job.Publish();
            job.Close();
            job.JobStatus.Should().Be(JobStatus.Closed);

            // Act
            job.Close();

            // Assert
            job.JobStatus.Should().Be(JobStatus.Closed);
        }

        [Fact]
        public async Task Close_WhenJobIsDraft_ShouldNotChangeStatus()
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

            var job = new Job(
                company.Id,
                "Software Engineer",
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

            job.JobStatus.Should().Be(JobStatus.Draft);

            // Act
            job.Close();

            // Assert
            job.JobStatus.Should().Be(JobStatus.Draft);
        }

        #endregion

        #region IsExpired

        [Fact]
        public async Task IsExpired_WhenExpirationDateIsPast_ShouldReturnTrue()
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

            var job = new Job(
                company.Id,
                "Software Engineer",
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
                DateTime.UtcNow.AddDays(-5)
            );

            // Act
            var result = job.IsExpired();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsExpired_WhenExpirationDateIsInFuture_ShouldReturnFalse()
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

            var job = new Job(
                company.Id,
                "Software Engineer",
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

            // Act
            var result = job.IsExpired();

            // Assert
            result.Should().BeFalse();
        }

        #endregion
    }
}
