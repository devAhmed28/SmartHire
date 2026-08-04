using FluentValidation;

namespace SmartHire.Application.Features.SavedJobs.Commands.UnsaveJob
{
    public class UnsaveJobCommandValidator : AbstractValidator<UnsaveJobCommand>
    {
        public UnsaveJobCommandValidator()
        {
            RuleFor(x => x.CandidateId)
                .NotEmpty().WithMessage("Candidate ID is required");

            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("Job ID is required");
        }
    }

}
