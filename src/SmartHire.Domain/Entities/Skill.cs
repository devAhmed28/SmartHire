using SmartHire.Domain.Common;

namespace SmartHire.Domain.Entities
{
    public class Skill : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public ICollection<JobSkill> JobSkills { get; private set; } = new List<JobSkill>();
        public ICollection<CandidateSkill> CandidateSkills { get; private set;} = new List<CandidateSkill>();
    }
}
