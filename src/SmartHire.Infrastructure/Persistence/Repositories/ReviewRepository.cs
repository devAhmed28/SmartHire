using Microsoft.EntityFrameworkCore;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Domain.Entities;
using SmartHire.Infrastructure.Persistence.Context;

namespace SmartHire.Infrastructure.Persistence.Repositories;

public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Review>> GetByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.CompanyId == companyId)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Review>> GetByUserIdWithDetailsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Company)
            .Include(r => r.User)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Review?> GetByCompanyAndUserAsync(Guid companyId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.CompanyId == companyId && r.UserId == userId, cancellationToken);
    }

    public async Task<double> GetAverageRatingAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var ratings = await _dbSet
            .Where(r => r.CompanyId == companyId)
            .Select(r => r.Rating)
            .ToListAsync(cancellationToken);

        if (ratings.Count == 0)
            return 0;

        return ratings.Average();
    }

    public async Task<IReadOnlyList<Review>> GetWithCompanyDetailsAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Company)
            .Include(r => r.User)
            .Where(r => r.CompanyId == companyId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}