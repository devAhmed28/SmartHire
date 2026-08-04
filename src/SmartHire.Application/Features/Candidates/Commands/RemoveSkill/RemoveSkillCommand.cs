using MediatR;
using SmartHire.Application.Common.Models;

namespace SmartHire.Application.Features.Candidates.Commands.RemoveSkill
{
    public class RemoveSkillCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
        public string SkillName { get; set; } = string.Empty;
    }
}
