using Microsoft.EntityFrameworkCore;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Domain.Entities;
using SmartHire.Infrastructure.Persistence.Context;

namespace SmartHire.Infrastructure.Persistence.Repositories;

public class SavedJobRepository : BaseRepository<SavedJob>, ISavedJobRepository
{
    public SavedJobRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<SavedJob>> GetByCandidateProfileAsync(Guid candidateProfileId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(sj => sj.CandidateProfileId == candidateProfileId)
            .OrderByDescending(sj => sj.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<SavedJob?> GetByCandidateAndJobAsync(Guid candidateProfileId, Guid jobId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(sj => sj.CandidateProfileId == candidateProfileId && sj.JobId == jobId, cancellationToken);
    }

    public async Task<bool> IsJobSavedAsync(Guid candidateProfileId, Guid jobId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(sj => sj.CandidateProfileId == candidateProfileId && sj.JobId == jobId, cancellationToken);
    }

    public async Task<IReadOnlyList<SavedJob>> GetWithJobDetailsAsync(Guid candidateProfileId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(sj => sj.CandidateProfileId == candidateProfileId)
            .Include(sj => sj.Job)
                .ThenInclude(j => j.Company)
            .OrderByDescending(sj => sj.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}