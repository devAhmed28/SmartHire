using Microsoft.EntityFrameworkCore;
using SmartHire.Infrastructure.Persistence.Context;

namespace SmartHire.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"SmartHireTestDb_{Guid.NewGuid()}")
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
