using Microsoft.EntityFrameworkCore;
using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Domain.Entities;
using SmartHire.Infrastructure.Persistence.Context;
namespace SmartHire.Infrastructure.Persistence.Repositories
{
    public class SkillRepository : BaseRepository<Skill>, ISkillRepository
    {
        public SkillRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Skill?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
        }

        public async Task<Skill> GetOrCreateAsync(string name, CancellationToken cancellationToken = default)
        {
            var skill = await GetByNameAsync(name, cancellationToken);

            if (skill != null)
                return skill;

            var newSkill = new Skill(name);

            await AddAsync(newSkill, cancellationToken);
            return newSkill;
        }

        public async Task<IReadOnlyList<Skill>> GetByNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(s => names.Contains(s.Name))
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Skill>> GetPopularSkillsAsync(int count = 10, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .OrderByDescending(s => s.CandidateSkills.Count)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}
