using FluentValidation;

namespace SmartHire.Application.Features.Applications.Commands.WithdrawApplication
{
    public class WithdrawApplicationCommandValidator : AbstractValidator<WithdrawApplicationCommand>
    {
        public WithdrawApplicationCommandValidator()
        {
            RuleFor(x => x.ApplicationId)
                .NotEmpty().WithMessage("Application ID is required");

            RuleFor(x => x.CandidateId)
                .NotEmpty().WithMessage("Candidate ID is required");
        }
    }
}
