using SmartHire.Domain.Entities;

namespace SmartHire.Application.Common.Interfaces.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IReadOnlyList<Review>> GetByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Review>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Review?> GetByCompanyAndUserAsync(Guid companyId, Guid userId, CancellationToken cancellationToken = default);
        Task<double> GetAverageRatingAsync(Guid companyId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Review>> GetWithCompanyDetailsAsync(CancellationToken cancellationToken = default);
    }
}
