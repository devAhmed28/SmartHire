using FluentValidation;

namespace SmartHire.Application.Features.Admin.Commands.DeleteJob
{
    public class DeleteJobCommandValidator : AbstractValidator<DeleteJobCommand>
    {
        public DeleteJobCommandValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("Job ID is required");
        }
    }

}
