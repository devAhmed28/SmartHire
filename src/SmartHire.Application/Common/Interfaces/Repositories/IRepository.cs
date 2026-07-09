using SmartHire.Domain.Common;
using System.Linq.Expressions;

namespace SmartHire.Application.Common.Interfaces.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id,  CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

        // Gets entities that match a predicate with optional ordering and pagination
        Task<IReadOnlyList<T>> GetAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity,  CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        // check if any entity matches the predicate
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        // Counts entities that matching a predicate
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null,  CancellationToken cancellationToken = default);
    }
}
