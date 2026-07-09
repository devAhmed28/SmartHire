using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Common.Interfaces.Repositories
{
    public interface IJobRepository : IRepository<Job>
    {
        Task<IReadOnlyList<Job>> GetByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
        Task<Job?> GetJobWithApplicationsAsync(Guid jobId,  CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Job>> GetActiveJobsAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Job>> SearchJobsAsync(
            string? searchTerm = null,
            JobType? jobType = null,
            WorkMode? workMode = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string? location = null,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Job>> GetExpiredJobsAsync(CancellationToken cancellationToken = default);
    }
}
