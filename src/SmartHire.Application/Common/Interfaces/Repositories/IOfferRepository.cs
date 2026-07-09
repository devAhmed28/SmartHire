using SmartHire.Domain.Entities;

namespace SmartHire.Application.Common.Interfaces.Repositories
{
    public interface IOfferRepository : IRepository<Offer>
    {
        Task<Offer?> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Offer>> GetByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Offer>> GetByCandidateIdAsync(Guid candidateProfileId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Offer>> GetPendingOffersAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Offer>> GetExpiredOffersAsync(CancellationToken cancellationToken = default);
    }
}
