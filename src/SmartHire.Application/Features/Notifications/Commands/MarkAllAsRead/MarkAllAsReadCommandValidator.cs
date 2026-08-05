using FluentValidation;

namespace SmartHire.Application.Features.Notifications.Commands.MarkAllAsRead
{
    public class MarkAllAsReadCommandValidator : AbstractValidator<MarkAllAsReadCommand>
    {
        public MarkAllAsReadCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");
        }
    }
}
