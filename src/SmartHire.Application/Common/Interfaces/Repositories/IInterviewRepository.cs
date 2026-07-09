using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Common.Interfaces.Repositories
{
    public interface IInterviewRepository : IRepository<Interview>
    {
        Task<IReadOnlyList<Interview>> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Interview>> GetByStatusAsync(InterviewStatus status, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Interview>> GetUpcomingForCandidateAsync(Guid candidateId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Interview>> GetUpcomingForCompanyAsync(Guid companyId, CancellationToken cancellationToken = default);
    }
}
