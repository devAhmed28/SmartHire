using Microsoft.EntityFrameworkCore;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Domain.Entities;
using SmartHire.Infrastructure.Persistence.Context;
namespace SmartHire.Infrastructure.Persistence.Repositories
{
    public class CandidateProfileRepository : BaseRepository<CandidateProfile>, ICandidateProfileRepository
    {
        public CandidateProfileRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<CandidateProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(cp => cp.UserId == userId, cancellationToken);
        }

        public async Task<CandidateProfile?> GetWithSkillsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(cp => cp.CandidateSkills)
                .ThenInclude(cs => cs.Skill)
                .FirstOrDefaultAsync(cp => cp.UserId == userId, cancellationToken);
        }

        public async Task<CandidateProfile?> GetWithSkillsAsync(Guid candidateProfileId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(cp => cp.CandidateSkills)
                .ThenInclude(cs => cs.Skill)
                .FirstOrDefaultAsync(cp => cp.Id == candidateProfileId, cancellationToken);
        }

        public async Task<CandidateProfile?> GetWithApplicationsAsync(Guid candidateProfileId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(cp => cp.JobApplications)
                .FirstOrDefaultAsync(cp => cp.Id == candidateProfileId, cancellationToken);
        }

        public async Task<CandidateProfile?> GetWithAllDetailsAsync(Guid candidateProfileId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(cs => cs.CandidateSkills)
                .ThenInclude(cs => cs.Skill)
                .Include(cp => cp.JobApplications)
                .Include(cp => cp.SavedJobs)
                .ThenInclude(sj => sj.Job)
                .FirstOrDefaultAsync(cp => cp.Id == candidateProfileId, cancellationToken);
        }

        public async Task<IReadOnlyList<CandidateProfile>> GetBySkillAsync(Guid skillId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(cp => cp.CandidateSkills.Any(cs => cs.SkillId == skillId))
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<CandidateProfile>> GetOpenToWorkAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(cp => cp.IsOpenToWork == true)
                .OrderByDescending(cp => cp.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
