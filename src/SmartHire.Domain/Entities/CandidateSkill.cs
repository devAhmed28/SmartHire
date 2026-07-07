using SmartHire.Domain.Common;
using SmartHire.Domain.Entities;

public class CandidateSkill : BaseEntity
{
    public Guid CandidateProfileId { get; private set; }

    public Guid SkillId { get; private set; }

    public int YearsOfExperience { get; private set; }
    public CandidateProfile CandidateProfile { get; private set; } = null!;
    public Skill Skill { get; private set; } = null!;
}