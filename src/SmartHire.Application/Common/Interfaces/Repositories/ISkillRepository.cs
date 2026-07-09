using SmartHire.Domain.Entities;

namespace SmartHire.Application.Common.Interfaces.Repositories
{
    public interface ISkillRepository : IRepository<Skill>
    {
        Task<Skill?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<Skill> GetOrCreateAsync(string name, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Skill>> GetByNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Skill>> GetPopularSkillsAsync(int count = 10, CancellationToken cancellationToken = default);
    }
}
