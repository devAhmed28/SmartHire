using FluentValidation;

namespace SmartHire.Application.Features.Candidates.Commands.AddSkill
{
    public class AddSkillCommandValidator : AbstractValidator<AddSkillCommand>
    {
        public AddSkillCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.SkillName)
                .NotEmpty().WithMessage("Skill name is required")
                .MaximumLength(100).WithMessage("Skill name must not exceed 100 characters");

            RuleFor(x => x.YearsOfExperience)
                .GreaterThanOrEqualTo(0).WithMessage("Years of experience cannot be negative")
                .LessThanOrEqualTo(50).WithMessage("Years of experience cannot exceed 50");
        }
    }
}
