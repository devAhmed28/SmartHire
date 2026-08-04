using SmartHire.Domain.Common;
using SmartHire.Domain.Entities;

public class CandidateSkill : BaseEntity
{
    public CandidateSkill(Guid candidateProfileId, Guid skillId, int yearsOfExperience)
    {
        Id = Guid.NewGuid();
        CandidateProfileId = candidateProfileId;
        SkillId = skillId;
        YearsOfExperience = yearsOfExperience;
    }

    private CandidateSkill() {}

    public Guid CandidateProfileId { get; private set; }

    public Guid SkillId { get; private set; }

    public int YearsOfExperience { get; private set; }
    public CandidateProfile CandidateProfile { get; private set; } = null!;
    public Skill Skill { get; private set; } = null!;
}