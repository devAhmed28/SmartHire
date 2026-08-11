using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using SmartHire.Infrastructure.Persistence.Context;
using SmartHire.Tests.Customizations;

namespace SmartHire.Tests.Helpers
{
    public class TestBase : IDisposable
    {
        protected readonly IFixture _fixture;
        protected readonly ApplicationDbContext _dbContext;

        public TestBase()
        {
            _fixture = new Fixture();
            
            _fixture.Customize(new UserCustomization());
            _fixture.Customize(new CompanyCustomization());
            
            // for prevent infinite loop..
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"SmartHireTestDb_{Guid.NewGuid()}")
            .Options;

            _dbContext = new ApplicationDbContext(options);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
