using SmartHire.Domain.Entities;

namespace SmartHire.Application.Common.Interfaces.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company?> GetCompanyWithJobsAsync(Guid  companyId, CancellationToken cancellationToken = default);
        Task<Company?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Company?> GetCompanyWithDetailsAsync(Guid companyId, CancellationToken cancellationToken = default);
        Task<bool> IsCompanyNameUniqueAsync(string companyName, CancellationToken cancellationToken = default);
    }
}
