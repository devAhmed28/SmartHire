using FluentValidation;

namespace SmartHire.Application.Features.Jobs.Commands.CreateJob
{
    public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobCommandValidator()
        {
            RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("Company ID is required");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Job title is required")
                .MaximumLength(200).WithMessage("Job title must not exceed 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Job description is required")
                .MaximumLength(4000).WithMessage("Job description must not exceed 4000 characters");

            RuleFor(x => x.Responsibilities)
                .MaximumLength(3000).WithMessage("Responsibilities must not exceed 3000 characters");

            RuleFor(x => x.Requirements)
                .MaximumLength(3000).WithMessage("Requirements must not exceed 3000 characters");

            RuleFor(x => x.SalaryMin)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum salary cannot be negative");

            RuleFor(x => x.SalaryMax)
                .GreaterThanOrEqualTo(0).WithMessage("Maximum salary cannot be negative")
                .GreaterThanOrEqualTo(x => x.SalaryMin)
                .WithMessage("Maximum salary must be greater than or equal to minimum salary");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required")
                .MaximumLength(200).WithMessage("Location must not exceed 200 characters");

            RuleFor(x => x.Vacancies)
                .GreaterThan(0).WithMessage("Vacancies must be greater than 0");

            RuleFor(x => x.Currency)
                .IsInEnum().WithMessage("Invalid currency");

            RuleFor(x => x.JobType)
                .IsInEnum().WithMessage("Invalid job type");

            RuleFor(x => x.WorkMode)
                .IsInEnum().WithMessage("Invalid work mode");

            RuleFor(x => x.ExpirationDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Expiration date must be in the future");

            RuleFor(x => x.SkillIds)
                .NotNull().WithMessage("Skills list cannot be null");
        }
    }
}
