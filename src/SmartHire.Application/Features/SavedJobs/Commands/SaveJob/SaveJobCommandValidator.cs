using FluentValidation;

namespace SmartHire.Application.Features.SavedJobs.Commands.SaveJob
{
    public class SaveJobCommandValidator : AbstractValidator<SaveJobCommand>
    {
        public SaveJobCommandValidator()
        {
            RuleFor(x => x.CandidateId)
                .NotEmpty().WithMessage("Candidate ID is required");

            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("Job ID is required");
        }
    }
}
