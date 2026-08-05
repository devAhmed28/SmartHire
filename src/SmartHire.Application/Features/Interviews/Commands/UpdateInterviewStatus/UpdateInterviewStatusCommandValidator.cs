using FluentValidation;

namespace SmartHire.Application.Features.Interviews.Commands.UpdateInterviewStatus
{
    public class UpdateInterviewStatusCommandValidator : AbstractValidator<UpdateInterviewStatusCommand>
    {
        public UpdateInterviewStatusCommandValidator()
        {
            RuleFor(x => x.InterviewId)
                .NotEmpty().WithMessage("Interview ID is required");

            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid interview status");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("Notes must not exceed 2000 characters");
        }
    }
}
