using Microsoft.EntityFrameworkCore;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Infrastructure.Persistence.Context;
namespace SmartHire.Infrastructure.Persistence.Repositories
{
    public class JobRepository : BaseRepository<Job>, IJobRepository
    {
        public JobRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Job>> GetByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(j => j.CompanyId == companyId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Job?> GetJobWithApplicationsAsync(Guid jobId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(j => j.JobApplications)
                .FirstOrDefaultAsync(j => j.Id == jobId, cancellationToken);
        }

        public async Task<IReadOnlyList<Job>> GetActiveJobsAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            return await _dbSet
                .Where(j => j.JobStatus == JobStatus.Published && j.ExpirationDate > now)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Job>> SearchJobsAsync(
        string? searchTerm = null,
        JobType? jobType = null,
        WorkMode? workMode = null,
        decimal? minSalary = null,
        decimal? maxSalary = null,
        string? location = null,
        CancellationToken cancellationToken = default)
        {
            // starting with the query 
            var query = _dbSet.AsQueryable();

            // filter by search term (title or description)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(j =>
                j.Title.Contains(searchTerm) ||
                j.Description.Contains(searchTerm));
            }

            // filter by job type
            if (jobType.HasValue)
            {
                query = query.Where(j => j.JobType == jobType.Value);
            }

            // filter by work mode
            if (workMode.HasValue)
            {
                query = query.Where(j => j.WorkMode == workMode.Value);
            }

            // filter by min salary
            if (minSalary.HasValue)
            {
                query = query.Where(j => j.SalaryMin >= minSalary.Value);
            }
            
            // filter by max salary
            if (maxSalary.HasValue)
            {
                query = query.Where(j => j.SalaryMax <= maxSalary.Value);
            }

            // filter by location
            if (!string.IsNullOrWhiteSpace(location))
            {
                query = query.Where(j => j.Location.Contains(location));
            }

            // show only active jobs
            var now = DateTime.Now;
            query = query.Where(j => j.JobStatus == JobStatus.Published && j.ExpirationDate > now);

            // order by most recent first
            query = query.OrderByDescending(j => j.CreatedAt);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Job>> GetExpiredJobsAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            return await _dbSet
                .Where(j => j.ExpirationDate <= now && j.JobStatus == JobStatus.Published)
                .ToListAsync(cancellationToken);
        }
    }
}
