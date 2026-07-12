using Microsoft.EntityFrameworkCore;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Infrastructure.Persistence.Context;
namespace SmartHire.Infrastructure.Persistence.Repositories
{
    public class InterviewRepository : BaseRepository<Interview>, IInterviewRepository
    {
        public InterviewRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Interview>> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(i => i.JobApplicationId == applicationId)
                .OrderBy(i => i.InterviewDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Interview>> GetByStatusAsync(InterviewStatus status, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(i => i.Status == status)
                .OrderBy(i => i.InterviewDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Interview>> GetUpcomingForCandidateAsync(Guid candidateProfileId, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            return await _dbSet
                .Where(i => i.JobApplication.CandidateProfileId == candidateProfileId
                    && i.Status == InterviewStatus.Scheduled
                    && i.InterviewDate > now)
                .Include(i => i.JobApplication)
                    .ThenInclude(ja => ja.Job)
                .OrderBy(i => i.InterviewDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Interview>> GetUpcomingForCompanyAsync(Guid companyId, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            return await _dbSet
                .Where(i => i.JobApplication.Job.CompanyId == companyId
                    && i.Status == InterviewStatus.Scheduled
                    && i.InterviewDate > now)
                .Include(i => i.JobApplication)
                    .ThenInclude(ja => ja.Job)
                .Include(i => i.JobApplication)
                    .ThenInclude(ja => ja.CandidateProfile)
                    .ThenInclude(cp => cp.User)
                .OrderBy(i => i.InterviewDate)
                .ToListAsync(cancellationToken);
        }
    }
}
