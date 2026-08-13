using Microsoft.EntityFrameworkCore;
using SmartHire.Infrastructure.Persistence.Context;

namespace SmartHire.Tests.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        public ApplicationDbContext Context { get; private set; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"SmartHireTestDb_{Guid.NewGuid()}")
                .Options;

            Context = new ApplicationDbContext(options);
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
