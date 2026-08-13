using FluentAssertions;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Infrastructure.Persistence.Repositories;
using SmartHire.Tests.Fixtures;

namespace SmartHire.Tests.IntegrationTests.Repositories
{
    public class JobRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public JobRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        #region AddAsync

        [Fact]
        public async Task AddAsync_ShouldAddJobToDatabase()
        {
            // Arrange
            var userRepo = new UserRepository(_fixture.Context);
            var user = new User("Company", "User", "company@test.com", "hash", "123", UserRole.Company);
            await userRepo.AddAsync(user);
            await _fixture.Context.SaveChangesAsync();

            var companyRepo = new CompanyRepository(_fixture.Context);
            var company = new Company(user.Id, "TechCorp", "Description", "Tech", CompanySize.Medium);
            await companyRepo.AddAsync(company);
            await _fixture.Context.SaveChangesAsync();

            var jobRepo = new JobRepository(_fixture.Context);
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

            // Act
            await jobRepo.AddAsync(job);
            await _fixture.Context.SaveChangesAsync();

            // Assert
            var savedJob = await jobRepo.GetByIdAsync(job.Id);
            savedJob.Should().NotBeNull();
            savedJob.Title.Should().Be("Software Engineer");
            savedJob.CompanyId.Should().Be(company.Id);
            savedJob.JobStatus.Should().Be(JobStatus.Draft);
        }

        #endregion

        #region GetByCompanyIdAsync

        [Fact]
        public async Task GetByCompanyIdAsync_ShouldReturnJobsForCompany()
        {
            // Arrange
            var userRepo = new UserRepository(_fixture.Context);
            var user = new User("Company2", "User", "company2@test.com", "hash", "456", UserRole.Company);
            await userRepo.AddAsync(user);
            await _fixture.Context.SaveChangesAsync();

            var companyRepo = new CompanyRepository(_fixture.Context);
            var company = new Company(user.Id, "TechCorp2", "Description", "Tech", CompanySize.Medium);
            await companyRepo.AddAsync(company);
            await _fixture.Context.SaveChangesAsync();

            var jobRepo = new JobRepository(_fixture.Context);
            var job1 = new Job(company.Id, "Job1", "Desc", "Resp", "Req", 50000, 70000, "NY", 2, Currency.USD, JobType.FullTime, WorkMode.Remote, DateTime.UtcNow.AddDays(30));
            var job2 = new Job(company.Id, "Job2", "Desc", "Resp", "Req", 60000, 80000, "NY", 3, Currency.USD, JobType.PartTime, WorkMode.Hybrid, DateTime.UtcNow.AddDays(30));
            await jobRepo.AddAsync(job1);
            await jobRepo.AddAsync(job2);
            await _fixture.Context.SaveChangesAsync();

            // Act
            var result = await jobRepo.GetByCompanyIdAsync(company.Id);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(j => j.Title == "Job1");
            result.Should().Contain(j => j.Title == "Job2");
        }

        #endregion
    }
}
