using FluentValidation;

namespace SmartHire.Application.Features.Applications.Commands.ApplyForJob
{
    public class ApplyForJobCommandValidator : AbstractValidator<ApplyForJobCommand>
    {
        public ApplyForJobCommandValidator()
        {
            RuleFor(x => x.CandidateId)
                .NotEmpty().WithMessage("Candidate ID is required");

            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("Job ID is required");

            RuleFor(x => x.CoverLetter)
                .MaximumLength(3000).WithMessage("Cover letter must not exceed 3000 characters");
        }
    }
}
