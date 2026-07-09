using SmartHire.Domain.Entities;

namespace SmartHire.Application.Common.Interfaces.Repositories
{
    public interface ISavedJobRepository : IRepository<SavedJob>
    {
        Task<IReadOnlyList<SavedJob>> GetByCandidateProfileAsync(Guid candidateProfileId, CancellationToken cancellationToken = default);
        Task<SavedJob?> GetByCandidateAndJobAsync(Guid candidateProfileId, Guid jobId, CancellationToken cancellationToken = default);
        Task<bool> IsJobSavedAsync(Guid candidateProfileId, Guid jobId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<SavedJob>> GetWithJobDetailsAsync(Guid candidateId, CancellationToken? cancellationToken = default);
    }
}
