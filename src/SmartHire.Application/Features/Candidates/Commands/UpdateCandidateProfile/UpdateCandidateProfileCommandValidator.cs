using FluentValidation;

namespace SmartHire.Application.Features.Candidates.Commands.UpdateCandidateProfile
{
    public class UpdateCandidateProfileCommandValidator : AbstractValidator<UpdateCandidateProfileCommand>
    {
        public UpdateCandidateProfileCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.Bio)
                .MaximumLength(2000).WithMessage("Bio must not exceed 2000 characters");

            RuleFor(x => x.GitHubUrl)
                .MaximumLength(500).WithMessage("GitHub URL must not exceed 500 characters")
                .Matches(@"^https?:\/\/[^\s]+$").WithMessage("GitHub URL must be a valid URL")
                .When(x => !string.IsNullOrEmpty(x.GitHubUrl));

            RuleFor(x => x.LinkedInUrl)
                .MaximumLength(500).WithMessage("LinkedIn URL must not exceed 500 characters")
                .Matches(@"^https?:\/\/[^\s]+$").WithMessage("LinkedIn URL must be a valid URL")
                .When(x => !string.IsNullOrEmpty(x.LinkedInUrl));

            RuleFor(x => x.PortfolioUrl)
                .MaximumLength(500).WithMessage("Portfolio URL must not exceed 500 characters")
                .Matches(@"^https?:\/\/[^\s]+$").WithMessage("Portfolio URL must be a valid URL")
                .When(x => !string.IsNullOrEmpty(x.PortfolioUrl));

            RuleFor(x => x.YearsOfExperience)
                .GreaterThanOrEqualTo(0).WithMessage("Years of experience cannot be negative")
                .LessThanOrEqualTo(50).WithMessage("Years of experience cannot exceed 50");

            RuleFor(x => x.CurrentPosition)
                .MaximumLength(100).WithMessage("Current position must not exceed 100 characters");

            RuleFor(x => x.CurrentLocation)
                .MaximumLength(100).WithMessage("Current location must not exceed 100 characters");

            RuleFor(x => x.ExpectedSalary)
                .GreaterThanOrEqualTo(0).WithMessage("Expected salary cannot be negative");
        }
    }
}
