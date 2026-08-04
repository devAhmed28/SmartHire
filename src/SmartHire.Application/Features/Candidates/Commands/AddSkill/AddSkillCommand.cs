using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Candidate;

namespace SmartHire.Application.Features.Candidates.Commands.AddSkill
{
    public class AddSkillCommand : IRequest<Result<CandidateProfileResponse>>
    {
        public Guid UserId { get; set; }
        public string SkillName { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
    }
}
