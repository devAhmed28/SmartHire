using FluentValidation;

namespace SmartHire.Application.Features.Jobs.Commands.PublishJob
{
    public class PublishJobCommandValidator : AbstractValidator<PublishJobCommand>
    {
        public PublishJobCommandValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("Job ID is required");

            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required");
        }
    }
}
