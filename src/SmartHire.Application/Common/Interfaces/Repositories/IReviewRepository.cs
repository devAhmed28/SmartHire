using SmartHire.Domain.Entities;

namespace SmartHire.Application.Common.Interfaces.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IReadOnlyList<Review>> GetByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Review>> GetByUserIdWithDetailsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Review?> GetByCompanyAndUserAsync(Guid companyId, Guid userId, CancellationToken cancellationToken = default);
        Task<double> GetAverageRatingAsync(Guid companyId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Review>> GetWithCompanyDetailsAsync(Guid companyId, CancellationToken cancellationToken = default);
    }
}
