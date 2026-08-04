using FluentValidation;

namespace SmartHire.Application.Features.Applications.Commands.UpdateApplicationStatus
{
    public class UpdateApplicationStatusCommandValidator : AbstractValidator<UpdateApplicationStatusCommand>
    {
        public UpdateApplicationStatusCommandValidator()
        {
            RuleFor(x => x.ApplicationId)
                .NotEmpty().WithMessage("Application ID is required");

            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid application status");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("Notes must not exceed 2000 characters");
        }
    }
}
