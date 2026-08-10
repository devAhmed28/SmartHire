using FluentValidation;

namespace SmartHire.Application.Features.Admin.Commands.ActivateUser
{
    public class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
    {
        public ActivateUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");
        }
    }
}
