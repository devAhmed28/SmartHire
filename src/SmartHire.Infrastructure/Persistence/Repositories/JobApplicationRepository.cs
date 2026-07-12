using Microsoft.EntityFrameworkCore;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Infrastructure.Persistence.Context;
namespace SmartHire.Infrastructure.Persistence.Repositories
{
    public class JobApplicationRepository : BaseRepository<JobApplication>, IJobApplicationRepository
    {
        public JobApplicationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<JobApplication>> GetByJobIdAsync(Guid jobId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(ja => ja.JobId == jobId)
                .OrderByDescending(ja => ja.AppliedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<JobApplication>> GetByCandidateProfileIdAsync(Guid candidateProfileId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(ja => ja.CandidateProfileId ==  candidateProfileId)
                .OrderByDescending(ja => ja.AppliedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<JobApplication?> GetWithInterviewAndOfferAsync(Guid applicationId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(ja => ja.Interviews)
                .Include(ja => ja.Offer)
                .FirstOrDefaultAsync(ja => ja.Id == applicationId, cancellationToken);
        }

        public async Task<IReadOnlyList<JobApplication>> GetByStatusAsync(ApplicationStatus status, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(ja => ja.Status == status)
                .OrderByDescending(ja => ja.AppliedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasAppliedAsync(Guid jobId, Guid candidateProfileId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(ja => ja.JobId == jobId && ja.CandidateProfileId == candidateProfileId, cancellationToken);
        }

        
        public async Task<IReadOnlyList<JobApplication>> GetCompanyApplicationsAsync(Guid companyId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(ja => ja.Job.CompanyId == companyId)
                .Include(ja => ja.Job)
                .Include(ja => ja.CandidateProfile)
                .ThenInclude(cp => cp.User)
                .OrderByDescending(ja => ja.AppliedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
