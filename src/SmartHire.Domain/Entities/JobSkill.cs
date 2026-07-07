using SmartHire.Domain.Common;

namespace SmartHire.Domain.Entities
{
    public class JobSkill : BaseEntity
    {
        public Guid JobId { get; private set; }
        public Guid SkillId { get; private set; }
        public bool IsRequired { get; private set; }
        public Job Job { get; private set; } = null!;
        public Skill Skill { get; private set; } = null!;
    }
}
