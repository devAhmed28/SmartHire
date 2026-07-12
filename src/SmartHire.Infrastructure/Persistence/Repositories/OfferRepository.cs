using Microsoft.EntityFrameworkCore;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Domain.Entities;
using SmartHire.Infrastructure.Persistence.Context;

namespace SmartHire.Infrastructure.Persistence.Repositories;

public class OfferRepository : BaseRepository<Offer>, IOfferRepository
{
    public OfferRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Offer?> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(o => o.JobApplicationId == applicationId, cancellationToken);
    }

    public async Task<IReadOnlyList<Offer>> GetByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(o => o.JobApplication.Job.CompanyId == companyId)
            .Include(o => o.JobApplication)
                .ThenInclude(ja => ja.Job)
            .Include(o => o.JobApplication)
                .ThenInclude(ja => ja.CandidateProfile)
                    .ThenInclude(cp => cp.User)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Offer>> GetByCandidateIdAsync(Guid candidateProfileId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(o => o.JobApplication.CandidateProfileId == candidateProfileId)
            .Include(o => o.JobApplication)
                .ThenInclude(ja => ja.Job)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Offer>> GetPendingOffersAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        return await _dbSet
            .Where(o => o.IsAccepted == false && o.ExpirationDate > now)
            .Include(o => o.JobApplication)
                .ThenInclude(ja => ja.Job)
            .Include(o => o.JobApplication)
                .ThenInclude(ja => ja.CandidateProfile)
                    .ThenInclude(cp => cp.User)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Offer>> GetExpiredOffersAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        return await _dbSet
            .Where(o => o.ExpirationDate <= now && o.IsAccepted == false)
            .ToListAsync(cancellationToken);
    }
}