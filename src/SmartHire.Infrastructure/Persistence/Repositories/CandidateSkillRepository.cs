using SmartHire.Application.Common.Interfaces.Repositories;
using SmartHire.Infrastructure.Persistence.Context;

namespace SmartHire.Infrastructure.Persistence.Repositories
{
    public class CandidateSkillRepository : BaseRepository<CandidateSkill>, ICandidateSkillRepository
    {
        public CandidateSkillRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
