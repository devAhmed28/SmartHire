using FluentValidation;
using SmartHire.Application.Features.Candidates.Commands.RemoveSkill;

namespace SmartHire.Application.Features.Candidates.Commands.RemoveSkill
{
    public class RemoveSkillCommandValidator : AbstractValidator<RemoveSkillCommand>
    {
        public RemoveSkillCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.SkillName)
                .NotEmpty().WithMessage("Skill name is required")
                .MaximumLength(100).WithMessage("Skill name must not exceed 100 characters");
        }
    }
}
