using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Infrastructure.Persistence.Context;
using SmartHire.Infrastructure.Services;

namespace SmartHire.Infrastructure.Persistence.Seed
{
    public static class SeedData
    {
        public static async Task SeedAdminUserAsync(ApplicationDbContext context)
        {
            var adminExists = context.Users.Any(u => u.Role == UserRole.Admin);

            if (adminExists)
                return;

            var passwordHasher = new PasswordHasher();
            var hashedPassword = passwordHasher.HashPassword("Admin@123");

            var admin = new User(
                "Admin",
                "SmartHire",
                "admin@smarthire.com",
                hashedPassword,
                "+201114893385",
                UserRole.Admin
            );

            admin.ConfirmEmail();

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}
