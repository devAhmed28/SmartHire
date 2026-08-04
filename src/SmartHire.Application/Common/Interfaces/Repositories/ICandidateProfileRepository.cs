using SmartHire.Domain.Entities;

namespace SmartHire.Application.Common.Interfaces.Repositories
{
    public interface ICandidateProfileRepository : IRepository<CandidateProfile>
    {
        Task<CandidateProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<CandidateProfile?> GetWithSkillsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<CandidateProfile?> GetWithSkillsAsync(Guid candidateProfileId, CancellationToken cancellationToken = default);
        Task<CandidateProfile?> GetWithApplicationsAsync(Guid candidateProfileId, CancellationToken cancellationToken = default);
        Task<CandidateProfile?> GetWithAllDetailsAsync(Guid candidateProfileId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CandidateProfile>> GetBySkillAsync(Guid skillId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CandidateProfile>> GetOpenToWorkAsync(CancellationToken cancellationToken = default);
    }
}
