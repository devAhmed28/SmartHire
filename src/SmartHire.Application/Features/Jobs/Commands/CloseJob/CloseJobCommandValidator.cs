using FluentValidation;

namespace SmartHire.Application.Features.Jobs.Commands.CloseJob
{
    public class CloseJobCommandValidator : AbstractValidator<CloseJobCommand>
    {
        public CloseJobCommandValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("Job ID is required");

            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required");
        }
    }
}
