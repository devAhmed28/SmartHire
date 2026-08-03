using FluentValidation;

namespace SmartHire.Application.Features.Jobs.Commands.DeleteJob
{
    public class DeleteJobCommandValidator : AbstractValidator<DeleteJobCommand>
    {
        public DeleteJobCommandValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("Job ID is required");

            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required");
        }
    }
}
