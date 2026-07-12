using Microsoft.EntityFrameworkCore;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Domain.Entities;
using SmartHire.Infrastructure.Persistence.Context;
namespace SmartHire.Infrastructure.Persistence.Repositories
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Company?> GetCompanyWithJobsAsync(Guid companyId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.Jobs)
                .FirstOrDefaultAsync(c => c.Id == companyId, cancellationToken);
        }

        public async Task<Company?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
        }

        public async Task<Company?> GetCompanyWithDetailsAsync(Guid companyId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.Jobs)
                .Include(c => c.Reviews)
                .FirstOrDefaultAsync(c => c.Id == companyId, cancellationToken);
        }

        public async Task<bool> IsCompanyNameUniqueAsync(string companyName, CancellationToken cancellationToken = default)
        {
            return !await _dbSet
                .AnyAsync(c => c.CompanyName == companyName, cancellationToken);
        }
    }
}
